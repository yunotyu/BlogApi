using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Blog.Api.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using System.Linq;

namespace Blog.Api.Controllers
{
    public class PermissionController : BaseApiController
    {
        private readonly IPermissionServices _permissionServices;
        private readonly IRolepermission _rolepermission;
        private readonly IRoleServices _roleServices;
        private readonly ILogger<PermissionController> _logger;
        private readonly IUser _globalUser;
        private readonly IMenuServices _menuServices;
        private readonly IPermissionMenuServices _permissionMenuServices;

        public PermissionController(IPermissionServices permissionServices, IRolepermission rolepermission,
                        IRoleServices roleServices, ILogger<PermissionController> logger, IUser globalUser, 
                        IMenuServices menuServices, IPermissionMenuServices permissionMenuServices)
        {
            _permissionServices = permissionServices;
            _rolepermission = rolepermission;
            _roleServices = roleServices;
            _logger = logger;
            _globalUser = globalUser;
            _menuServices = menuServices;
            _permissionMenuServices = permissionMenuServices;
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public ActionResult<ResultMsg<List<PermissionMenuData>>> PageData(int pageIndex=1,int pageSize=5)
        {
            var permissionMenus = _permissionServices.GetPermissionMenu();
            var pageData= permissionMenus.Skip(pageIndex-1).Take(pageSize).ToList();
            return Success(pageData);
        }

        [HttpGet]
        //[Authorize(Roles ="admin")]
        public ActionResult<ResultMsg<List<Permission>>> GetPermissionByRoleId(long roleId)
        {
            var role = _roleServices.QueryWhere(r=>r.Id==roleId).FirstOrDefault();
            if (role == null) return Fail<List<Permission>>(null,"角色不存在");
            var pIds= _rolepermission.QueryWhere(r=>r.Id== roleId).ToList().Select(p=>p.PermissionId);
            var ps= _permissionServices.QueryWhere(p => pIds.Contains(p.Id)).ToList();

            return Success(ps);
        }

        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Update(List<Permission> ps)
        {
            //Permission是否存在id
            var pIds =ps.Select(p=>p.Id).ToList();
            var exitsIds= _permissionServices.QueryWhere(p => pIds.Contains(p.Id)).Select(p=>p.Id).ToList();
            var exceptIds= pIds.Except(exitsIds).ToList();
            if (exceptIds.Count > 0)
            {
                return Fail("",$"{string.Join(",", exceptIds)}: PermissionId不存在");
            }

            //menu表是否存在id
            List<long> menuIds = ps.Select(m=>m.MenuId).ToList();
            List<long> exitsMenuIds = _menuServices.QueryWhere(m => menuIds.Contains(m.Id)).Select(m => m.Id).ToList();
            List<long> exceptMenuIds = menuIds.Except(exitsMenuIds).ToList();
            if (exceptMenuIds.Count > 0)
            {
                return Fail("", $"{string.Join(",", exceptMenuIds)}: MenuId不存在");
            }

            var updatePermission = new List<Permission>();  
            foreach (var p in ps)
            {
                updatePermission.Add(new Permission
                {
                    Id= p.Id,
                    Icon=p.Icon,
                    IsBtn=p.IsBtn,
                    Description=p.Description,
                    CreateName=_globalUser.UserName,
                    CreateTime=DateTime.Now,
                    ModifyName=_globalUser.UserName,
                    ModifyTime=DateTime.Now,
                    MenuId=p.MenuId,
                });
            }
            await _permissionServices.Update(updatePermission);
            return Success("","ok");
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Add(List<AddPermissionViewModel> permissions)
        {
            string index="";
            for (int i = 0; i < permissions.Count; i++)
            {
                var p =await _permissionServices.QueryWhere(p => p.Icon == permissions[i].Icon && p.IsBtn == permissions[i].IsBtn && p.MenuId == permissions[i].MenuId)
                    .FirstOrDefaultAsync();
                if (p != null)
                {
                    index += (i+1).ToString() + ",";
                }
            }
            if (!string.IsNullOrEmpty(index))
            {
                return Fail("", $"{index}:记录已经存在");
            }

            //menu表是否存在id
            List<long> menuIds = permissions.Select(m => m.MenuId).ToList();
            List<long> exitsMenuIds = _menuServices.QueryWhere(m => menuIds.Contains(m.Id)).Select(m => m.Id).ToList();
            List<long> exceptMenuIds = menuIds.Except(exitsMenuIds).ToList();
            if (exceptMenuIds.Count > 0)
            {
                return Fail("", $"{string.Join(",", exceptMenuIds)}: MenuId不存在");
            }

            var addPermissions = new List<Permission>();

            for (int i = 0; i < permissions.Count; i++)
            {
                addPermissions.Add(new Permission()
                {
                    Icon = permissions[i].Icon,
                    IsBtn = permissions[i].IsBtn,
                    IsShow= permissions[i].IsShow,
                    MenuId = permissions[i].MenuId,
                    ModifyName=_globalUser.UserName,
                    ModifyTime=DateTime.Now,
                    Description = permissions[i].Description,
                    CreateName=_globalUser.UserName,
                    CreateTime=DateTime.Now
                });
            }
            await _permissionServices.AddBulk(addPermissions);
            return Success("");
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles ="admin")]
        public async Task<ActionResult<ResultMsg<string>>> Delete(List<long>pIds )
        {
            var ps = _permissionServices.QueryWhere(p => pIds.Contains(p.Id)).ToList();
            var exitsIds = ps.Select(p => p.Id).ToList();
            var exceptIds = pIds.Except(exitsIds).ToList();
            if (exceptIds.Count > 0)
            {
                return Fail($"{string.Join(",", exceptIds)}:id不存在");
            }
            var delPs=new List<Permission>();
            for (int i = 0; i < pIds.Count; i++)
            {
                delPs.Add(new Permission()
                {
                    Id = pIds[i],
                });
            }
            await _permissionServices.BulkDeleteData(delPs);
            return Success("","ok");
        }
    }
}
