using Blog.Api.Common.Json;
using Blog.Api.IServices;
using Blog.Api.Models.TempModels;
using Blog.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Blog.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public LoginController(IUserServices userServices)
        {
            _userServices= userServices;
        }

        [HttpPost]
        public ActionResult Login(string username,string pwd)
        {
            return null;
        }

        [HttpGet]
        public ActionResult<ResultMsg<User>> GetUser()
        {
            var user= _userServices.QueryWhere(u => u.Username == "admin").FirstOrDefault();
            return Ok(new ResultMsg<User>() { Code=1,Msg=user });
        }
    }
}
