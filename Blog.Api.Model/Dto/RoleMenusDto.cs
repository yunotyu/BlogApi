using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    /// <summary>
    /// 返回给前端的角色对应菜单类
    /// </summary>
    public class RoleMenusDto
    {
        public long RoleID { get; set; }
        public long PermissionId { get; set; }
        public long MenuId { get; set; }
        public bool? IsBtn { get; set; }
        public bool IsShow { get; set; }
        public string? Icon { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
    }
}
