using Microsoft.AspNetCore.Builder;
using System;

namespace Creekdream.AspNetCore
{
    /// <summary>
    /// Creekdream application service usage
    /// </summary>
    public static class CreekdreamApplicationBuilderExtensions
    {
        /// <summary>
        /// Use application framework service
        /// </summary>
        public static void UseCreekdream(this IApplicationBuilder app, Action<AppBuilderOptions> options = null)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var builder = new AppBuilderOptions(app.ApplicationServices);
            options?.Invoke(builder);
        }
    }
}
