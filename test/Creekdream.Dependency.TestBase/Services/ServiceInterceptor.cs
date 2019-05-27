using Castle.DynamicProxy;
using Creekdream.DynamicProxy;

namespace Creekdream.Dependency.TestBase.Services
{
    public class ServiceInterceptor : InterceptorBase
    {
        public override void Intercept(IMethodInvocation invocation)
        {
            if (invocation.TargetObject.GetType() == typeof(SingletonService))
            {
                SingletonService.Interceptor = "Singleton";
            }
            if (invocation.TargetObject.GetType() == typeof(ScopedService))
            {
                ScopedService.Interceptor = "Scoped";
            }
            if (invocation.TargetObject.GetType() == typeof(TransientService))
            {
                TransientService.Interceptor = "Transient";
            }
        }
    }
}
