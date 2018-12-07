using Creekdream.Orm.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Creekdream.SimpleDemo.Books;
using Creekdream.SimpleDemo.UserManage;
using System;

namespace Creekdream.SimpleDemo.EntityFrameworkCore
{
    /// <summary>
    /// Data access context
    /// </summary>
    public class SimpleDemoDbContext : DbContextBase
    {
        private readonly string _id;

        public SimpleDemoDbContext(DbContextOptions<SimpleDemoDbContext> options)
            : base(options)
        {
            _id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// user of linq queries
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// userinfo of linq queries
        /// </summary>
        public DbSet<UserInfo> UserInfos { get; set; }

        /// <summary>
        /// book of linq queries
        /// </summary>
        public DbSet<Book> Books { get; set; }

        public override void Dispose()
        {
           base.Dispose();
        }
    }
}


