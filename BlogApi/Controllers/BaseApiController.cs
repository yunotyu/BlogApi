using Blog.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController: ControllerBase
    {
        public ResultMsg<T> Success<T>(T data)
        {
            var res = new ResultMsg<T>()
            {
                Code = 1,
                Data = data
            };
            return res;
        }

        public ResultMsg<T> Fail<T>(T data)
        {
            var res = new ResultMsg<T>() { Code = 0, Data = data };
             return res;
        }
    }
}
