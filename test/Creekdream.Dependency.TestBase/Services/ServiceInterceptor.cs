using Castle.DynamicProxy;
using Creekdream.DynamicProxy;

namespace Creekdream.Dependency.TestBase.Services
{
    public class ServiceInterceptor : InterceptorBase
    {
        public override void Intercept(IInvocation invocation)
        {
            if (invocation.TargetType == typeof(SingletonService))
            {
                SingletonService.Interceptor = "Singleton";
            }
            if (invocation.TargetType == typeof(ScopedService))
            {
                ScopedService.Interceptor = "Scoped";
            }
            if (invocation.TargetType == typeof(TransientService))
            {
                TransientService.Interceptor = "Transient";
            }
        }
    }
}
