using Blog.Api.IServices;
using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    public class RolepermissionServices : BaseServices<Rolepermission>,IRolepermission
    {
        public RolepermissionServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }

        /// <summary>
        /// 获取角色能访问的菜单项
        /// </summary>
        /// <param name="rIds">当前用户的角色ID</param>
        /// <returns></returns>
        public List<RoleMenusDto> GetMenuUrls(List<long> rIds)
        {
            //先关联permissions表和Rolepermissions表，获取每个角色的权限
            var permissions = DbContext.Permissions.Join(DbContext.Rolepermissions, p => p.Id, rp => rp.PermissionId, (p, rp) =>
            new
            {
                rp.RoleId,
                rp.PermissionId,
                rp.IsDel,
                rp.Enabel,
                p.MenuId,
                p.IsBtn,
                p.IsShow,
                p.Icon,
                PIsDel = p.IsDel,
                PEnable = p.Enable
            }).Where(t => rIds.Contains(t.RoleId) && (bool)t.Enabel && (bool)t.PEnable && !t.IsDel && !t.PIsDel);

            //再关联menus，获取每个权限能访问的菜单项
            var menus = permissions.Join(DbContext.Menus, t => t.MenuId, m => m.Id, (t, m) =>
             new
             {
                 t.RoleId,
                 t.PermissionId,
                 t.MenuId,
                 t.IsBtn,
                 t.IsShow,
                 t.Icon,
                 m.Id,
                 m.MenuNames,
                 m.Url,
                 m.IsDel
             }).Where(k=>!k.IsDel).ToList();

            List<RoleMenusDto> list= new List<RoleMenusDto>();
            foreach (var item in menus)
            {
                list.Add(new RoleMenusDto()
                {
                    RoleID=item.RoleId,
                    PermissionId=item.PermissionId,
                    MenuId=item.MenuId,
                    IsBtn=(bool)item.IsBtn,
                    IsShow=item.IsShow, 
                    Icon=item.Icon,
                    MenuName=item.MenuNames,
                    Url=item.Url,
                });
            }
            return list;
        }
    }
}
