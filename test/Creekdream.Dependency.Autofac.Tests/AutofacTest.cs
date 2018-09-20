namespace Creekdream.Dependency.Autofac.Tests
{
    public class AutofacTest : TestBase.TestBase
    {
        protected override IocRegisterBase GetIocRegister()
        {
            var iocRegister = new AutofacIocRegister();
            iocRegister.Register<IIocResolver, AutofacIocResolver>(DependencyLifeStyle.Transient);
            return iocRegister;
        }
    }
}
