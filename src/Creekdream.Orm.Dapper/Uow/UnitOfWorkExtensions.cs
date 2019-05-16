using Creekdream.Uow;
using DapperExtensions;
using System;

namespace Creekdream.Orm.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="UnitOfWork"/>.
        /// </summary>
        public static IDatabase GetDatabase(this IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is UnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(UnitOfWork).FullName, "unitOfWork");
            }

            return (unitOfWork as UnitOfWork).GetOrCreateDatabase();
        }
    }
}
