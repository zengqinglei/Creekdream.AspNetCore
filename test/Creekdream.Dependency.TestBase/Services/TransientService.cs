using System;

namespace Creekdream.Dependency.TestBase.Services
{
    public interface ITransientService : ITransientDependency
    {
        Guid Id { get; set; }
        string Name { get; set; }

        string GetName();
    }

    public class TransientService : BaseService, ITransientService
    {
        public static string Interceptor = nameof(TransientService);

        public Guid Id { get; set; }
        public string Name { get; set; }

        public TransientService()
        {
            Id = Guid.NewGuid();
            Name = nameof(TransientService);
        }

        public string GetName()
        {
            return Name;
        }
    }
}
