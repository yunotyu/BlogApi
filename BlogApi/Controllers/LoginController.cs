using Blog.Api.Common;
using Blog.Api.Common.Json;
using Blog.Api.Common.Token;
using Blog.Api.IServices;
using Blog.Api.Model;
using Blog.Api.Models.TempModels;
using Blog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : BaseApiController
    {
        private readonly IUserServices _userServices;
        private readonly ILogger<LoginController> _logger;
        private readonly TokenHelper _tokenHelper;

        public LoginController(IUserServices userServices, ILogger<LoginController> logger, TokenHelper tokenHelper)
        {
            _userServices = userServices;
            _logger = logger;
            _tokenHelper = tokenHelper;
        }

        [HttpPost]
        public ActionResult<ResultMsg<TokenModel>> Login(string username="admin",string pwd = "admin")
        {
            if(username=="admin"&&pwd=="admin") {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ""));
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim("phone", "123"));
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                string jwt= _tokenHelper.GetToken(claims);
                return Success<TokenModel>(new TokenModel()
                {
                    Success = true,
                    Token = jwt,
                    Expires = 30,
                }) ;
               
            }
             return Fail<TokenModel>(new TokenModel()
            {
                Success = false,
                Token = "",
                Expires = _tokenHelper.Expiration.Second,
            });
        }

        [HttpGet]
        [Authorize(Roles="admin")]
        public ActionResult<ResultMsg<User>> GetUser()
        {
            var user= _userServices.QueryWhere(u => u.Username == "admin").FirstOrDefault();
            return Success<User>(user);
        }
    }
}
