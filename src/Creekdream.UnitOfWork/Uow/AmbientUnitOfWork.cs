using Creekdream.Dependency;
using System.Threading;

namespace Creekdream.Uow
{
    /// <inheritdoc />
    public class AmbientUnitOfWork : IAmbientUnitOfWork, ISingletonDependency
    {
        private readonly AsyncLocal<IUnitOfWork> _currentUow;

        /// <inheritdoc />
        public AmbientUnitOfWork()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }

        /// <inheritdoc />
        public IUnitOfWork Get()
        {
            return _currentUow.Value;
        }

        /// <inheritdoc />
        public void Set(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }
    }
}
