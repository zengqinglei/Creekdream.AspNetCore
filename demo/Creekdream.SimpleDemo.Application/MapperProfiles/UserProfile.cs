using AutoMapper;
using Creekdream.SimpleDemo.UserManage;
using Creekdream.SimpleDemo.UserManage.Dto;

namespace Creekdream.SimpleDemo.MapperProfiles
{
    /// <summary>
    /// Model mapping of user entity
    /// </summary>
    public class UserProfile : Profile
    {
        /// <inheritdoc />
        public UserProfile()
        {
            CreateMap<User, GetUserOutput>().ForMember(
                t => t.Name,
                opts => opts.MapFrom(d => d.UserInfo == null ? null : d.UserInfo.Name)
            ).ForMember(
                t => t.Age,
                opts => opts.MapFrom(d => d.UserInfo == null ? 0 : d.UserInfo.Age)
            );
            CreateMap<CreateUserInput, User>();
        }
    }
}
