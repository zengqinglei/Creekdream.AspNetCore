using Creekdream.Dependency;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Xunit;

namespace Creekdream.AspNetCore.Tests
{
    public class AspNetCoreTest
    {
        private readonly TestServer _server;

        public AspNetCoreTest()
        {
            var builder = new HostBuilder()
                .UseServiceProviderFactory(context =>
                {
                    return context.UseWindsor();
                })
                .Build()
                .Services;
            _server = new TestServer(builder);
        }

        [Fact]
        public void Test_CreateClient()
        {
            var client = _server.CreateClient();
            client.ShouldNotBeNull();
        }
    }
}
