using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Autofac implementation of the ASP.NET Core <see cref="IServiceProvider"/>.
    /// </summary>
    public class AutofacServiceProvider : IServiceProvider, ISupportRequiredService
    {
        private readonly IComponentContext _componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceProvider"/> class.
        /// </summary>
        /// <param name="componentContext">
        /// The component context from which services should be resolved.
        /// </param>
        public AutofacServiceProvider(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        /// <summary>
        /// Gets service of type <paramref name="serviceType" /> from the
        /// <see cref="AutofacServiceProvider" /> and requires it be present.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetRequiredService(Type serviceType)
        {
            return this._componentContext.Resolve(serviceType);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">
        /// An object that specifies the type of service object to get.
        /// </param>
        /// <returns>
        /// A service object of type <paramref name="serviceType" />; or <see langword="null" />
        /// if there is no service object of type <paramref name="serviceType" />.
        /// </returns>
        public object GetService(Type serviceType)
        {
            return this._componentContext.ResolveOptional(serviceType);
        }
    }
}
