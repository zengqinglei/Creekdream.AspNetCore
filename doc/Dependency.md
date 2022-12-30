# Creekdream.AspNetCore 框架之 IoC 和 DI

* Ioc(控制反转)：由容器控制程序之间的（依赖）关系，而非传统实现中，由程序代码直接操控。
* DI(依赖注入)：简单来讲即由容器动态的将某种依赖关系注入到组件之中。

框架抽象了控制反转以及依赖注入的接口，使得IoC容器也可以被替换，框架中目前提供了两种比较主流的组件：Autofac、Castle.Windsor。

## 使用 Autofac 作为IoC以及DI组件(`推荐`)

### 1. 安装组件
``` csharp
Install-Package Creekdream.Dependency.Autofac
```
### 2. 配置启用组件
``` csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .UseServiceProviderFactory(context => context.UseAutofac());
```

## 使用 Castle.Windsor 作为IoC以及DI组件

### 1. 安装组件
``` csharp
Install-Package Creekdream.Dependency.Windsor
```
### 2. 配置启用组件
``` csharp
// Windsor目前不支持.net core 3.0+ 版本，请改用Autofac
public class Startup
{
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.AddCreekdream(
            options =>
            {
                options.UseWindsor();
            });
    }
}
```

## 使用示例

``` csharp
public class BookService : ApplicationService, IBookService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    /// <inheritdoc />
    public BookService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    /// <inheritdoc />
    public async Task<GetBookOutput> Get(Guid id)
    {
        var book = await _bookRepository.GetAsync(id);
        return book.MapTo<GetBookOutput>();
    }
}
```
# 常见问题

## 1. 接口与实现命名不规范导致服务注入异常
``` csharp
    在swagger下调试Api时提示以下异常：
    Code:500
    Error: Internal Server Error
    "message": "An exception was thrown while activating xxx.xxx.xxxxxxService."
    看到上面熟悉的代码抛异常信息，有一点可以知道服务没有注入成功。
    这个时候检查一下代码，Startup.cs，Program.cs,构造函数，好像都没有什么问题。

    “难道是框架的Ioc、DI做的有缺憾？”
    
    “请不要怀疑框架，也请不要有这样的想法。”
    
    Creekdream 框架对所有继承IsingletonDependency，ItransientDependency 服务接口在构造函数进行统一注入。
    约定: 接口类与实现类的命名必须一致，如接口IabcService，那么实现类必须是abcService，否则无法正常实例化注入。
    如 public class BoookService : ApplicationService, IBookService 必定必所示必定异常的
    
    所以当你的服务没法正常注入时，第一时间先看接口类与实现类的命名是否一致。
