using Castle.DynamicProxy;
using Creekdream.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Creekdream.DynamicProxy
{
    /// <inheritdoc />
    public class CastleMethodInvocationAdapter : IMethodInvocation
    {
        /// <inheritdoc />
        public object[] Arguments => Invocation.Arguments;

        /// <inheritdoc />
        public IReadOnlyDictionary<string, object> ArgumentsDictionary => _lazyArgumentsDictionary.Value;
        private readonly Lazy<IReadOnlyDictionary<string, object>> _lazyArgumentsDictionary;

        /// <inheritdoc />
        public Type[] GenericArguments => Invocation.GenericArguments;

        /// <inheritdoc />
        public object TargetObject => Invocation.InvocationTarget ?? Invocation.MethodInvocationTarget;

        /// <inheritdoc />
        public MethodInfo Method => Invocation.MethodInvocationTarget ?? Invocation.Method;

        /// <inheritdoc />
        public object ReturnValue
        {
            get => _actualReturnValue ?? Invocation.ReturnValue;
            set => Invocation.ReturnValue = value;
        }

        private object _actualReturnValue;

        /// <inheritdoc />
        protected IInvocation Invocation { get; }
        /// <inheritdoc />
        protected IInvocationProceedInfo ProceedInfo { get; }

        /// <inheritdoc />
        public CastleMethodInvocationAdapter(IInvocation invocation, IInvocationProceedInfo proceedInfo)
        {
            Invocation = invocation;
            ProceedInfo = proceedInfo;

            _lazyArgumentsDictionary = new Lazy<IReadOnlyDictionary<string, object>>(GetArgumentsDictionary);
        }

        /// <inheritdoc />
        public void Proceed()
        {
            ProceedInfo.Invoke();

            if (Invocation.Method.IsAsync())
            {
                AsyncHelper.RunSync(() => (Task)Invocation.ReturnValue);
            }
        }

        /// <inheritdoc />
        public Task ProceedAsync()
        {
            ProceedInfo.Invoke();

            _actualReturnValue = Invocation.ReturnValue;

            return Invocation.Method.IsAsync()
                ? (Task)_actualReturnValue
                : Task.FromResult(_actualReturnValue);
        }

        private IReadOnlyDictionary<string, object> GetArgumentsDictionary()
        {
            var dict = new Dictionary<string, object>();

            var methodParameters = Method.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                dict[methodParameters[i].Name] = Invocation.Arguments[i];
            }

            return dict;
        }
    }
}
