using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.IServices;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Blog.Api.Model.ViewModels;
using Blog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Controllers
{
    public class MenuController:BaseApiController
    {
        private readonly ILogger<MenuController> logger;
        private readonly IMenuServices _menuServices;
        private readonly IPermissionServices _permissionServices;
        private readonly IUser _globalUser;
        public MenuController(ILogger<MenuController> logger, IMenuServices menuServices,
                            IPermissionServices permissionServices, IUser user)
        {
            this.logger = logger;
            _menuServices = menuServices;
            _permissionServices = permissionServices;
            _globalUser = user;
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public ActionResult<ResultMsg<List<Menu>>> PageData(int pageIndex=1,int pageSize=5)
        {
            var menus = _menuServices.QueryPage(pageIndex,pageSize,m=>!m.IsDel).ToList();
            return Success(menus);
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<Menu>>> GetMenuByPermissionId(long permissionId)
        {
            var p=await _permissionServices.QueryWhere(p=>p.Id == permissionId).FirstOrDefaultAsync();
            if (p == null)
            {
                return Fail<Menu>(null, "权限不存在");
            }
            var m= await _menuServices.QueryWhere(m => m.Id == p.MenuId).FirstOrDefaultAsync();
            if(m == null)
            {
                return Fail<Menu>(null, "菜单项不存在");
            }
            return Success(m);
        }

        /// <summary>
        /// 获取子菜单
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<List<Menu>>>> GetMenuChildNode(long parentId)
        {
            var childMenus =_menuServices.QueryWhere(m => m.ParentId == parentId).ToList();
            return Success(childMenus);
        }

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<List<Menu>>>> GetMenuParentNode(long parentId)
        {
            var childMenus = _menuServices.QueryWhere(m => m.Id == parentId).ToList();
            return Success(childMenus);
        }

        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Add(List<AddMenuViewModel> menus)
        {
            //父节点名是否重复
            var menuNames = menus.Select(m => m.MenuNames).ToList();
            var existNames= _menuServices.QueryWhere(m => menuNames.Contains(m.MenuNames)).Select(m=>m.MenuNames).ToList();
            var exceptNames= menuNames.Except(existNames);
            if (exceptNames.Count() > 0)
            {
                return Fail("", $"{string.Join(",", exceptNames.ToArray())}:根菜单已经存在");
            }

            //子节点的父节点是否存在

            return Success("ok");
        }
    }
}
