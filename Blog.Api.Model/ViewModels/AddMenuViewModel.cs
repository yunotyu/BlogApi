using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.ViewModels
{
    public class AddMenuViewModel
    {
        public string MenuNames { get; set; } = null!;

        /// <summary>
        /// parentId 为0说明是根节点
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 根节点下面的最大子节点的长度
        /// </summary>
        public int? Depth { get; set; }

        /// <summary>
        /// 对应的路由
        /// </summary>
        public string Url { get; set; } = null!;

        /// <summary>
        /// 子菜单
        /// </summary>
        List<AddMenuViewModel> ChildMenus { get; set; }=new List<AddMenuViewModel>();
    }
}
