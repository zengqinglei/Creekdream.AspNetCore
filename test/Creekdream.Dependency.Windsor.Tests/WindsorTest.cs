namespace Creekdream.Dependency.Windsor.Tests
{
    public class WindsorTest : TestBase.TestBase
    {
        protected override IocRegisterBase GetIocRegister()
        {
            var iocRegister = new WindsorIocRegister();
            iocRegister.Register<IIocResolver, WindsorIocResolver>(DependencyLifeStyle.Transient);
            return iocRegister;
        }
    }
}
