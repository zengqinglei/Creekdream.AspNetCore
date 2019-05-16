using Castle.MicroKernel.Registration;
using System;

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
        public static ComponentRegistration<T> ApplyLifestyle<T>(
            this ComponentRegistration<T> registration,
            DependencyLifeStyle lifeStyle)
            where T : class
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    return registration.LifestyleTransient();
                case DependencyLifeStyle.Scoped:
                    return registration.LifestyleScoped<LocalLifetimeScopeAccessor>();
                case DependencyLifeStyle.Singleton:
                    return registration.LifestyleSingleton();
                default:
                    throw new ArgumentException(nameof(lifeStyle));
            }
        }
    }
}
