using Blog.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Api.Filter
{
    public class ModelValidateActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //模型验证无效
            if(!context.ModelState.IsValid)
            {
               //获取验证失败的model字段错误消息
                var errPropertys = context.ModelState.Where(e => e.Value.Errors.Count > 0)
                                 .Select(e => e.Value.Errors.First().ErrorMessage).ToList();
                
                //所有错误内容，| 分割
                string err=string.Join("| ", errPropertys);

                var res = new ResultMsg<string>()
                {
                    Code = 0,
                    Data = err
                };
                context.Result=new BadRequestObjectResult(res);
                await Task.CompletedTask;
                return;
            }
            await next();
        }
    }
}
