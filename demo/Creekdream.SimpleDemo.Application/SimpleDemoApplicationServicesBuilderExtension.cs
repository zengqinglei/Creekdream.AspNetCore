namespace Creekdream.SimpleDemo
{
    /// <summary>
    /// SimpleDemo application module extension methods for <see cref="ServicesBuilderOptions" />.
    /// </summary>
    public static class SimpleDemoApplicationServicesBuilderExtension
    {
        /// <summary>
        /// Add an SimpleDemoApplication module
        /// </summary>
        public static ServicesBuilderOptions AddSimpleDemoApplication(this ServicesBuilderOptions builder)
        {
            builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(SimpleDemoApplicationServicesBuilderExtension).Assembly);
            return builder;
        }
    }
}


