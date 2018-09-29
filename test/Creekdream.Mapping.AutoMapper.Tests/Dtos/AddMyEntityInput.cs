using Creekdream.Mapping.AutoMapper.Tests.Entities;
using System;

namespace Creekdream.Mapping.AutoMapper.Tests.Dtos
{
    public class AddMyEntityInput
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public StatusType Status { get; set; }
    }
}
