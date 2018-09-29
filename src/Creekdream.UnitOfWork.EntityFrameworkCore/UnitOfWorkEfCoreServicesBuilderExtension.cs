using System;

namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// EfCore uow specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class UnitOfWorkEfCoreServicesBuilderExtension
    {
        /// <summary>
        /// Use unit of work，note that only relational databases are supported
        /// </summary>
        public static ServicesBuilderOptions UseUnitOfWork(this ServicesBuilderOptions builder, Action<UnitOfWorkCoreOptionsBuilder> options = null)
        {
            var uowOptionsBuilder = new UnitOfWorkCoreOptionsBuilder();
            options?.Invoke(uowOptionsBuilder);
            uowOptionsBuilder.Build(builder.IocRegister);

            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(UnitOfWorkEfCoreServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}

