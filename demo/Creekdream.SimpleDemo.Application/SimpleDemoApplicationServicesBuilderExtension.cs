using Creekdream.Dependency;
using Creekdream.SimpleDemo.Interceptors;
using System.Linq;
using System.Reflection;

namespace Creekdream.SimpleDemo
{
    /// <summary>
    /// SimpleDemo application module extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class SimpleDemoApplicationServicesBuilderExtension
    {
        /// <summary>
        /// Add an SimpleDemoApplication module
        /// </summary>
        public static ServicesBuilderOptions AddSimpleDemoApplication(this ServicesBuilderOptions builder)
        {
            builder.Services.OnRegistred(context =>
            {
                if (context.ImplementationType.IsDefined(typeof(AuditLogAttribute), true))
                {
                    context.Interceptors.Add<AuditLogInterceptor>();
                    return;
                }
                var methods = context.ImplementationType.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);
                if (methods.Any(m => m.IsDefined(typeof(AuditLogAttribute), true)))
                {
                    context.Interceptors.Add<AuditLogInterceptor>();
                    return;
                }
            });
            builder.Services.RegisterAssemblyByBasicInterface(typeof(SimpleDemoApplicationServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}


