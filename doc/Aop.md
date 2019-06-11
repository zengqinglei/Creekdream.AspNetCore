# Creekdream.AspNetCore 框架之 切面编程(拦截器)

首先我们想简单的了解下[AOP编程](https://baike.baidu.com/item/AOP/1332219?fr=aladdin)的概念，在我们的项目框架中主要实现接口或者类中虚拟方法的拦截器。  
当我们开发的拦截器应用到接口或者类上虚拟方法时，用户调用方法前，会自动调用拦截器方法，我们可以很方便的在拦截器中完成我们公共的一些操作，如：加入接口调用审计日志记录、工作单元管理、授权拦截等等功能。

## 1. 创建拦截器

``` csharp
/// <summary>
/// Unit of work interceptor
/// </summary>
public class UnitOfWorkInterceptor : InterceptorBase, ITransientDependency
{
    private readonly UnitOfWorkOptions _unitOfWorkOptions;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    /// <inheritdoc />
    public UnitOfWorkInterceptor(
        UnitOfWorkOptions unitOfWorkOptions,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkOptions = unitOfWorkOptions;
        _unitOfWorkManager = unitOfWorkManager;
    }

    /// <inheritdoc />
    public override void Intercept(IInvocation invocation)
    {
        // before do some things
        invocation.Proceed();
        // Behind do some things
    }
}
```

## 2. 注册拦截器

*拦截器注册的类型必须为接口或者类中存在的虚方法。*

``` csharp
/// <summary>
/// Use unit of work
/// </summary>
public static ServicesBuilderOptions UseUnitOfWork(this ServicesBuilderOptions options, Action<UnitOfWorkOptions> uowOptions = null)
{
    options.Services.OnRegistred(context =>
    {
        if (context.ImplementationType.IsDefined(typeof(UnitOfWorkAttribute), true))
        {
            context.Interceptors.Add<UnitOfWorkInterceptor>();
            return;
        }
    });
    // 扫描注入拦截器
    options.Services.RegisterAssemblyByBasicInterface(typeof(UowServicesBuilderExtension).Assembly);
    return options;
}
```