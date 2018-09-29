namespace Creekdream.SimpleDemo
{
    /// <summary>
    /// SimpleDemo core module extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class SimpleDemoCoreServicesBuilderExtension
    {
        /// <summary>
        /// Add a SimpleDemo core module
        /// </summary>
        public static ServicesBuilderOptions AddSimpleDemoCore(this ServicesBuilderOptions builder)
        {
            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(SimpleDemoCoreServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}


