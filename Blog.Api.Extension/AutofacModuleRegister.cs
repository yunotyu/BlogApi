using Autofac;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Extension
{
    //注册程序集到autofac里，一次性注册全部类
    public class AutofacModuleRegister: Autofac.Module
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            var basePath=AppContext.BaseDirectory;
            var servicePath = Path.Combine(basePath, "Blog.Api.Services.dll");

            if(!File.Exists(servicePath)) 
            {
                var msg = "Blog.Api.Services.dll程序集不存在";
                logger.Error(msg);
                throw new Exception(msg);
            }

            Assembly assembly = Assembly.LoadFrom(servicePath);
            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()//使用接口注册所有类型
                .InstancePerDependency()//每次从IOC容器获取的都是新实例
                .PropertiesAutowired();//对这些类进行属性注入


        }
    }
}
