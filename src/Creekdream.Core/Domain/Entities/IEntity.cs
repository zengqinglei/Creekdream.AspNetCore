namespace Creekdream.Domain.Entities
{
    /// <summary>
    /// Entity base interface
    /// </summary>
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Primary key unique Id
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}