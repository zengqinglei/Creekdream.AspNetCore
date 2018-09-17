namespace Creekdream.Domain.Entities
{
    /// <inheritdoc />
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <inheritdoc />
        public virtual TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// Entity base class, the default primary key is an integer
    /// </summary>
    public abstract class Entity : Entity<int>
    {

    }
}