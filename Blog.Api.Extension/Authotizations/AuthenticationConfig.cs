using Blog.Api.Common;
using Blog.Api.Common.HttpContextUser;
using Blog.Api.Extension.Authotizations.Policy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Extension.Authotizations
{
    /// <summary>
    /// 配置JWT认证，IServiceCollection拓展类
    /// </summary>
    public static class AuthenticationConfig
    {
        public static void AddAuthenticationConfig(this IServiceCollection services)
        {
            if(services==null) throw new ArgumentNullException(nameof(services));

            var sercetKey = AppConfigs.ReadAppConfig(new string[] { "JWT", "SecretKey" });
            var tempByte=System.Text.Encoding.Default.GetBytes(sercetKey);
            var base64Key=Convert.ToBase64String(tempByte);//对key再次进行base64加密
            AppConfigs.Base64Key = base64Key;

            var issuser = AppConfigs.ReadAppConfig(new string[] { "JWT", "Issuer" });//发行人
            var audience = AppConfigs.ReadAppConfig(new string[] { "JWT", "Audience" });//订阅人

            //设置token验证的时候，使用的参数
            var tokenValidParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,//获取或设置一个布尔值，该值控制是否调用对securityToken签名的SecurityKey的验证。
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(base64Key)),//验证token的时候，使用的key
                ValidateIssuer=true,//验证Issuser字段
                ValidIssuer=issuser,//发行人
                ValidAudience=audience,//订阅人
                ValidateAudience=true,//验证audience字段
                ValidateLifetime=true,//验证token是否过期
                RequireExpirationTime=true,//指示token必须有过期值
                //设置时钟漂移，可以在验证token有效期时，允许一定的时间误差（如时间刚达到token中exp，但是允许未来5分钟内该token仍然有效）。
                //默认为300s，即5min。本例jwt的签发和验证均是同一台服务器，所以这里就不需要设置时钟漂移了。
                ClockSkew =TimeSpan.FromSeconds(30),
            };

            services.AddAuthentication(a =>
            {
                a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;//使用jwt默认的验证方案
                //添加jwt验证失败自定义的401,403返回报文
                a.DefaultChallengeScheme = nameof(CustomAuthotiResponseHandle);
                a.DefaultForbidScheme = nameof(CustomAuthotiResponseHandle);
            })
                //将自定义的handler添加到验证服务中
             .AddScheme<AuthenticationSchemeOptions, CustomAuthotiResponseHandle>(nameof(CustomAuthotiResponseHandle), o => { })
             .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = tokenValidParams;

                //在jwt验证过程中，触发对应事件执行的操作
                opt.Events = new JwtBearerEvents
                {
                    //token错误时，返回加上头Token-Error
                    OnChallenge = context =>
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    //验证失败时
                    OnAuthenticationFailed = context =>
                    {
                        var jwtHandler = new JwtSecurityTokenHandler();
                        //获取token
                        var token = context.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                        if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
                        {
                            var jwtToken = jwtHandler.ReadJwtToken(token);

                            //issuse字段不对
                            if (jwtToken.Issuer != issuser)
                            {
                                //返回内容添加头Token-Error-Iss
                                context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                            }
                            //audience字段不对
                            if (jwtToken.Audiences.FirstOrDefault() != audience)
                            {
                                //返回内容添加头Token-Error-Aud
                                context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                            }
                        }

                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }

        public static bool IsNotEmptyOrNull(this object thisValue)
        {
            return ObjToString(thisValue) != "" && ObjToString(thisValue) != "undefined" && ObjToString(thisValue) != "null";
        }
    }
}
