using Castle.MicroKernel.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Creekdream.Dependency.Windsor
{
    /// <summary>
    /// Windsor ComponentRegistration extensions
    /// </summary>
    public static class ComponentRegistrationExtensions
    {
        /// <summary>
        /// Lifestyle conversion and application
        /// </summary>
        public static ComponentRegistration<T> ConfigureLifecycle<T>(
            this ComponentRegistration<T> registration,
            ServiceLifetime lifecycleKind)
            where T : class
        {
            switch (lifecycleKind)
            {
                case ServiceLifetime.Singleton:
                    registration.LifestyleSingleton();
                    break;
                case ServiceLifetime.Scoped:
                    registration.LifestyleScoped();
                    break;
                case ServiceLifetime.Transient:
                    registration.LifestyleTransient();
                    break;
            }
            return registration;
        }
    }
}
