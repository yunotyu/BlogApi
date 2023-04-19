using Blog.Api.Common;
using Blog.Api.IServices;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    public class RoleController:BaseApiController
    {
        private readonly IRoleServices _roleServices;
        private readonly IRolepermission _rolepermission;
        private readonly IUserRoleServices _userRoleServices;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleServices roleServices, ILogger<RoleController> logger, IRolepermission rolepermission,
                                IUserRoleServices userRoleServices)
        {
            _roleServices = roleServices;
            _logger = logger;
            _rolepermission = rolepermission;
            _userRoleServices = userRoleServices;
        }

        [HttpPost]
        public ActionResult<ResultMsg<PageDataDto<Role>>> PageData(int pageIndex=1, int pageCount=5)
        {
            var roles = _roleServices.QueryPage(pageIndex, pageCount).ToList();
            long totalCount = _roleServices.Count().Result;
            var pegaDatDto = new PageDataDto<Role>()
            {
                PageCount = pageCount,
                PageIndex = pageIndex,
                TotalCount = totalCount,
                PageData = roles,
                TotalPages = (long)Math.Ceiling((double)totalCount / pageCount)

            };
            return Success<PageDataDto<Role>>(pegaDatDto);
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult<ResultMsg<string>> GetUserRoleById(int userId)
        {
            var urs=_userRoleServices.QueryWhere(ur => ur.UserId == userId).ToList();
            return null;
        }
    }
}
