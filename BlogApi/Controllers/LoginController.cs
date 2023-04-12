using Blog.Api.Common;
using Blog.Api.Common.Json;
using Blog.Api.Common.Token;
using Blog.Api.Common.Utils;
using Blog.Api.IServices;
using Blog.Api.Model;
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

        public LoginController(IUserServices userServices, ILogger<LoginController> logger,
            TokenHelper tokenHelper, IRoleServices roleServices, IRolepermission rolepermission, 
            IUserRoleServices userRoleServices)
        {
            _userServices = userServices;
            _logger = logger;
            _tokenHelper = tokenHelper;
            _roleServices = roleServices;
            _rolepermission = rolepermission;
            _userRoleServices = userRoleServices;
        }

        [HttpPost]
        //public ActionResult<ResultMsg<TokenModel>> Login(string username="admin",string pwd = "admin")
        public ActionResult<ResultMsg<TokenModel>> Login(string username,string pwd)
        {
            var md5Pwd = MD5Helper.MD5Encrypt(pwd);
            var user= _userServices.QueryWhere(u => u.Username == username && u.Pwd == md5Pwd).FirstOrDefault();
            if(user == null)
            {
                return new ResultMsg<TokenModel>()
                {
                    Code=0,
                    Data=new TokenModel()
                    {
                        Token="",
                        Success=false,
                        Msg="账号或密码错误"
                    }
                };
            }

            var roleNames = _roleServices.GetUserRoleNames(user.Id).ToList();

            if(username=="admin"&&pwd=="admin") {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ""));
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim("phone", "123"));
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                string jwt= _tokenHelper.GetToken(claims);
                return Success<TokenModel>(new TokenModel()
                {
                    Success = true,
                    Token = jwt,
                    Expires = 30,
                }) ;
               
            }
             return Fail<TokenModel>(new TokenModel()
            {
                Success = false,
                Token = "",
                Expires = _tokenHelper.Expiration.Second,
            });
        }

        [HttpGet]
        [Authorize(Roles="admin")]
        public ActionResult<ResultMsg<User>> GetUser()
        {
            var user= _userServices.QueryWhere(u => u.Username == "admin").FirstOrDefault();
            return Success<User>(user);
        }

        [HttpPost]
        [Route("/api/register")]
        public async Task<ActionResult<ResultMsg<string>>> Register(RegisterModel registerModel)
        {
            if(registerModel.Pwd!=registerModel.RepaetPwd)
            {
                return Fail<string>("两次输入密码不一致");
            }
            User u = new User()
            {
                Username = registerModel.Username,
                Age = registerModel.Age,
                Sex = (int?)registerModel.Sex,
                Birth=registerModel.Birth,
                Pwd=MD5Helper.MD5Encrypt(registerModel.Pwd),
                CreateTime= DateTime.Now,
            };
            try
            {
                using (_userServices.DbContextTransaction)
                {
                    _userServices.BeginTransaction();

                    var u1= _userServices.QueryWhere(u => u.Username == registerModel.Username).FirstOrDefault();
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
                    var role= _roleServices.QueryWhere(r => r.RoleName == "用户").FirstOrDefault();
                    if (role != null)
                    {
                        Userrole userrole=new Userrole()
                        {
                            RoleId=role.Id,
                            UserId=u.Id,
                            CreateTime= DateTime.Now,
                        };
                        int count2= await _userRoleServices.Add(userrole);
                        if(count2 != 1)
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
    }
}
