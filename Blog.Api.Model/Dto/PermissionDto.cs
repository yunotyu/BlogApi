using Blog.Api.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    public class PermissionDto
    {
        public Permission Permission { get; set; }

        public List<Menu> Menus { get; set; }=new List<Menu>();
    }
}
