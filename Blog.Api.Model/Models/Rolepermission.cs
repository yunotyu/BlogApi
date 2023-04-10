using System;
using System.Collections.Generic;

namespace Blog.Api.Models.TempModels;

public partial class Rolepermission
{
    public long Id { get; set; }

    /// <summary>
    /// 外键，角色表
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// 外键，permission表
    /// </summary>
    public long PermissionId { get; set; }

    /// <summary>
    /// 是否逻辑删除，0代表未删除，1代表删除
    /// </summary>
    public ulong? IsDel { get; set; }

    /// <summary>
    /// 是否启用，1代表启用，0代表不启用
    /// </summary>
    public ulong? Enabel { get; set; }

    public string? CreateUsername { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? ModifyUsername { get; set; }

    public DateTime? ModifyTime { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
