using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Userrole
{
    public long Id { get; set; }

    /// <summary>
    /// 外键，关联user表id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 外键，关联角色表id
    /// </summary>
    public long RoleId { get; set; }

    public string? CreateUsername { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? ModifyUsername { get; set; }

    public DateTime? ModifyTime { get; set; }
}
