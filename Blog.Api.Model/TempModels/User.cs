using System;
using System.Collections.Generic;

namespace Blog.Api.Model.TempModels;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 修改信息时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 上次密码错误时间
    /// </summary>
    public DateTime? LastErrorTime { get; set; }

    /// <summary>
    /// 密码错误次数
    /// </summary>
    public int? ErrorCount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public ulong? Sex { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birth { get; set; }

    /// <summary>
    /// 是否逻辑删除，0代表未删除，1代表删除
    /// </summary>
    public ulong? IsDel { get; set; }

    /// <summary>
    /// 是否启用，1代表启用，0代表不启用
    /// </summary>
    public ulong? IsEnable { get; set; }

    /// <summary>
    /// 修改信息用户名
    /// </summary>
    public string? ModifyName { get; set; }

    public virtual ICollection<Userrole> Userroles { get; } = new List<Userrole>();
}
