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

                //����ʹ��autofac����Ĭ��IOC����
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                            .ConfigureContainer<ContainerBuilder>(builder =>
                            {
                                //autofacע����򼯺�һЩ����
                                builder.RegisterModule(new AutofacModuleRegister());
                                builder.RegisterModule<AutofacPropertityModuleReg>();
                            });

                //ע�������ļ���ȡ��
                builder.Services.AddSingleton(new AppConfigs(configuration));
                
                //ʹ��NewtonsoftJsonȡ��.net core��Ĭ�����л���
                builder.Services.AddControllers()
                //��Ҫ��װMicrosoft.AspNetCore.Mvc.NewtonsoftJson
                .AddNewtonsoftJson(options =>
                {
                    //������ѭ�����ã�Ҳ���Ƕ��ڹ������ ������б��������л�������Serialize����Ҫ���л�
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //����ʹ��Ĭ�Ͻ�����������������������ʹ��ĳЩ���ԣ�
                    //����һ��CamelCaseNamingStrategy���ԣ��ò���ʹ����һ����Լ�����������ٴ�Сд���л��������������л����������ȫ��Сд��
                    //���忴�����ĵ�
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //ͳһ���ص����л�datetime��ʽ������Dateime�������ڵĸ�ʽ����ϵͳ�Դ��Ļ��ʽ����iso���ڱ�׼{"BirthDay":"2011-01-01T00:00:00"}
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    //��ֵ�Ĵ���NullValueHandling
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    //ת��ʱ��ʱ�����յ���ʱ�䴦��
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    //��ö��ת��Ϊ�ַ���
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddHttpContextAndUser();//ע��httpcontext��globalUser
                builder.Services.AddAuthenticationConfig();//����JWT�������

                builder.Services.AddDbContext<BlogsqlContext>(opt =>
                {
                    opt.UseMySql(configuration.GetConnectionString("MysqlConnStr"), new MySqlServerVersion(new Version()));
                });

                builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));

                //���swaggerЯ��jwt����
                builder.Services.AddSwaggerGen(c =>
                {
                    var scheme = new OpenApiSecurityScheme()
                    {
                        //һ������������д
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
                app.ConfigAppStartAndEnd();//�Զ�����չ��

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                //���JWT��֤
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