using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    public class LoginDto
    {
        public List<RoleMenusDto> RoleMenus { get; set; }=new List<RoleMenusDto>();
        public TokenModel TokenData { get; set; }=new TokenModel();
    }
}
