using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Services
{
    public class RoleServices : BaseServices<Role>,IRoleServices
    {
        public RoleServices(BlogsqlContext DbContext) : base(DbContext)
        {
        }

        public IQueryable<string> GetUserRoleNames(long userId,out List<long> ids)
        {
            var list = DbContext.Userrole.Join(DbContext.Role, ur => ur.RoleId, r => r.Id,
                (ur, r) => new { ur.RoleId, r.Id, ur.UserId, r.RoleName, ur.IsDel, RDel = r.IsDel, REnable = r.Enable })
                               .Where(c => c.UserId == userId && !c.IsDel&&!c.RDel&&(bool)c.REnable);
            ids= list.Select(c => c.RoleId).ToList();
            return list.Select(c => c.RoleName);
        }
    }
}
