using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;
using System.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.Api.Common.Token
{
    public class TokenHelper
    {
        public string ClaimType { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime Expiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public  string GetToken(List<Claim> claims)
        {
            var expires = Convert.ToDouble(AppConfigs.ReadAppConfig(new string[] { "JWT", "Expires" }));
            Expiration =DateTime.Now.AddSeconds(expires);
            var tokenDes = new JwtSecurityToken(issuer:Issuer,audience:Audience,claims: claims, expires: Expiration, signingCredentials: SigningCredentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDes);
            return jwt;
        }
    }
}
