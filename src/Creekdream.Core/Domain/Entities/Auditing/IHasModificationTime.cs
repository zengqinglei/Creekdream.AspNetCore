using System;

namespace Creekdream.Domain.Entities.Auditing
{
    /// <summary>
    /// Inherit from this interface must have modification time
    /// </summary>
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }
}