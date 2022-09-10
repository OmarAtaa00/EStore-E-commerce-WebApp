using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> FindUserByClaimWithAddressAsync(this UserManager<User> input,
        ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email); //the clean version 
            //another way to add this line again 
            // var email = user.Claims?.firstOrDefault(x => x.Type == claimTypes.Email)?.Value;

            return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);

        }
        public static async Task<User> FindByEmailFormClaims(this UserManager<User> input,
        ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email); //the clean version 

            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);

        }

    }
}
