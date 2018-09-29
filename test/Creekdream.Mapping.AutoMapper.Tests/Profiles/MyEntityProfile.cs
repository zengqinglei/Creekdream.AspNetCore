using AutoMapper;
using Creekdream.Mapping.AutoMapper.Tests.Dtos;
using Creekdream.Mapping.AutoMapper.Tests.Entities;

namespace Creekdream.Mapping.AutoMapper.Tests.Profiles
{
    public class MyEntityProfile : Profile
    {
        public MyEntityProfile()
        {
            CreateMap<MyEntity, GetMyEntityOutput>().ForMember(
                t => t.StatusName,
                opts => opts.MapFrom(d => d.Status.ToString())
            );
        }
    }
}
