using Blog.Api.IServices;
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

        public IQueryable<string> GetUserRoleNames(long userId)
        {
            var list= DbContext.Userroles.Join(DbContext.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.RoleId, r.Id, ur.UserId, r.RoleName })
                               .Where(c => c.UserId == userId);
            return list.Select(c => c.RoleName);
        }
    }
}
