# Creekdream.AspNetCore 框架之 Dto映射

API或者MVC的接口开发过程中，实体(Entity)与UI的模型(Dto)之间需要相互转化，我们这里使用AutoMapper来完成模型之间的映射工作。

## 初始化工作

在模块设计上，仍然尽可能保持原类库的写法以及配置方式，使用起来更加平滑。

```
Install-Package AutoMapper
Install-Package AutoMapper.Extensions.Microsoft.DependencyInjection
```

### 使用AutoMappe Profile配置
``` csharp
public class BookProfile : Profile
{
    /// <inheritdoc />
    public BookProfile()
    {
        CreateMap<Book, GetBookOutput>().ForMember(
            t => t.UserName,
            opts => opts.MapFrom<DependencyResolver>()
        );
        CreateMap<CreateBookInput, Book>();
        CreateMap<UpdateBookInput, Book>();
    }
}

public class DependencyResolver : IValueResolver<Book, GetBookOutput, string>
{
    private readonly IRepository<User, Guid> _userRepository;

    /// <inheritdoc />
    public DependencyResolver(IRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public string Resolve(Book source, GetBookOutput destination, string destMember, ResolutionContext context)
    {
        var user = _userRepository.GetAsync(source.UserId)
            .GetAwaiter()
            .GetResult();
        return user?.UserName;
    }
}
```

### 在框架中配置
``` csharp
public static ServicesBuilderOptions AddSimpleDemoApplication(this ServicesBuilderOptions builder)
{
    builder.Services.AddAutoMapper(typeof(BookProfile), typeof(UserProfile));
    return builder;
}
```

### 使用示例

#### 注入Mapper
``` csharp
private readonly IMapper _mapper;

public UserService(IMapper mapper)
{
    _mapper = mapper;
}
```

#### Entity 转换为 Dto
``` csharp
var books = _bookRepository.GetQueryIncluding(p => p.User).ToListAsync();
var booksOutput = _mapper.Map<List<GetBookOutput>>(books);
```

#### Dto 转换为 Entity
``` csharp
var input = new AddBookInput();
var book = _mapper.Map<Book>(input);
book = await _bookRepository.InsertAsync(book);
```

#### 实体覆盖赋值
``` csharp
var book = await _bookRepository.GetAsync(id);
_mapper.Map(input, book);
```

更多的关于AutoMapper的使用方式，请参考[官方文档](http://docs.automapper.org/en/stable/Getting-started.html)！