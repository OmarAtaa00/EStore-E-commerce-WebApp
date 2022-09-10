using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                //New user
                var user = new User
                {
                    DisplayName = "Omar Potato",
                    Email = "omar.ataa@outlook.com",
                    UserName = "omar.ataa@outlook.com",
                    Address = new Address
                    {
                        FirstName = "Omar",
                        LastName = "Ataa",
                        Street = "RadySt.",
                        City = "Tanta",
                        ZipCode = "15111",
                    }

                };
                await userManager.CreateAsync(user, "Pa$$w0rd"); //Must be complex one capital one small special char and a number.


            }

        }
    }
}

