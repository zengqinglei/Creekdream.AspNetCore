using System;

namespace Creekdream.Dependency.TestBase.Services
{
    public interface ISingletonService : ISingletonDependency
    {
        Guid Id { get; set; }
        string Name { get; set; }

        string GetName();
    }

    public class SingletonService : BaseService, ISingletonService
    {
        public static string Interceptor = nameof(SingletonService);

        public Guid Id { get; set; }
        public string Name { get; set; }

        public SingletonService()
        {
            Id = Guid.NewGuid();
            Name = nameof(SingletonService);
        }

        public string GetName()
        {
            return Name;
        }
    }
}
