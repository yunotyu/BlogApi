using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.IServices
{
    public interface IRoleServices:IBaseServices<Role>
    {
        /// <summary>
        /// 获取用户的所有角色名
        /// </summary>
        /// <returns></returns>
        IQueryable<string> GetUserRoleNames(long userId, out List<long> ids);
    }
}
