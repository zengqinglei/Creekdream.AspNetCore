using System.Threading.Tasks;

namespace Creekdream.DynamicProxy
{
    /// <inheritdoc />
    public abstract class InterceptorBase : IInterceptor
    {
        /// <inheritdoc />
        public abstract void Intercept(IMethodInvocation invocation);

        /// <inheritdoc />
        public virtual Task InterceptAsync(IMethodInvocation invocation)
        {
            Intercept(invocation);
            return Task.CompletedTask;
        }
    }
}
