# Creekdream.AspNetCore 框架之 工作单元

项目开发中，我们经常需要业务数据经常需要和数据库打交道，那么何时打开一个连接，何时开启一个事务，如何保证链接被正确释放则成了我们框架里面一个重要的组成模块。

## 1. 功能说明

工作单元有效作用域范围为Application或Reponsitory层，其中进入的第一个方法，则自动开启工作单元，内部所有方法调用结束后，关闭并释放工作单元。

* 默认不开启工作单元，我们的工作单元仅仅帮助管理DbContext的启用及释放功能；  
* 开启工作单元时，它将控制DbContext的启用及释放，同时控制事务，实现事务的开启提交及回滚操作。

### 1.1 支持的ORM组件

框架支持`EntityFrameworkCore`、`Dapper(DapperExtensions)`组件，理论支持它们所支持的所有数据库。目前项目实际中已测试过 `Mysql`、`Sqlserver`、`Oracle` 数据的数据库操作及事务单元。

### 1.2 初始配置

在`Startup`中启用ORM仓储:
``` csharp
// 默认不启用工作单元
return services.AddCreekdream(
    options =>
    {
        options.UseEfCore();
    });

// 默认启用工作单元
return services.AddCreekdream(
    options =>
    {
        options.UseEfCore(uowOptions =>
        {
            uowOptions.IsTransactional = true;
        });
    });
```

## 2 使用场景

以下示例基于默认不开启工作单元的情况下：

### 2.1 常规属性过滤器使用

进入该方法，开启事务,该方法执行完毕后，如无异常则提交数据库：

``` csharp
[UnitOfWork]
public async Task<GetBookOutput> Add(AddBookInput input)
{
    var book = input.MapTo<Book>();
    // 此时不持久化到数据库
    book = await _bookRepository.InsertAsync(book);

    return book.MapTo<GetBookOutput>();
}
```

### 2.2 局部作用域开启事务

`using` 范围内独立工作单，不受外部影响：

``` csharp
public async Task<GetBookOutput> Add(AddBookInput input)
{
    // 开启一个新的工作单元
    using (var uow = _unitOfWorkManager.Begin())
    {
        var book = input.MapTo<Book>();
        book = await _bookRepository.InsertAsync(book);

        // 如不调用此方法，则不会提交数据库，且不会抛异常
        uow.Complete();

        return book.MapTo<GetBookOutput>();
    }
}

// 需要注意场景
public async Task<GetBookOutput> Update(Guid id, UpdateBookInput input)
{
    var book = await _bookRepository.GetAsync(id);
    // 注：开启模型跟踪时，由于book模型在外部被跟踪修改状态，内部工作单元将无效
    try{
        using (var uow = _unitOfWorkManager.Begin())
        {
            input.MapTo(book);
            book = await _bookRepository.UpdateAsync(book);

            throw new Exception("test");
        }
    } cacth { }
    
    return book.MapTo<GetBookOutput>();
}
```

### 2.3 共享工作单元

当外部已存在工作单元时，在开启工作单元时，可以指定共享外部工作单元。
``` csharp
[UnitOfWork]
public async Task<GetBookOutput> Add(AddBookInput input)
{
    using (var uow = _unitOfWorkManager.Begin(requiresNew: false))
    {
        var book = input.MapTo<Book>();
        book = await _bookRepository.InsertAsync(book);

        // 此时无效，数据持久化操作将交由方法过滤器上开启的工作单元
        uow.Complete();

        return book.MapTo<GetBookOutput>();
    }
}
```

### 2.4 多线程中独立工作单元

项目业务开发中，有时需要优先返回信息给前端，后端异步处理数据：
``` csharp
public async Task<GetBookOutput> Add(AddBookInput input)
{
    // 以下为工作单元多种适用场景
    new Thread(() =>
    {
        Thread.Sleep(10000);
        try
        {
            // 正常使用
            var books = _bookRepository.GetListAsync(m => m.Name.Contains("test")).Result;
            
            // 当使用Query查询时，需要独立开启，调用Application层则不受影响
            using (var uow = _unitOfWorkManager.Begin())
            {
                var query = _bookRepository.GetQueryIncluding(p => p.User);
                var totalCount = query.CountAsync().Result;

                uow.Complete();

                return book.MapTo<GetBookOutput>();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }).Start();
    book = await _bookRepository.InsertAsync(book);
    return book.MapTo<GetBookOutput>();
}
```

