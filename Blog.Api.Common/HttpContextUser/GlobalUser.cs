using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Common.HttpContextUser
{
    public class GlobalUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<GlobalUser> _logger;

        public GlobalUser(IHttpContextAccessor accessor, ILogger<GlobalUser> logger)
        {
            _accessor = accessor;
            _logger = logger;
        }

        public string UserName => GetUserName();

        private string GetUserName()
        {
            //注意，这里要设置ClaimTypes.Name才会有值
            if (IsAuthenticated()&&!string.IsNullOrEmpty(_accessor.HttpContext.User.Identity.Name))
            {
                return _accessor.HttpContext.User.Identity.Name;
            }
            return "";
        }

        public int Id => 0;

        public long TenantId => 0;

        /// <summary>
        /// 用户是否被认证
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated() {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 获取jwt里面的角色
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoles()
        {
            var claims = _accessor.HttpContext.User.Claims.ToList();
            for (int i = 0; i < claims.Count; i++)
            {
                if (claims[i].Type == ClaimTypes.Role)
                {
                    var arr= claims[i].Value.Split(",");
                    return arr.ToList();
                }
            }
            return new List<string>();
        }

        /// <summary>
        /// 获取JWT里面Claim
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            var claims = _accessor.HttpContext.User.Claims.ToList();
            var headers = _accessor.HttpContext.Request.Headers;
            foreach (var header in headers)
            {
                claims.Add(new Claim(header.Key, header.Value));
            }

            return claims;
        }

        public List<string> GetClaimValueByType(string claimType)
        {
            return null;
        }

        /// <summary>
        /// 获取JWT
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            return _accessor.HttpContext?.Request?.Headers["Authorization"].ToString().Trim().Replace("Bearer ", "");
        }

        public List<string> GetUserInfoFormToken(string claimType)
        {
            return null;
        }
    }
}
