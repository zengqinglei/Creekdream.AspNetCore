using System;

namespace Creekdream.Mapping.AutoMapper.Tests.Entities
{
    public enum StatusType
    {
        Disable = 0,
        Enable = 1
    }

    public class MyEntity
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual bool IsDeleted { get; set; }

        public virtual StatusType Status { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public MyEntity()
        {
            Id = Guid.NewGuid();
            CreationTime = DateTime.Now;
        }
    }
}
