using System;
using System.Collections.Generic;

namespace Blog.Api.Model.TempModels;

public partial class Role
{
    public long Id { get; set; }

    /// <summary>
    /// 角色名
    /// </summary>
    public string RoleName { get; set; } = null!;

    /// <summary>
    /// 角色描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 角色等级
    /// </summary>
    public int? Level { get; set; }

    /// <summary>
    /// 是否启用，1代表启用，0代表不启用
    /// </summary>
    public ulong? Enable { get; set; }

    /// <summary>
    /// 是否逻辑删除，0代表未删除，1代表删除
    /// </summary>
    public ulong? IsDel { get; set; }

    /// <summary>
    /// 创建的用户名
    /// </summary>
    public string CreateUsername { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifyTime { get; set; }

    public virtual ICollection<Rolepermission> Rolepermissions { get; } = new List<Rolepermission>();

    public virtual ICollection<Userrole> Userroles { get; } = new List<Userrole>();
}
