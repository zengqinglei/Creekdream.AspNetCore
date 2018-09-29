using Creekdream.Mapping.AutoMapper.Tests.Entities;
using System;

namespace Creekdream.Mapping.AutoMapper.Tests.Dtos
{
    public class GetMyEntityOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public StatusType Status { get; set; }

        public string StatusName { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
