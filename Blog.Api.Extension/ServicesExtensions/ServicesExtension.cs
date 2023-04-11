using Blog.Api.Common.HttpContextUser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Extension.ServicesExtensions
{
    /// <summary>
    /// 容器拓展类
    /// </summary>
    public static class ServicesExtension
    {
          public static void  AddHttpContextAndUser(this IServiceCollection serviceProvider)
        {
            if(serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            //注入httpcontext
            serviceProvider.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //注入User对象   
            serviceProvider.AddScoped<IUser, GlobalUser>();
        }
    }
}
