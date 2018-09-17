using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Creekdream.Threading;

namespace Creekdream.Tests.Threading
{
    /// <summary>
    /// 异步帮助类测试
    /// </summary>
    public class AsyncHelper_Tests
    {
        /// <summary>
        /// 异步方法同步执行测试
        /// </summary>
        [Fact]
        public void SyncRun_AsyncMethod()
        {
            AsyncHelper.RunSync(AsyncWaitSomeTime);
            AsyncHelper.RunSync(() => AsyncCalculateResult(21)).ShouldBe(42);
        }

        /// <summary>
        /// 异步等待
        /// </summary>
        private async Task AsyncWaitSomeTime()
        {
            await Task.Delay(10);
        }

        /// <summary>
        /// 异步计算结果
        /// </summary>
        private async Task<int> AsyncCalculateResult(int p)
        {
            await Task.Delay(10);
            return p * 2;
        }
    }
}

