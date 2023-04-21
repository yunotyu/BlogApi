using Blog.Api.Common;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController: ControllerBase
    {
        public ResultMsg<T> Success<T>(T data,string msg="ok")
        {
            var res = new ResultMsg<T>()
            {
                Code = 1,
                Data = data,
                Msg = msg
            };
            return res;
        }

        public ResultMsg<T> Fail<T>(T data,string msg="fail")
        {
            var res = new ResultMsg<T>() { Code = 0, Data = data , Msg = msg };
             return res;
        }
    }
}
