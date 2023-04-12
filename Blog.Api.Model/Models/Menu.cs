using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Menu
{
    public long Id { get; set; }

    public string? MenuNames { get; set; }

    /// <summary>
    /// parentId 为0说明是根节点
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 根节点下面的最大子节点的长度
    /// </summary>
    public int? Depth { get; set; }

    /// <summary>
    /// 对应的路由
    /// </summary>
    public string? Url { get; set; }
}
