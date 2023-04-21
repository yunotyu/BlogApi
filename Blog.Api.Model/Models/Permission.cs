using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Permission
{
    public long Id { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 是否显示菜单为按钮，0代表不显示，1显示
    /// </summary>
    public bool? IsBtn { get; set; }

    /// <summary>
    /// 是否显示该菜单项，0代表不显示，1代表显示
    /// </summary>
    public bool IsShow { get; set; }

    public string? Description { get; set; }

    public string? CreateName { get; set; }

    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 修改人的用户名
    /// </summary>
    public string? ModifyName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? ModifyTime { get; set; }

    /// <summary>
    /// 菜单id
    /// </summary>
    public long MenuId { get; set; }
}
