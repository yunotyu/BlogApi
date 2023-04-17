using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.ViewModels
{
    public class AdminModifyModel
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "管理员密码不能为空")]
        public string AdminPwd { get; set; }

        [Required(ErrorMessage = "新密码不能为空")]

        public string Pwd { get; set; }

        [Required(ErrorMessage = "重复密码不能为空")]
        public string RepeatPwd { get; set; }
    }
}
