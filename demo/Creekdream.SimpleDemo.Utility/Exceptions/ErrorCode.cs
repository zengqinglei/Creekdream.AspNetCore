namespace Creekdream.SimpleDemo.Exceptions
{
    public enum ErrorCode
    {
        #region 400
        /// <summary>
        /// 错误的请求
        /// </summary>
        BadRequest = 40000,
        #endregion

        #region 401
        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 40100,
        #endregion

        #region 403
        /// <summary>
        /// 禁止访问
        /// </summary>
        Forbidden = 40300,
        #endregion

        #region 404
        /// <summary>
        /// 资源不存在
        /// </summary>
        NotFound = 40400,
        #endregion

        #region 415
        /// <summary>
        /// 不支持的媒体类型
        /// </summary>
        UnsupportedMediaType = 41500,
        #endregion

        #region 422
        /// <summary>
        /// 模型验证错误
        /// </summary>
        UnprocessableEntity = 42200,
        #endregion

        #region 500
        /// <summary>
        /// 服务器异常
        /// </summary>
        InternalServerError = 50000,
        /// <summary>
        /// 短信服务异常
        /// </summary>
        Sms_Service_Exception = 50001,
        /// <summary>
        /// 业务处理异常
        /// </summary>
        Business_Exception = 50002,
        #endregion
    }
}