using System.Threading.Tasks;

namespace Creekdream.DynamicProxy
{
    /// <inheritdoc />
    public interface IInterceptor
    {
        /// <inheritdoc />
        void Intercept(IMethodInvocation invocation);

        /// <inheritdoc />
        Task InterceptAsync(IMethodInvocation invocation);
    }
}
