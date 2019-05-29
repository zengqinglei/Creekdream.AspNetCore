using System;
using System.Data;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Is disabled transaction (default: false)
        /// </summary>
        public bool IsDisabled { get; set; }

        internal UnitOfWorkOptions CreateOptionsFromDefault(UnitOfWorkOptions defaultUnitOfWorkOptions)
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = !IsDisabled,
                IsolationLevel = IsolationLevel ?? defaultUnitOfWorkOptions.IsolationLevel,
                Timeout = Timeout ?? defaultUnitOfWorkOptions.Timeout
            };
        }
    }
}

