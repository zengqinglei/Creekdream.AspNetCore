using AutoMapper;
using Creekdream.Mapping.AutoMapper.Tests.Dtos;
using Creekdream.Mapping.AutoMapper.Tests.Entities;
using Creekdream.Mapping.AutoMapper.Tests.Profiles;
using Shouldly;
using Xunit;

namespace Creekdream.Mapping.AutoMapper.Tests
{
    public class AutoMapperTest
    {
        static AutoMapperTest()
        {
            Mapper.Initialize(
                options =>
                {
                    options.ValidateInlineMaps = false;
                    options.AddProfile<MyEntityProfile>();
                });
        }

        [Fact]
        public void Test_Mapper_DtoToEntity()
        {
            var input = new AddMyEntityInput()
            {
                Name = nameof(MyEntity),
                Status = StatusType.Enable,
                IsDeleted = false
            };
            var entity = input.MapTo<MyEntity>();
            entity.ShouldNotBeNull();
            entity.Name.ShouldBe(input.Name);
            entity.Status.ShouldBe(input.Status);
            entity.IsDeleted.ShouldBe(input.IsDeleted);
        }

        [Fact]
        public void Test_Mapper_EntityToDto()
        {
            var entity = new MyEntity()
            {
                Name = nameof(MyEntity),
                Status = StatusType.Enable,
                IsDeleted = false
            };
            var output = entity.MapTo<GetMyEntityOutput>();
            output.ShouldNotBeNull();
            output.Id.ShouldBe(entity.Id);
            output.Name.ShouldBe(entity.Name);
            output.Status.ShouldBe(entity.Status);
            output.IsDeleted.ShouldBe(entity.IsDeleted);
            output.CreationTime.ShouldBe(entity.CreationTime);
        }
    }
}
