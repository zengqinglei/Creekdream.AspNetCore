# Creekdream AspNetCore 框架
Creekdream AspNetCore 致力于打造原生、简洁、清晰化结构的 .NET CORE 项目架构设计，在组件以及框架的封装尽可能遵从 .NET CORE 的设计理念。

## 框架特点
框架吸收了 [**ABP**](https://github.com/aspnetboilerplate/aspnetboilerplate) 等一些开源框架的优秀设计，由于近几年微服务盛行，框架设计原则尽可能从简单、易用。
在模块化设计上尽可能独立，在核心类库上尽可能减少依赖，开发者一个干净整洁的体验。

## 框架特性

**框架基础核心模块：**
* 依赖注入：Autofac、Castle.Windsor
* 模块化
* Model映射：AutoMapper
* ORM：EntityFrameworkCore、Dapper支持主流数据库
* 工作单元支持
* AOP切面拦截器
* 单元测试：xunit

**框架独立模块**

不依赖项目框架，独立模块有自己的核心接口层，可自行根据不同需求多实现。
* 缓存模块：Redis、MemoryCache
* 消息队列
* Apollo配置中心
* Consul服务中心

## 微服务独立项目
* API网关(Ocelot + Consul)
* IdentityServer4认证授权项目

## 框架示例及模板

* [快速创建项目模板](https://github.com/zengqinglei/Creekdream.AspNetCore.Template)
* [EntityFrameworkCore 的简单示例项目](https://github.com/zengqinglei/Creekdream.SimpleDemo)
* [Dapper 的简单示例项目](https://github.com/zengqinglei/Creekdream.SimpleDemo/tree/dapper)

框架示例领域驱动分层：
* 应用层
* 核心领域层
* 仓储层
* UI层

## 参与贡献
1. Fork Creekdream.AspNetCore 开源框架
2. 新建 feature-\{tag} 分支
3. 完成功能并提交代码
4. 新建 Pull Request

[**更新日志**](https://github.com/zengqinglei/Creekdream.AspNetCore/releases)