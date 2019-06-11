using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Creekdream.Dependency;
using Creekdream.DynamicProxy;
using Creekdream.Threading;

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

            var unitOfWorkOptions = GetUnitOfWorkAttribute(method);
            if (unitOfWorkOptions == null)
            {
                //No need to a uow
                invocation.Proceed();
                return;
            }

            //No current uow, run a new one
            PerformUow(invocation, unitOfWorkOptions);
        }

        private void PerformUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            if (invocation.Method.IsAsync())
            {
                PerformAsyncUow(invocation, options);
            }
            else
            {
                PerformSyncUow(invocation, options);
            }
        }

        private void PerformSyncUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            using (var uow = _unitOfWorkManager.Begin(options, requiresNew: false))
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        private void PerformAsyncUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            var uow = _unitOfWorkManager.Begin(options, requiresNew: false);

            try
            {
                invocation.Proceed();
            }
            catch
            {
                uow.Dispose();
                throw;
            }

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () =>
                    {
                        await uow.CompleteAsync();
                    },
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () =>
                    {
                        await uow.CompleteAsync();
                    },
                    exception => uow.Dispose()
                );
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

