using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Collection
{
    public long Id { get; set; }

    /// <summary>
    /// 文章id
    /// </summary>
    public long? ArticleId { get; set; }

    public string? CreateUsername { get; set; }

    public DateTime? CreateTime { get; set; }

    public bool IsDel { get; set; }
}
