# Creekdream.AspNetCore 框架之模块化

本框架参考过 [ABP](https://github.com/aspnetboilerplate/aspnetboilerplate)、[OrchardCore](https://github.com/OrchardCMS/OrchardCore)、[Util](https://github.com/twitter/util)等框架的模块化设计。
* **ABP**：通过模块依赖关系执行进行组合并排序后按顺序先后执行初始化函数。
* **Util**：通过扫描程序集并在应用程序启动时加载含有集成模块基类的程序集并执行初始化函数

本项目并不推荐这两种实现方案，主要结合.NET CORE 新的设计模式，应尽量将模块的初始化工作放到Startup中，并能明确的列出所使用的模块，应尽量不依赖各自的执行顺序关系，避免产生侵入或耦合。

## 项目采用扩展ServicesBuilderOptions方法进行模块化初始化
新建一个模块,在模块中可通过AppOptionsBuilder中的注入组件初始化该模块的相关内容
``` csharp
/// <summary>
/// ProjectName application module extension methods for <see cref="AppOptionsBuilder" />.
/// </summary>
public static class ProjectNameApplicationOptionsBuilderExtension
{
    /// <summary>
    /// Add a ProjectName application module
    /// </summary>
    public static ServicesBuilderOptions AddProjectNameApplication(this ServicesBuilderOptions builder)
    {
        builder.IocRegister.RegisterAssemblyByBasicInterface(typeof(ProjectNameApplicationOptionsBuilderExtension).Assembly);
        return builder;
    }
}
```

初始化模块，在Startup启动类中启用模块
``` csharp
/// <inheritdoc />
public class Startup
{
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    /// </summary>
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.AddCreekdream(
            options =>
            {
                options.AddProjectNameApplication();
            });
    }
}
```