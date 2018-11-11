using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Creekdream.SimpleDemo.Exceptions
{
    /// <summary>
    /// 友好的异常信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UserFriendlyException : Exception
    {
        /// <summary>
        /// 异常Id
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// 辅助状态码
        /// </summary>
        [JsonProperty]
        public ErrorCode Code { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        [JsonProperty]
        public override string Message => base.Message;

        /// <summary>
        /// 模型验证错误信息
        /// </summary>
        [JsonProperty]
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonProperty]
        public override IDictionary Data => base.Data;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">错误码</param>
        /// <param name="message">消息</param>
        /// <param name="errors">模型验证错误集合</param>
        public UserFriendlyException(ErrorCode code, string message = null, Dictionary<string, IEnumerable<string>> errors = null)
            : base(message ?? code.ToString())
        {
            Id = Guid.NewGuid().ToString();
            Code = code;
            Errors = errors ?? new Dictionary<string, IEnumerable<string>>();
        }
    }
}
