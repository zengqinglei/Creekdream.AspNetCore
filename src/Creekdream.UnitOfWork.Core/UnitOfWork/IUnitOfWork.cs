namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// Unit of work interface
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// The only unit of work Id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Outer unit of work
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        /// Open unit of work
        /// </summary>
        void Begin(UnitOfWorkOptions options);

        /// <summary>
        /// Is disposed
        /// </summary>
        bool IsDisposed { get; }
    }
}

