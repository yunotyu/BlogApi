using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    public class RoleDto
    {
        public long Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; } = null!;

        /// <summary>
        /// 角色描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 是否启用，1代表启用，0代表不启用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 创建的用户名
        /// </summary>
        public string CreateUsername { get; set; } = null!;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }
}
