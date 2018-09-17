using System;

namespace Creekdream.Domain.Entities.Auditing
{
    /// <summary>
    /// Inherit from this interface must contain delete time
    /// </summary>
    public interface IHasDeletionTime : ISoftDelete
    {
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}