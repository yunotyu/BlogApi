using System;
using System.Collections.Generic;

namespace Blog.Api.Model.TempModels;

public partial class Comment
{
    public long Id { get; set; }

    /// <summary>
    /// 外键，关联对应的文章
    /// </summary>
    public long ArticleId { get; set; }

    /// <summary>
    /// 评论，500字以内
    /// </summary>
    public string? Comment1 { get; set; }

    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 可以为空，默认为游客访问
    /// </summary>
    public string? CreateName { get; set; }

    public ulong? IsDel { get; set; }
}
