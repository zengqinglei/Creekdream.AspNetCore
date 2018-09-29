using AutoMapper;
using Creekdream.Dependency;
using Creekdream.SimpleDemo.Books;
using Creekdream.SimpleDemo.Books.Dto;

namespace Creekdream.SimpleDemo.MapperProfiles
{
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
}
