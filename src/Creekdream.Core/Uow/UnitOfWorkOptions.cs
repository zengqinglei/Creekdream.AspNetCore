using Creekdream.Application.Service;
using Creekdream.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Creekdream.Uow
{
    /// <summary>
    /// Unit of work options
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// Transaction scope
        /// </summary>
        public TransactionScopeOption Scope { get; set; } = TransactionScopeOption.Required;

        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public bool IsTransactional { get; set; } = false;

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadUncommitted;

        /// <summary>
        /// This option should be set to <see cref="TransactionScopeAsyncFlowOption.Enabled"/>
        /// if unit of work is used in an async scope.
        /// </summary>
        public TransactionScopeAsyncFlowOption AsyncFlowOption { get; set; } = TransactionScopeAsyncFlowOption.Enabled;

        /// <summary>
        /// Apply uow type
        /// </summary>
        public List<Func<Type, bool>> ConventionalUowSelectors { get; set; }

        /// <inheritdoc />
        public UnitOfWorkOptions()
        {
            ConventionalUowSelectors = new List<Func<Type, bool>>
            {
                type => typeof(IRepository).IsAssignableFrom(type),
                type => typeof(IApplicationService).IsAssignableFrom(type)
            };
        }
    }
}