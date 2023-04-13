﻿using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.Common.Json;
using Blog.Api.Common.Token;
using Blog.Api.Common.Utils;
using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Blog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : BaseApiController
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<LoginController> _logger;
        private readonly IRoleServices _roleServices;
        private readonly IRolepermission _rolepermission;
        private readonly IUserRoleServices _userRoleServices;
        private readonly TokenHelper _tokenHelper;
        private readonly IUser _globalUser;

        public LoginController(IUserServices userServices, ILogger<LoginController> logger,
            TokenHelper tokenHelper, IRoleServices roleServices, IRolepermission rolepermission,
            IUserRoleServices userRoleServices, IUser globalUser)
        {
            _userServices = userServices;
            _logger = logger;
            _tokenHelper = tokenHelper;
            _roleServices = roleServices;
            _rolepermission = rolepermission;
            _userRoleServices = userRoleServices;
            _globalUser = globalUser;
        }

        [HttpPost]
        public ActionResult<ResultMsg<LoginDto>> Login([FromBody]LoginModel loginModel)
        {
            string username=loginModel.Username;
            string pwd=loginModel.Pwd;
            var md5Pwd = MD5Helper.MD5Encrypt(pwd);
            var user = _userServices.QueryWhere(u => u.Username == username && u.Pwd == md5Pwd).FirstOrDefault();
            if (user == null)
            {
                return new ResultMsg<LoginDto>()
                {
                    Code = 0,
                    Data = new LoginDto()
                    {
                        TokenData=new TokenModel()
                        {
                            Token = "",
                            Success = false,
                            Msg = "账号或密码错误"
                        }
                    }
                };
            }
            //角色id
            List<long> rIds=new List<long>();
            //获取所有角色
            var roleNames = _roleServices.GetUserRoleNames(user.Id,out rIds).ToList();
            if(roleNames.Count == 0) {
                return new ResultMsg<LoginDto>()
                {
                    Code = 0,
                    Data = new LoginDto()
                    {
                        TokenData = new TokenModel()
                        {
                            Token = "",
                            Success = false,
                            Msg = "角色不存在"
                        }
                    }
                };
            }

            //获取所有角色的权限菜单
            List<RoleMenusDto> roleMenusDtos= _rolepermission.GetMenuUrls(rIds);

            //登录成功，返回JWT
            List<Claim> claims = new List<Claim>();
            //添加多个角色
            for (int i = 0; i < roleNames.Count; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleNames[i]));
            }

            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            string jwt = _tokenHelper.GetToken(claims);
            return new ResultMsg<LoginDto>()
            {
                Code = 0,
                Data = new LoginDto()
                {
                    RoleMenus= roleMenusDtos,
                    TokenData = new TokenModel()
                    {
                        Token = jwt,
                        Success = false,
                        Msg = "获取token成功"
                    }
                }
            };
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<User>> GetUserByName(string username)
        {
            var user = _userServices.QueryWhere(u => u.Username == username).FirstOrDefault();
            return Success<User>(user);
        }

        /// <summary>
        /// 获取当前用户的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<User>> GetMyInfo()
        {
            var user = _userServices.QueryWhere(u => u.Username == _globalUser.UserName).FirstOrDefault();
            return Success<User>(user);
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles ="admin")]
        public ActionResult<ResultMsg<List<User>>> GetUsers()
        {
            var users= _userServices.QueryAll().ToList();
            return Success<List<User>>(users);  
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/register")]
        public async Task<ActionResult<ResultMsg<string>>> Register(RegisterModel registerModel)
        {
            if (registerModel.Pwd != registerModel.RepaetPwd)
            {
                return Fail<string>("两次输入密码不一致");
            }
            User u = new User()
            {
                Username = registerModel.Username,
                Age = registerModel.Age,
                Sex = (int?)registerModel.Sex,
                Birth = registerModel.Birth,
                Pwd = MD5Helper.MD5Encrypt(registerModel.Pwd),
                CreateTime = DateTime.Now,
            };
            try
            {
                using (_userServices.DbContextTransaction)
                {
                    _userServices.BeginTransaction();

                    var u1 = _userServices.QueryWhere(u => u.Username == registerModel.Username).FirstOrDefault();
                    if (u1 != null)
                    {
                        _logger.LogDebug("用户已存在");
                        return Fail<string>("用户已存在！");
                    }
                    int count = await _userServices.Add(u);
                    if (count != 1)
                    {
                        _logger.LogError("插入用户失败");
                        return Fail<string>("注册失败，请联系管理员!");
                    }
                    //注册的用户统一为user角色
                    var role = _roleServices.QueryWhere(r => r.RoleName == "user").FirstOrDefault();
                    if (role != null)
                    {
                        Userrole userrole = new Userrole()
                        {
                            RoleId = role.Id,
                            UserId = u.Id,
                            CreateTime = DateTime.Now,
                        };
                        int count2 = await _userRoleServices.Add(userrole);
                        if (count2 != 1)
                        {
                            _userServices.RollbackTransaction();
                            _logger.LogError("插入角色失败");
                            return Fail<string>("注册失败，请联系管理员!");
                        }
                    }
                    _userServices.Commit();

                }
            }
            catch (Exception ex)
            {
                _userServices.RollbackTransaction();
                _logger.LogError(ex.ToString(), ex);
            }


            return Success<string>("注册成功");
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<ResultMsg<PageDataDto<User>>>GetPageData(int pageIndex=1,int pageCount=5)
        {
            var users= _userServices.QueryPage(pageIndex, pageCount).ToList();
            long totalCount= _userServices.Count().Result;
            var pegaDatDto = new PageDataDto<User>()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageData = users,
            };
            return Success<PageDataDto<User>>(pegaDatDto);
        }
    }
}
