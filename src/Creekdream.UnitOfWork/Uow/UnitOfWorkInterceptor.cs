using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Creekdream.Dependency;
using Creekdream.DynamicProxy;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work interceptor
    /// </summary>
    public class UnitOfWorkInterceptor : InterceptorBase, ITransientDependency
    {
        private readonly UnitOfWorkOptions _unitOfWorkOptions;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <inheritdoc />
        public UnitOfWorkInterceptor(
            UnitOfWorkOptions unitOfWorkOptions,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkOptions = unitOfWorkOptions;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <inheritdoc />
        public override void Intercept(IMethodInvocation invocation)
        {
            var unitOfWorkOptions = GetUnitOfWorkAttribute(invocation.Method);
            if (unitOfWorkOptions == null)
            {
                //No need to a uow
                invocation.Proceed();
                return;
            }

            //No current uow, run a new one
            using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        /// <inheritdoc />
        public override async Task InterceptAsync(IMethodInvocation invocation)
        {
            var unitOfWorkOptions = GetUnitOfWorkAttribute(invocation.Method);
            if (unitOfWorkOptions == null)
            {
                //No need to a uow
                await invocation.ProceedAsync();
                return;
            }

            //No current uow, run a new one
            using (var uow = _unitOfWorkManager.Begin(unitOfWorkOptions))
            {
                await invocation.ProceedAsync();
                uow.Complete();
            }
        }

        private UnitOfWorkOptions GetUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0].CreateOptionsFromDefault(_unitOfWorkOptions);
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0].CreateOptionsFromDefault(_unitOfWorkOptions);
            }

            if (_unitOfWorkOptions.ConventionalUowSelectors.Any(selector => selector(methodInfo.DeclaringType)))
            {
                return _unitOfWorkOptions;
            }

            return null;
        }
    }
}

