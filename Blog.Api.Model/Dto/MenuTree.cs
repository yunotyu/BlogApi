using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    public class MenuTree
    {
        public long Id { get; set; }

        public string MenuNames { get; set; } = null!;

        /// <summary>
        /// parentId 为0说明是根节点
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 根节点下面的最大子节点的长度
        /// </summary>
        public int? Depth { get; set; }

        /// <summary>
        /// 对应的路由
        /// </summary>
        public string Url { get; set; } = null!;

        public bool IsDel { get; set; }

        public List<MenuTree> ChildTree { get; set; } = new List<MenuTree>();
    }
}
