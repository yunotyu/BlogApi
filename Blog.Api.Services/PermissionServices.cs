using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {
        public PermissionServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }

        public List<PermissionMenuData> GetPermissionMenu()
        {
            return DbContext.Permission.Join(DbContext.Menus, p => p.MenuId, m => m.Id, (p, m) => new PermissionMenuData
            {
                Permission = p,
                Menu = m,
            }).ToList();
        }
    }
}
