using Blog.Api.Model;
using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.IServices
{
    public interface IPermissionServices:IBaseServices<Permission>
    {
        /// <summary>
        /// 获取权限表对应的菜单
        /// </summary>
        /// <returns></returns>
        public List<PermissionMenuData> GetPermissionMenu();
    }
}
