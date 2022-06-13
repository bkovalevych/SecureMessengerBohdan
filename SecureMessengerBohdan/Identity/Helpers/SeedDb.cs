using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Identity.Helpers
{
    public static class SeedDb
    {
        public static async Task Invoke(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var applicationContext = provider.GetRequiredService<ApplicationDbContext>();
            var firstUser = new ApplicationUser
            {
                UserName = "biba",
                Email = "biba@gmail.com",
                FirstName = "Biba",
                LastName = "LastName"
            };
            var secondUser = new ApplicationUser
            {
                UserName = "boba",
                Email = "boba@gmail.com",
                FirstName = "Boba",
                LastName = "LastName"
            };

            if (await userManager.FindByEmailAsync(firstUser.Email) == null)
            {
                await userManager.CreateAsync(firstUser, "12345678");
                
            }
            if (await userManager.FindByEmailAsync(secondUser.Email) == null)
            {
                await userManager.CreateAsync(secondUser, "12345678");
                await applicationContext.ChatRecord.InsertOneAsync(new Chat()
                {
                    Members = new List<ApplicationUser>()
                    {
                        firstUser,
                        secondUser
                    },
                    Name = $"{firstUser.UserName} {secondUser.UserName}"
                });
            }
            //await applicationContext.MessageRecord.InsertOneAsync(new Message()
            //{
            //    ChatId = applicationContext.ChatRecord.AsQueryable().First().Id,
            //    ChatName = applicationContext.ChatRecord.AsQueryable().First().Name,
            //    FromId = firstUser.Id,
            //    FromName = firstUser.UserName,
            //    Sent = DateTimeOffset.Now,
            //    Text = "hello"
            //});
        }
    }
}
