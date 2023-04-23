using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Permissonmenu
{
    public long Id { get; set; }

    public long? PermissonId { get; set; }

    public string? MenuId { get; set; }

    public string? ModifyName { get; set; }

    public DateTime? ModifyTime { get; set; }
}
