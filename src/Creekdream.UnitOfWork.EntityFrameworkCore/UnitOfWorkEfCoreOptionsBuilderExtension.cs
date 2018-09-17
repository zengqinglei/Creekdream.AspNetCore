using System;

namespace Creekdream.UnitOfWork
{
    /// <summary>
    /// EfCore uow specific extension methods for <see cref="AppOptionsBuilder" />.
    /// </summary>
    public static class UnitOfWorkEfCoreOptionsBuilderExtension
    {
        /// <summary>
        /// Use unit of work
        /// </summary>
        public static AppOptionsBuilder UseUnitOfWork(this AppOptionsBuilder builder, Action<UnitOfWorkCoreOptionsBuilder> options = null)
        {
            var uowOptionsBuilder = new UnitOfWorkCoreOptionsBuilder();
            options?.Invoke(uowOptionsBuilder);
            uowOptionsBuilder.Build(builder.IocRegister);

            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(UnitOfWorkEfCoreOptionsBuilderExtension).Assembly);
            return builder;
        }
    }
}

