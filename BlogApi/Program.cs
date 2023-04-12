using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Blog.Api.Services;
using Microsoft.EntityFrameworkCore;
using Blog.Api.IServices;
using Blog.Api.Models.TempModels;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Blog.Api.Extension;
using Blog.Api.Filter;
using Blog.Api.Common;
using Blog.Api.Common.App;
using Blog.Api.Extension.ServicesExtensions;
using Blog.Api.Extension.Authotizations;
using Microsoft.OpenApi.Models;

namespace BlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Debug("init main");
           
            try
            {
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var builder = WebApplication.CreateBuilder(args);

                //配置使用autofac代替默认IOC容器
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                            .ConfigureContainer<ContainerBuilder>(builder =>
                            {
                                //autofac注册程序集和一些配置
                                builder.RegisterModule(new AutofacModuleRegister());
                                builder.RegisterModule<AutofacPropertityModuleReg>();
                            });

                //注入配置文件读取类
                builder.Services.AddSingleton(new AppConfigs(configuration));
                
                //使用NewtonsoftJson取代.net core的默认序列化类
                builder.Services.AddControllers()
                //需要安装Microsoft.AspNetCore.Mvc.NewtonsoftJson
                .AddNewtonsoftJson(options =>
                {
                    //不处理循环引用，也就是对于关联表的 对象或列表都不会序列化出来。Serialize是仍要序列化
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //设置使用默认解析器，在这个解析器里，可以使用某些策略，
                    //比如一个CamelCaseNamingStrategy策略，该策略使用了一个契约解析器来跟踪大小写序列化的属性名。序列化后的属性名全是小写的
                    //具体看官网文档
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //统一返回的序列化datetime格式，对于Dateime类型日期的格式化，系统自带的会格式化成iso日期标准{"BirthDay":"2011-01-01T00:00:00"}
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //空值的处理NullValueHandling
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //转换时间时，按照当地时间处理
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //将枚举转换为字符串
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddHttpContextAndUser();//注入httpcontext和globalUser
                builder.Services.AddAuthenticationConfig();//配置JWT加密相关

                builder.Services.AddDbContext<BlogsqlContext>(opt =>
                {
                    opt.UseMySql(configuration.GetConnectionString("MysqlConnStr"), new MySqlServerVersion(new Version()));
                });

                builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));

                //添加swagger携带jwt调试
                builder.Services.AddSwaggerGen(c =>
                {
                    var scheme = new OpenApiSecurityScheme()
                    {
                        //一个描述。随意写
                        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                    };
                    c.AddSecurityDefinition("Authorization", scheme);
                    var requirement = new OpenApiSecurityRequirement();
                    requirement[scheme] = new List<string>();
                    c.AddSecurityRequirement(requirement);
                });

                var app = builder.Build();
                app.ConfigAppStartAndEnd();//自定义拓展类

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //添加JWT认证
                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
          
        }
    }
}