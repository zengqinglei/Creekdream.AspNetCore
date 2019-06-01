# Creekdream.AspNetCore 框架之 Dto映射

API或者MVC的接口开发过程中，实体(Entity)与UI的模型(Dto)之间需要相互转化，我们这里使用AutoMapper来完成模型之间的映射工作。

## 初始化工作

在模块设计上，仍然尽可能保持原类库的写法以及配置方式，使用起来更加平滑。

### 使用AutoMappe Profile配置
``` csharp
/// <summary>
/// Model mapping of book entity
/// </summary>
public class BookProfile : Profile, ISingletonDependency
{
    /// <inheritdoc />
    public BookProfile(IBookService bookService)
    {
        // TODO: Use bookService do something

        CreateMap<Book, GetBookOutput>().ForMember(
            t => t.UserName,
            opts => opts.MapFrom(d => d.User.UserName)
        );
    }
}
```

### 在框架中配置
``` csharp
/// <inheritdoc />
public class Startup
{
    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseCreekdream(
            options =>
            {
                options.UseAutoMapper(
                    config =>
                    {
                        config.AddProfile(options.IocResolver.Resolve<BookProfile>());
                    });
            });
    }
}
```

### 使用示例

#### Entity 转换为 Dto
``` csharp
var books = _bookRepository.GetQueryIncluding(p => p.User).ToListAsync();
var booksOutput = books.MapTo<List<GetBookOutput>>();
```

#### Dto 转换为 Entity
``` csharp
var input = new AddBookInput();
var book = input.MapTo<Book>();
book = await _bookRepository.InsertAsync(book);
```

更多的关于AutoMapper的使用方式，请参考[官方文档](http://docs.automapper.org/en/stable/Getting-started.html)！