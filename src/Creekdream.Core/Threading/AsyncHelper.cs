using Nito.AsyncEx;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Creekdream.Threading
{
    /// <summary>
    /// Asynchronous method call helper class
    /// </summary>
    public static class AsyncHelper
    {
        /// <summary>
        /// Determine if it is an asynchronous method
        /// </summary>
        public static bool IsAsync(this MethodInfo method)
        {
            return (
                method.ReturnType == typeof(Task) ||
                (method.ReturnType.GetTypeInfo().IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }

        /// <summary>
        /// Synchronously run asynchronous method
        /// </summary>
        /// <typeparam name="TResult">Generic object type</typeparam>
        /// <param name="func">asynchronous method</param>
        /// <returns>Return generic object</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        /// <summary>
        /// Synchronously run asynchronous method
        /// </summary>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}

