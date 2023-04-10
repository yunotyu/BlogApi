using System;
using System.Collections.Generic;

namespace Blog.Api.Models.TempModels;

public partial class Advertisement
{
    public long Id { get; set; }

    /// <summary>
    /// 广告标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 广告图片
    /// </summary>
    public string? ImgUrl { get; set; }

    /// <summary>
    /// 广告链接
    /// </summary>
    public string? Url { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreateTime { get; set; }

    public ulong? IsDel { get; set; }
}
