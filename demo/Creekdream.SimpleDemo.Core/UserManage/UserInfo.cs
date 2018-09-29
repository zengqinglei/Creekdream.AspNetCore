using Creekdream.Domain.Entities;
using Creekdream.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Creekdream.SimpleDemo.UserManage
{
    /// <summary>
    /// User personal information
    /// </summary>
    public class UserInfo : Entity<Guid>, IHasModificationTime, IHasCreationTime
    {
        public const int MaxNameLength = 100;

        /// <summary>
        /// Primary key unique Id
        /// </summary>
        [ForeignKey(nameof(User))]
        [Column("UserId", Order = 1)]
        public override Guid Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Age
        /// </summary>
        public virtual int Age { get; set; }

        /// <summary>
        /// The last modified time for this userinfo.
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Creation time of this userinfo.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// User of the information
        /// </summary>
        public virtual User User { get; set; }
    }
}

