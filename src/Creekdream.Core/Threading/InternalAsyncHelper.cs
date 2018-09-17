using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Creekdream.Threading
{
    /// <summary>
    /// Asynchronous method call helper class
    /// </summary>
    public static class InternalAsyncHelper
    {
        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="finalAction">failure callback</param>
        public static async Task AwaitTaskWithFinally(Task actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="postAction">success callback</param>
        /// <param name="finalAction">failure callback</param>
        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                await actualReturnValue;
                await postAction();
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="preAction">Pre-call method</param>
        /// <param name="postAction">success callback</param>
        /// <param name="finalAction">failure callback</param>
        public static async Task AwaitTaskWithPreActionAndPostActionAndFinally(Func<Task> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return generic object</returns>
        public static async Task<T> AwaitTaskWithFinallyAndGetResult<T>(Task<T> actualReturnValue, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                return await actualReturnValue;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="taskReturnType">Asynchronous method</param>
        /// <param name="actualReturnValue">Asynchronous method required parameters</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return object</returns>
        public static object CallAwaitTaskWithFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, finalAction });
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="postAction">success callback</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return generic object</returns>
        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
        {
            Exception exception = null;

            try
            {
                var result = await actualReturnValue;
                await postAction();
                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="taskReturnType">Asynchronous method</param>
        /// <param name="actualReturnValue">Asynchronous method required parameters</param>
        /// <param name="action">success callback</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return object</returns>
        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="actualReturnValue">Asynchronous method</param>
        /// <param name="preAction">Pre-call method</param>
        /// <param name="postAction">success callback</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return generic object</returns>
        public static async Task<T> AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult<T>(Func<Task<T>> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            Exception exception = null;

            try
            {
                if (preAction != null)
                {
                    await preAction();
                }

                var result = await actualReturnValue();

                if (postAction != null)
                {
                    await postAction();
                }

                return result;
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                finalAction?.Invoke(exception);
            }
        }

        /// <summary>
        /// Asynchronous method call
        /// </summary>
        /// <param name="taskReturnType">Asynchronous method</param>
        /// <param name="actualReturnValue">Asynchronous method required parameters</param>
        /// <param name="preAction">Pre-call method</param>
        /// <param name="postAction">success callback</param>
        /// <param name="finalAction">failure callback</param>
        /// <returns>Return object</returns>
        public static object CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(Type taskReturnType, Func<object> actualReturnValue, Func<Task> preAction = null, Func<Task> postAction = null, Action<Exception> finalAction = null)
        {
            return typeof(InternalAsyncHelper)
                .GetMethod("AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
                .MakeGenericMethod(taskReturnType)
                .Invoke(null, new object[] { actualReturnValue, preAction, postAction, finalAction });
        }
    }
}

