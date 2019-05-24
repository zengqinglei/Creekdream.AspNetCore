using System;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Autofac implementation of the ASP.NET Core <see cref="IServiceScope"/>.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.DependencyInjection.IServiceScope" />
    internal class AutofacServiceScope : IServiceScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacServiceScope"/> class.
        /// </summary>
        /// <param name="lifetimeScope">
        /// The lifetime scope from which services should be resolved for this service scope.
        /// </param>
        public AutofacServiceScope(ILifetimeScope lifetimeScope)
        {
            this._lifetimeScope = lifetimeScope;
            this.ServiceProvider = this._lifetimeScope.Resolve<IServiceProvider>();
        }

        /// <summary>
        /// Gets an <see cref="IServiceProvider" /> corresponding to this service scope.
        /// </summary>
        /// <value>
        /// An <see cref="IServiceProvider" /> that can be used to resolve dependencies from the scope.
        /// </value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Disposes of the lifetime scope and resolved disposable services.
        /// </summary>
        public void Dispose()
        {
            this._lifetimeScope.Dispose();
        }
    }
}