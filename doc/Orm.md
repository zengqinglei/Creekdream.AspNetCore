# Creekdream.AspNetCore 框架之 数据仓储(ORM)

框架目前支持 `EntityFrameworkCore`、`Dapper(DapperExtensions)` 两种ORM组件。

## EFCore 的实现功能

**(Demo)[https://github.com/zengqinglei/Creekdream.SimpleDemo]**

* 基于工作单元，实现了DbContext的 `创建`、`释放` 操作
* 基于工作单元，实现了事务的 `开启`、`提交`、`回滚`、`释放` 操作
* 提供出仓储基本的操作API外，额外拓展：`GetQueryIncluding` 用以获取获取自定义查询
* 提供出仓储基本的操作API外，额外拓展：`QueryAsync`、`ExecuteNonQueryAsync`、`ExecuteScalarAsync` 用以支持sql语句查询

**目前已测试 `Mysql`、`MSSQL` 数据库的数据持久化以及事务操作**(*理论支持EFCore支持的所有数据库*)

### 使用示例

#### 1. 新建仓储类库：Creekdream.SimpleDemo.EntityFrameworkCore

>引入ORM组件：`Install-Package Creekdream.Orm.EntityFrameworkCore`  
引入基于EFCore实现的数据库组件：`Install-Package Microsoft.EntityFrameworkCore.SqlServer`

#### 2. 创建 `DbContext`

``` csharp
/// <summary>
/// Data access context
/// </summary>
public class SimpleDemoDbContext : DbContextBase
{
    public SimpleDemoDbContext(DbContextOptions<SimpleDemoDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// book of linq queries
    /// </summary>
    public DbSet<Book> Books { get; set; }
}
```

#### 3. 在 `Startup` 中启用
``` csharp
// 在 ConfigureServices 加入如下配置
services.AddDbContext<DbContextBase, SimpleDemoDbContext>(
    options =>
    {
        options.UseSqlServer(
            _configuration.GetConnectionString("Default"),
            option => option.UseRowNumberForPaging());
    });

return services.AddCreekdream(
    options =>
    {
        options.UseEfCore();
    });
```

## Dapper 的实现功能

**(Demo)[https://github.com/zengqinglei/Creekdream.SimpleDemo/tree/dapper]**

* 基于工作单元，实现了DbContext的 `创建`、`释放` 操作
* 基于工作单元，实现了事务的 `开启`、`提交`、`回滚`、`释放` 操作
* 提供出仓储基本的操作API外，额外拓展：`GetPaged` 用以分页查询
* 提供出仓储基本的操作API外，额外拓展：`QueryAsync`、`ExecuteNonQueryAsync`、`ExecuteScalarAsync` 用以支持sql语句查询

**目前已测试 `Mysql`、`MSSQL`、`Oracle` 数据库的数据持久化以及事务操作**(*理论支持Dapper支持的所有数据库*)

### 使用示例

#### 1. 新建仓储类库：Creekdream.SimpleDemo.Dapper

>引入ORM组件：`Install-Package Creekdream.Orm.Dapper`  
引入基于Dapper实现的数据库组件：`Install-Package Creekdream.Orm.Dapper.MySql`

#### 2. 创建模型映射配置

``` csharp
/// <summary>
/// Book entity and table mapping
/// </summary>
public class BookMapper : ClassMapper<Book>
{
    public BookMapper()
    {
        Table("Books");
        Map(x => x.Id).Key(KeyType.Guid);
        Map(x => x.User).Ignore();
        AutoMap();
    }
}
```

#### 3. 在 `Startup` 中启用
``` csharp
// 在 ConfigureServices 加入如下配置
services.AddDbConnection(
    options =>
    {
        options.UseMySql(_configuration.GetConnectionString("Default"));
        // 此处仅需配置一次即可
        options.MapperAssemblies.Add(typeof(BookMapper).Assembly);
    });

return services.AddCreekdream(
    options =>
    {
        options.UseDapper();
    });
```