using Autofac.Builder;
using System;

namespace Creekdream.Dependency.Autofac
{
    /// <summary>
    /// Autofac RegistrationBuilder extensions
    /// </summary>
    public static class RegistrationBuilderExtensions
    {
        /// <summary>
        /// Lifestyle conversion and application
        /// </summary>
        public static void AddLifeStyle<TLimit, TActiatorData, TRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActiatorData, TRegistrationStyle> registration,
            DependencyLifeStyle lifeStyle)
        {
            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    registration.InstancePerDependency();
                    break;
                case DependencyLifeStyle.Scoped:
                    registration.InstancePerLifetimeScope();
                    break;
                case DependencyLifeStyle.Singleton:
                    registration.SingleInstance();
                    break;
                default:
                    throw new ArgumentException(nameof(lifeStyle));
            }
        }
    }
}
