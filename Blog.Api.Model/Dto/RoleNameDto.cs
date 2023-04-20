using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Dto
{
    public class RoleNameDto
    {
        public List<RoleData> RoleDatas { get; set; } = new List<RoleData>();
        public string Msg { get; set; }
    }
}
