using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Identity.Helpers
{
    public static class SeedDb
    {
        public static async Task Invoke(IServiceProvider provider)
        {
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByEmailAsync("biba@gmail.com") == null)
            {
                await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "biba",
                    Email = "biba@gmail.com",
                    FirstName = "Biba",
                    LastName = "LastName"
                }, "12345678");
            }
            if (await userManager.FindByEmailAsync("boba@gmail.com") == null)
            {
                await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "boba",
                    Email = "boba@gmail.com",
                    FirstName = "Boba",
                    LastName = "LastName"
                }, "12345678");
            }
        }
    }
}
