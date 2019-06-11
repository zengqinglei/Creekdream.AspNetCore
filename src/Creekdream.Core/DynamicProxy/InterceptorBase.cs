using Castle.DynamicProxy;

namespace Creekdream.DynamicProxy
{
    /// <inheritdoc />
    public abstract class InterceptorBase : IInterceptor
    {
        /// <inheritdoc />
        public abstract void Intercept(IInvocation invocation);
    }
}
