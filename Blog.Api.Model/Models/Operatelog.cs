using System;
using System.Collections.Generic;

namespace Blog.Api.Models.TempModels;

public partial class Operatelog
{
    public long Id { get; set; }

    /// <summary>
    /// 访问的区域
    /// </summary>
    public string? Area { get; set; }

    /// <summary>
    /// 访问的控制器名
    /// </summary>
    public string? ControllerName { get; set; }

    /// <summary>
    /// 访问的action方法
    /// </summary>
    public string? ActionName { get; set; }

    /// <summary>
    /// 来访的ip
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    /// 访问的时间
    /// </summary>
    public DateTime? LogTime { get; set; }

    /// <summary>
    /// 可以为空，如果是没登录的用户
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// 0:未删除，1:已删除
    /// </summary>
    public ulong? IsDel { get; set; }
}
