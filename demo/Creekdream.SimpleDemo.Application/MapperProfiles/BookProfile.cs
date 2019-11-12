using AutoMapper;
using Creekdream.Domain.Repositories;
using Creekdream.SimpleDemo.Books;
using Creekdream.SimpleDemo.Books.Dto;
using Creekdream.SimpleDemo.UserManage;
using System;

namespace Creekdream.SimpleDemo.MapperProfiles
{
    /// <summary>
    /// Model mapping of book entity
    /// </summary>
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

    /// <summary>
    /// 依赖反转
    /// </summary>
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
}
