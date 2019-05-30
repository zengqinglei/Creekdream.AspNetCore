namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work management service interface
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Current unit of work
        /// </summary>
        IUnitOfWork Current { get; }
        /// <summary>
        /// Open a unit of work
        /// </summary>
        IUnitOfWork Begin(UnitOfWorkOptions options = null, bool requiresNew = true);
    }
}

