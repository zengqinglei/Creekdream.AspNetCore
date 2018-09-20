using System;

namespace Creekdream.Dependency.TestBase.Services
{
    public interface IScopedService : IScopedDependency
    {
        Guid Id { get; set; }
        string Name { get; set; }

        string GetName();
    }

    public class ScopedService : BaseService, IScopedService
    {
        public static string Interceptor = nameof(ScopedService);

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ScopedService()
        {
            Id = Guid.NewGuid();
            Name = nameof(ScopedService);
        }

        public string GetName()
        {
            return Name;
        }
    }
}
