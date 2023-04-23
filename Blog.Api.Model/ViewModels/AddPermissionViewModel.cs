using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.ViewModels
{
    public class AddPermissionViewModel
    {
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否显示菜单为按钮，0代表不显示，1显示
        /// </summary>
        public bool? IsBtn { get; set; }

        /// <summary>
        /// 是否显示该菜单项，0代表不显示，1代表显示
        /// </summary>
        public bool IsShow { get; set; }

        public string? Description { get; set; }

        /// <summary>
        /// 菜单id
        /// </summary>
        public string MenuId { get; set; }
    }
}
