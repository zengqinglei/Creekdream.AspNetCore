using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Creekdream.SimpleDemo.Books;
using Creekdream.SimpleDemo.EntityFrameworkCore;
using Creekdream.SimpleDemo.UserManage;
using System;
using System.Threading.Tasks;

namespace Creekdream.SimpleDemo.Migrations
{
    /// <summary>
    /// Initialize the data required by the application
    /// </summary>
    public class SeedData
    {
        /// <inheritdoc />
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<DbContext>() as SimpleDemoDbContext;
                context.Database.Migrate();

                var user = new User
                {
                    UserName = "zengql",
                    Password = "123456",
                    CreationTime = DateTime.Now
                };
                if (!await context.Users.AnyAsync(m => m.UserName == user.UserName))
                {
                    await context.Users.AddAsync(user);

                    var userInfo = new UserInfo
                    {
                        Id = user.Id,
                        Name = "zengqinglei",
                        Age = 18,
                        CreationTime = DateTime.Now
                    };
                    await context.UserInfos.AddAsync(userInfo);

                    for (int i = 0; i < 100; i++)
                    {
                        var book = new Book
                        {
                            Name = $"Book-{i}",
                            UserId = user.Id,
                            CreationTime = DateTime.Now
                        };
                        await context.Books.AddAsync(book);
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

