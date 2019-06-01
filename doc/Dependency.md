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
public class Startup
{
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        return services.AddCreekdream(
            options =>
            {
                options.UseAutofac();
            });
    }
}
```

## 使用 Castle.Windsor 作为IoC以及DI组件

### 1. 安装组件
``` csharp
Install-Package Creekdream.Dependency.Windsor
```
### 2. 配置启用组件
``` csharp
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