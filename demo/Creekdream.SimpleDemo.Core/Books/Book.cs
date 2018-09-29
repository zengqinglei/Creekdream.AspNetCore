using Creekdream.Domain.Entities;
using Creekdream.Domain.Entities.Auditing;
using Creekdream.SimpleDemo.UserManage;
using System;
using System.ComponentModel.DataAnnotations;

namespace Creekdream.SimpleDemo.Books
{
    /// <summary>
    /// Book information
    /// </summary>
    public class Book : Entity<Guid>, IHasCreationTime
    {
        public const int MaxNameLength = 50;

        /// <summary>
        /// User Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Book name
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Creation time of this book.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// User of the book information
        /// </summary>
        public virtual User User { get; set; }
    }
}


