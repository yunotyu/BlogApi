using Blog.Api.Common.HttpContextUser;
using Blog.Api.Common;
using Blog.Api.IServices;
using Blog.Api.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Api.Model.Dto;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Blog.Api.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Blog.Api.Model.ViewModels;
using System.Linq.Expressions;
using Blog.Api.Common.Utils;

namespace Blog.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<UserController> _logger;
        private readonly IUser _globalUser;
        private readonly BlogsqlContext blogsqlContext;

        public UserController(IUserServices userServices, ILogger<UserController> logger, IUser globalUser, BlogsqlContext blogsqlContext)
        {
            _userServices = userServices;
            _logger = logger;
            _globalUser = globalUser;
            this.blogsqlContext = blogsqlContext;
        }

        /// <summary>
        /// 获取当前用户的信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<UserDto>> GetMyInfo()
        {
            var user = _userServices.QueryWhere(u => u.Username == _globalUser.UserName).FirstOrDefault();
            if (user == null)
            {
                return Fail(new UserDto());
            }

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Username = user.Username,
                CreateTime = user.CreateTime,
                UpdateTime = user.UpdateTime,
                LastErrorTime = user.LastErrorTime,
                ErrorCount = user.ErrorCount,
                Remark = user.Remark,
                Sex = user.Sex,
                Age = user.Age,
                Birth = user.Birth,
                IsDel = user.IsDel,
                IsEnable = user.IsEnable,
                ModifyName = user.ModifyName,
            };
            return Success<UserDto>(userDto);
        }

        /// <summary>
        /// 获取数据通过名字
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<List<UserDto>>> GetUserByName(string username)
        {
            var user = _userServices.QueryWhere(u => u.Username == username).FirstOrDefault();
            return Success<List<UserDto>>(AddDtoData(new List<User>() { user }));
        }

        /// <summary>
        /// 获取id获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<List<UserDto>>> GetUserById(long id)
        {
            var user = _userServices.QueryWhere(u => u.Id == id).FirstOrDefault();
            return Success<List<UserDto>>(AddDtoData(new List<User>() { user }));
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<ResultMsg<List<UserDto>>> GetUsers()
        {
            var users = _userServices.QueryAll().ToList();
            return Success<List<UserDto>>(AddDtoData(users));
        }

        /// <summary>
        /// 批量更新用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResultMsg<string>>> UpdateUsers([FromBody] List<User> users)
        {
            try
            {
                List<long> ids = new List<long>();
                users.ForEach(u =>
                {
                    ids.Add(u.Id);
                });
                //把密码设置到要修改的用户里，前端不会传入密码
                var oldUsers = _userServices.QueryWhere(u => ids.Contains(u.Id)).AsNoTracking().ToList();
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Id == oldUsers[i].Id)
                    {
                        users[i].Pwd = oldUsers[i].Pwd;
                        users[i].Id = oldUsers[i].Id;
                        users[i].UpdateTime= DateTime.Now;
                        users[i].ModifyName = _globalUser.UserName;
                    }
                }
                //更新数据
                await _userServices.Update(users);
                return Success("更新数据成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return Success("更新数据失败");
            }

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ResultMsg<string>>> DeleteUsers([FromBody] List<User> users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                users[i].IsDel = true;
                users[i].UpdateTime = DateTime.Now;
                users[i].ModifyName = _globalUser.UserName;
            }
            await _userServices.Delete(users);
            return Success("");
        }

        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<ResultMsg<PageDataDto<UserDto>>> GetPageData(int pageIndex = 1, int pageCount = 5)
        {
            var users = _userServices.QueryPage(pageIndex, pageCount).ToList();
            var userDtos = AddDtoData(users);
            long totalCount = _userServices.Count().Result;
            var pegaDatDto = new PageDataDto<UserDto>()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageData = userDtos,
                TotalPages = (long)Math.Ceiling((double)totalCount / pageCount)

            };
            return Success<PageDataDto<UserDto>>(pegaDatDto);
        }

        /// <summary>
        /// 用户修改自己信息
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "admin,user")]
        public async Task<ActionResult<ResultMsg<string>>> UserModifyInfo([FromBody] UserModifySelfModel model)
        {
            var user = await _userServices.QueryWhere(u => u.Id == model.Id, isTracking: false).FirstOrDefaultAsync();
            if (user == null) return Fail("用户不存在");

            //需要修改的属性名
            Expression<Func<User, object>>[] updateExpArr =
            {
                u=>u.Username,
                u=>u.ModifyName,
                u=>u.Remark,
                u=>u.Sex,
                u=>u.Age,
                u=>u.Birth
            };

            User u = new User
            {
                Id = model.Id,
                Username = model.Username,
                ModifyName = _globalUser.UserName,
                Remark = model.Remark,
                Sex = model.Sex,
                Age = model.Age,
                Birth = model.Birth,
            };

            if (!string.IsNullOrEmpty(model.Pwd))
            {
                if (model.Pwd != model.RepeatPwd)
                {
                    return Fail("两次输入密码不一致");
                }
                u.Pwd=MD5Helper.MD5Encrypt(model.RepeatPwd);
            }
            else
            {
                u.Pwd = user.Pwd;
            }
            u.UpdateTime = DateTime.Now;
            u.ModifyName = _globalUser.UserName;
            bool res = await _userServices.Update(u, updateExpArr);
            if (res) return Success("修改成功");

            return Fail("修改失败");
        }

        /// <summary>
        /// 管理员修改用户密码，需要单独操作
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<ResultMsg<string>>> AdminModifyUserPwd([FromBody] AdminModifyModel model)
        {
            var admin =await _userServices.QueryWhere(u => u.Username == _globalUser.UserName
                                            && u.Id == _globalUser.Id && u.Pwd == MD5Helper.MD5Encrypt(model.AdminPwd))
                                            .AsNoTracking().FirstOrDefaultAsync();
            if (admin == null)
            {
                return Fail("管理员信息错误");
            }

            var user =await _userServices.QueryWhere(u => u.Id == model.UserId).AsNoTracking().FirstOrDefaultAsync();
            if (user == null)
            {
                return Fail("用户不存在");
            }

            var u = new User()
            {
                Id =model.UserId,
            };
            if (!string.IsNullOrEmpty(model.Pwd))
            {
                if (model.Pwd != model.RepeatPwd)
                {
                    return Fail("两次输入密码不一致");
                }
                u.Pwd = MD5Helper.MD5Encrypt(model.Pwd);
            }
            u.UpdateTime = DateTime.Now;
            u.ModifyName = _globalUser.UserName;
            Expression<Func<User, Object>>[] expressions =
            {
                u=>u.Pwd
            };
            bool res=await _userServices.Update(u, expressions);
            if(!res) return Fail("修改失败");

            return Fail("修改成功");
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ResultMsg<string>> AddUsers([FromBody]List<User>users)
        {
            return Success("添加用户成功");
        }

        /// <summary>
        /// 将user数据转换为userDto数据
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        private List<UserDto> AddDtoData(List<User> users)
        {
            List<UserDto> usersDtos = new List<UserDto>();
            foreach (var item in users)
            {
                usersDtos.Add(
                       new UserDto()
                       {
                           Id = item.Id,
                           Username = item.Username,
                           CreateTime = item.CreateTime,
                           UpdateTime = item.UpdateTime,
                           LastErrorTime = item.LastErrorTime,
                           ErrorCount = item.ErrorCount,
                           Remark = item.Remark,
                           Sex = item.Sex,
                           Age = item.Age,
                           Birth = item.Birth,
                           IsDel = item.IsDel,
                           IsEnable = item.IsEnable,
                           ModifyName = item.ModifyName,
                       }
                    );
            }
            return usersDtos;
        }
    }
}
