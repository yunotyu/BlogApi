using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.ViewModels
{
    public class UserRoleViewModel
    {
        public long UserId { get; set; }
        public List<long> RoleIds { get; set; } = new List<long>();
    }
}
