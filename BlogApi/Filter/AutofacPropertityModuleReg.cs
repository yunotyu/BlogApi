using Autofac;
using BlogApi;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Filter
{
    public class AutofacPropertityModuleReg:Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //不对控制器类进行属性注入，继承ControllerBase的类
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired();

        }
    }
}
