using Creekdream.Application.Service;
using Creekdream.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace Creekdream.Uow
{
    /// <inheritdoc />
    public class UnitOfWorkOptions : IUnitOfWorkOptions
    {
        /// <inheritdoc />
        public bool IsTransactional { get; set; }

        /// <inheritdoc />
        public IsolationLevel? IsolationLevel { get; set; }

        /// <inheritdoc />
        public TimeSpan? Timeout { get; set; }

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

        /// <inheritdoc />
        public UnitOfWorkOptions Clone()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel,
                Timeout = Timeout
            };
        }
    }
}