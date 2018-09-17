using System;

namespace Creekdream.Domain.Entities.Auditing
{
    /// <summary>
    /// Inherit from this interface must have a creation time
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}