using System;

namespace Creekdream.Dependency.TestBase.Services
{
    public class ServiceOptions : ISingletonDependency
    {
        public Guid Id { get; set; }

        public ServiceOptions()
        {
            Id = Guid.NewGuid();
        }
    }
}
