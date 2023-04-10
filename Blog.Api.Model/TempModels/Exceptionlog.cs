using System;
using System.Collections.Generic;

namespace Blog.Api.Model.TempModels;

public partial class Exceptionlog
{
    public long Id { get; set; }

    public DateTime? Time { get; set; }

    /// <summary>
    /// 异常线程id
    /// </summary>
    public int ThreadId { get; set; }

    /// <summary>
    /// 异常等级
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// 异常信息
    /// </summary>
    public string Message { get; set; } = null!;

    /// <summary>
    /// 异常类型
    /// </summary>
    public string ExceptionType { get; set; } = null!;
}
