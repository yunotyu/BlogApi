using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="用户名不能为空")]
        public string Username { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string RepaetPwd { get; set; }

        public int Age { get; set; }
        public SexEnum Sex { get; set; } = SexEnum.Man;
        public DateTime Birth { get; set; }=new DateTime(1990,1,1);
    }

    public enum SexEnum
    {
        Man,
        Women
    }
}
