using Blog.Api.Common.HttpContextUser;
using Blog.Api.Common.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Blog.Api.Extension.Authotizations.Policy
{
    public class CustomResponseHandle : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUser _user;
        public CustomResponseHandle(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,IUser user) : base(options, logger, encoder, clock)
        {
            _user = user;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写JWT认证返回401时的返回json
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            await Response.WriteAsync(JsonHelper.Serialize(new { Code = "401", Msg = "您没有权限访问，请确保已经登录!" }));
        }

        /// <summary>
        /// 重写JWT认证时返回403的处理
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            await Response.WriteAsync(JsonHelper.Serialize(new { Code = "403", Msg = "禁止访问，您的权限不够！" }));
        }
    }
}
