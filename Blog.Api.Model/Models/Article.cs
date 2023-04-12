using System;
using System.Collections.Generic;

namespace Blog.Api.Model.Models;

public partial class Article
{
    public long Id { get; set; }

    /// <summary>
    /// 发布文章的用户名
    /// </summary>
    public string CreateUsername { get; set; } = null!;

    /// <summary>
    /// 文章标题
    /// </summary>
    public string ArticleTitle { get; set; } = null!;

    /// <summary>
    /// 文章分类，以--分隔
    /// </summary>
    public string ArticleKind { get; set; } = null!;

    /// <summary>
    /// 文章内容，注意长度限制，6000字以下
    /// </summary>
    public string? ArticleContent { get; set; }

    /// <summary>
    /// 文章发布时间
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 文章访问次数
    /// </summary>
    public int? LookCount { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public int? LikeCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    public int? CommentCount { get; set; }

    public bool? IsDel { get; set; }
}
