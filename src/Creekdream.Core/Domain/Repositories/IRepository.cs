using Creekdream.Dependency;
using System;

namespace Creekdream.Domain.Repositories
{
    /// <summary>
    /// This interface must be implemented by all repositories to identify them by convention.
    /// Implement generic version instead of this one.
    /// </summary>
    public interface IRepository : ITransientDependency
    {
        /// <summary>
        /// get service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }
}