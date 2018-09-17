namespace Creekdream.Domain.Entities
{
    /// <summary>
    /// Soft delete interface
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Is deleted
        /// </summary>
        bool IsDeleted { get; set; }
    }
}