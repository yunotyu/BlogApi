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
        public ActionResult<ResultMsg<User>> GetMyInfo()
        {
            var user = _userServices.QueryWhere(u => u.Username == _globalUser.UserName).FirstOrDefault();
            return Success<User>(user);
        }

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public ActionResult<ResultMsg<User>> GetUserByName(string username)
        {
            var user = _userServices.QueryWhere(u => u.Username == username).FirstOrDefault();
            return Success<User>(user);
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<ResultMsg<List<User>>> GetUsers()
        {
            var users = _userServices.QueryAll().ToList();
            return Success<List<User>>(users);
        }

        [HttpPost]
        public async Task<ActionResult<ResultMsg<List<User>>>> UpdateUsers([FromBody] List<User> users)
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
                }
            }
            //更新数据
            await _userServices.Update(users);


            //重新查询数据返回给前端
            var newUsers = _userServices.QueryWhere(u => ids.Contains(u.Id)).ToList();

            return Success(newUsers);
        }

        [HttpPost]
        public async Task<ActionResult<ResultMsg<string>>> DeleteUser(List<User>users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                users[i].IsDel = true;
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
        public ActionResult<ResultMsg<PageDataDto<User>>> GetPageData(int pageIndex = 1, int pageCount = 5)
        {
            var users = _userServices.QueryPage(pageIndex, pageCount).ToList();
            long totalCount = _userServices.Count().Result;
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
