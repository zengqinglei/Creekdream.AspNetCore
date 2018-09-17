using Creekdream.Dependency;

namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// Current uow provider
    /// </summary>
    public interface ICurrentUnitOfWorkProvider: ISingletonDependency
    {
        /// <summary>
        /// Gets currently active unit of work (or null if not exists).
        /// </summary>
        IUnitOfWork Current { get; set; }
    }
}

