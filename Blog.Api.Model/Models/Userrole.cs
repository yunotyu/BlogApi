using System;
using System.Collections.Generic;

namespace Blog.Api.Models.TempModels;

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

    /// <summary>
    /// 是否逻辑删除，0代表未删除，1代表删除
    /// </summary>
    public ulong? IsDel { get; set; }

    /// <summary>
    /// 是否启用，1代表启用，0代表不启用
    /// </summary>
    public ulong? Enable { get; set; }

    public string? CreateUsername { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? ModifyUsername { get; set; }

    public DateTime? ModifyTime { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
