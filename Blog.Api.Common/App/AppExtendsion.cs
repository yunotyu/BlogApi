using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Blog.Api.Common.App
{
    /// <summary>
    /// WebApplication的拓展类,WebApplication需要安装Serilog.AspNetCore
    /// </summary>
    public static class AppExtendsion
    {
        /// <summary>
        /// IOC容器
        /// </summary>
        public static IServiceProvider RootServices;

        //配置在web启动和结束的操作
        public static void ConfigAppStartAndEnd(this WebApplication app)
        {
            //程序开始时，获取到IOC容器
            app.Lifetime.ApplicationStarted.Register(() => { AppExtendsion.RootServices = app.Services; });
            app.Lifetime.ApplicationStopped.Register(() => { AppExtendsion.RootServices = null; });
        }
    }
}
