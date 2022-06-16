using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Models;
using SecureMessengerBohdan.Security.Requests.InitKeyForChat;

namespace SecureMessengerBohdan.Identity.Helpers
{
    public static class SeedDb
    {
        public static async Task Invoke(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var applicationContext = provider.GetRequiredService<ApplicationDbContext>();
            var sender = provider.GetRequiredService<ISender>();

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
                var chat = new Chat()
                {
                    Members = new List<string>()
                    {
                        firstUser.Id.ToString(),
                        secondUser.Id.ToString(),
                    },
                    Name = $"{firstUser.UserName} {secondUser.UserName}"
                };
                await applicationContext.ChatRecord.InsertOneAsync(chat);
                await sender.Send(new InitKeyForChatRequest() { ChatId = chat.Id });
            }
        }
    }
}
