using Blog.Api.Common.HttpContextUser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common.App
{
    /// <summary>
    /// 内部App类，获取一些Http请求相关的对象
    /// </summary>
    public class InnerApp
    {
        /// <summary>
        /// IOC容器对象
        /// </summary>
        public static IServiceProvider RootServices = AppExtendsion.RootServices;

        /// <summary>
        /// 内部上下文对象
        /// </summary>
        public static HttpContext InnerHttpContext { get; set; }=RootServices?.GetService<IHttpContextAccessor>()?.HttpContext;

        /// <summary>
        /// 内部用户对象
        /// </summary>
        public static IUser User { get; set; }= InnerHttpContext == null?null:RootServices?.GetService<IUser>();

    }
}
