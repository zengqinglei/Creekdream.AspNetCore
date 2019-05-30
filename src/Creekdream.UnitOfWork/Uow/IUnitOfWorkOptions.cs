using System;
using System.Data;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work options
    /// </summary>
    public interface IUnitOfWorkOptions
    {
        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        bool IsTransactional { get; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        IsolationLevel? IsolationLevel { get; }

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        TimeSpan? Timeout { get; }
    }
}
