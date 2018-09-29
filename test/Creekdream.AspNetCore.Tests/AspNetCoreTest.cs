using Creekdream.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Creekdream.AspNetCore.Tests
{
    public class AspNetCoreTest
    {
        private readonly TestServer _server;

        public AspNetCoreTest()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddCreekdream(
                        options =>
                        {
                            options.UseAutofac();
                        });
                }).Configure(app =>
                {
                    app.UseCreekdream(
                        options =>
                        {

                        });
                });
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
