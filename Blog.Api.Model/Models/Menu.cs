using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Api.Model.Models;

public partial class Menu
{
    public string Id { get; set; } = null!;

    public string MenuNames { get; set; } = null!;

    /// <summary>
    /// parentId 为root说明是根节点
    /// </summary>
    public string? ParentId { get; set; }

    /// <summary>
    /// 根节点下面的最大子节点的长度
    /// </summary>
    public int? Depth { get; set; }

    /// <summary>
    /// 对应的路由
    /// </summary>
    public string Url { get; set; } = null!;

    public bool IsDel { get; set; }

    [NotMapped]
    public List<Menu> ChildMenus { get; set; }=new List<Menu>();
}
