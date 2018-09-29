using System;

namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// Dapper uow specific extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class UnitOfWorkDapperServicesBuilderExtension
    {
        /// <summary>
        /// Use unit of work
        /// </summary>
        public static ServicesBuilderOptions UseUnitOfWork(this ServicesBuilderOptions builder, Action<UnitOfWorkCoreOptionsBuilder> options = null)
        {
            var uowOptionsBuilder = new UnitOfWorkCoreOptionsBuilder();
            options?.Invoke(uowOptionsBuilder);
            uowOptionsBuilder.Build(builder.IocRegister);

            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(UnitOfWorkDapperServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}

