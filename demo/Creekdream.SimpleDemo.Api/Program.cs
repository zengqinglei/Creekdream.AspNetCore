using Creekdream.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Creekdream.SimpleDemo.Api
{
    /// <inheritdoc />
    public class Program
    {
        /// <inheritdoc />
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <inheritdoc />
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseServiceProviderFactory(context => { 
                    return context.UseAutofac();
                });
    }
}
