using System;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work event args
    /// </summary>
    public class UnitOfWorkEventArgs : EventArgs
    {
        /// <summary>
        /// Reference to the unit of work related to this event.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc />
        public UnitOfWorkEventArgs(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
