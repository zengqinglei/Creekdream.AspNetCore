namespace Creekdream.Uow
{
    /// <summary>
    /// Used to get/set current <see cref="IUnitOfWork"/>. 
    /// </summary>
    public interface IAmbientUnitOfWork
    {
        /// <summary>
        /// Gets current <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork Get();

        /// <summary>
        /// sets current <see cref="IUnitOfWork"/>.
        /// Setting to null returns back to outer unit of work where possible.
        /// </summary>
        void Set(IUnitOfWork value);
    }
}
