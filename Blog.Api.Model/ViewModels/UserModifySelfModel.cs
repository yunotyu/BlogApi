using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.ViewModels
{
    public class UserModifySelfModel
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string Pwd { get; set; }
        public string RepeatPwd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 性别 0:男  1女
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birth { get; set; }
    }
}
