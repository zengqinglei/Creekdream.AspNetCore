using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Creekdream.Dependency;
using Creekdream.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Creekdream.SimpleDemo.Interceptors
{
    /// <summary>
    /// 服务层审计日志
    /// </summary>
    public class AuditLogInterceptor : InterceptorBase, ITransientDependency
    {
        private readonly ILogger<AuditLogInterceptor> _logger;

        /// <inheritdoc />
        public AuditLogInterceptor(ILogger<AuditLogInterceptor> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public override void Intercept(IInvocation invocation)
        {
            MethodInfo method;
            try
            {
                method = invocation.MethodInvocationTarget;
            }
            catch
            {
                method = invocation.GetConcreteMethod();
            }
            Exception error = null;
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                error = ex;
                throw;
            }
            finally
            {
                var auditLogAttribute = GetAuditLogAttribute(method);
                if (!auditLogAttribute.IsDisabled)
                {
                    _logger.LogInformation($"ClassName:{method.DeclaringType},MethodName:{method.Name},args:{invocation.Arguments.ToString()},return value:{invocation.ReturnValue},error:{error?.Message}");
                }
            }
        }

        private AuditLogAttribute GetAuditLogAttribute(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<AuditLogAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<AuditLogAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }
            return null;
        }
    }
}

