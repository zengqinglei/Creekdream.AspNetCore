using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Creekdream.SimpleDemo.Exceptions
{
    /// <summary>
    /// 友好的异常信息
    /// </summary>
    public class UserFriendlyException : Exception
    {
        /// <summary>
        /// 异常Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 辅助状态码
        /// </summary>
        public ErrorCode Code { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public override string Message => base.Message;

        /// <summary>
        /// 模型验证错误信息
        /// </summary>
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public override IDictionary Data => base.Data;

        /// <inheritdoc />
        public UserFriendlyException(
            ErrorCode code,
            string message = null,
            Dictionary<string, IEnumerable<string>> errors = null,
            Exception innerException = null)
            : base(message ?? code.ToString(), innerException)
        {
            Id = Guid.NewGuid().ToString();
            Code = code;
            Errors = errors ?? new Dictionary<string, IEnumerable<string>>();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return JsonConvert.SerializeObject(new
            {
                Id,
                Code,
                Message,
                Errors,
                Data,
                Details = InnerException?.ToString()
            });
        }
    }
}
