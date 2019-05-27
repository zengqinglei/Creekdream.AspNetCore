using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Creekdream.DynamicProxy
{
    /// <inheritdoc />
    public interface IMethodInvocation
    {
        /// <inheritdoc />
        object[] Arguments { get; }

        /// <inheritdoc />
        IReadOnlyDictionary<string, object> ArgumentsDictionary { get; }

        /// <inheritdoc />
        Type[] GenericArguments { get; }

        /// <inheritdoc />
        object TargetObject { get; }

        /// <inheritdoc />
        MethodInfo Method { get; }

        /// <inheritdoc />
        object ReturnValue { get; set; }

        /// <inheritdoc />
        void Proceed();

        /// <inheritdoc />
        Task ProceedAsync();
    }
}
