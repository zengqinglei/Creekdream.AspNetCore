using Creekdream.SimpleDemo.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Creekdream.SimpleDemo.Api.Filters
{
    /// <summary>
    /// 自定义异常过滤器
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (!(exception is UserFriendlyException friendlyException))
            {
                friendlyException = new UserFriendlyException(
                    ErrorCode.InternalServerError,
                    exception.Message);
            }
            var httpStatusCode = int.Parse(((int)friendlyException.Code).ToString().Substring(0, 3));
            if (httpStatusCode == 422)
            {
                httpStatusCode = 400;
            }

            var logStr = new StringBuilder();
            logStr.Append($"LogId: {friendlyException.Id}, Message: {friendlyException.Message}");
            logStr.AppendLine();
            logStr.Append($"Exception: {exception.ToString()}");
            _logger.LogError(logStr.ToString());

            context.HttpContext.Response.StatusCode = httpStatusCode;
            context.Result = new JsonResult(friendlyException);
        }
    }
}


