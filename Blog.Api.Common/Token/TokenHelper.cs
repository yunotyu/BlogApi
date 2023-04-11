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

        public  string GetToken(Claim[] claims)
        {
            Issuer = AppConfigs.ReadAppConfig(new string[] { "JWT", "Issuer" });
            Audience = AppConfigs.ReadAppConfig(new string[] { "JWT", "Audience" });
            Expiration = DateTime.Now.AddSeconds(30);
            var secKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AppConfigs.Base64Key));
            SigningCredentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDes = new JwtSecurityToken(claims: claims, expires: Expiration, signingCredentials: SigningCredentials);
            string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDes);
            return jwt;
        }
    }
}
