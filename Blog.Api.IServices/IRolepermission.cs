using Blog.Api.Model.Dto;
using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.IServices
{
    public interface IRolepermission:IBaseServices<Rolepermission>
    {
        /// <summary>
        /// 获取当前用户能访问的前端路由
        /// </summary>
        /// <param name="rIds"></param>
        /// <returns></returns>
        public List<RoleMenusDto> GetMenuUrls(List<long> rIds);
    }
}
