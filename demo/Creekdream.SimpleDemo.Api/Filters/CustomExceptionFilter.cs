using Creekdream.SimpleDemo.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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
                    exception.Message,
                    innerException: exception);
            }
            _logger.LogError(friendlyException, "请求异常");

            var httpStatusCode = int.Parse(((int)friendlyException.Code).ToString().Substring(0, 3));
            context.Result = new ContentResult
            {
                ContentType = "application/json",
                Content = friendlyException.ToString(),
                StatusCode = httpStatusCode
            };
        }
    }
}
