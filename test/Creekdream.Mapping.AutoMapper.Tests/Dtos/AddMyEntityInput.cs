using Creekdream.Mapping.AutoMapper.Tests.Entities;

namespace Creekdream.Mapping.AutoMapper.Tests.Dtos
{
    public class AddMyEntityInput
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public StatusType Status { get; set; }
    }
}
