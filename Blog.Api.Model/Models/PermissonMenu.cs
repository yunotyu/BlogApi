using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Model.Models
{
    public class PermissonMenu
    {
        public long Id { get; set; }

        public long? PermissionId { get; set; }
        public long? MenuId { get; set; }
        public string? ModifyName { get; set; }
        public DateTime? ModifyTime { get; set; }

    }
}
