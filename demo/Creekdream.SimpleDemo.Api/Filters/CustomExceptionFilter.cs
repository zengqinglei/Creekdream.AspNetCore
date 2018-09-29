using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Creekdream.SimpleDemo.Api.Filters
{
    /// <summary>
    ///     自定义异常过滤器
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        ///     异常处理
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            context.HttpContext.Response.StatusCode = 500;
            context.Result = new JsonResult(exception);
        }
    }
}


