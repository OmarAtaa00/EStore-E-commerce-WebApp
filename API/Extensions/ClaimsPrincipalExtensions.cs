using System;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email); // cleaner version 
            //not so good version 
            //return  User?Claims?.FirstOrDefault(x=> x.Type == ClaimTypes)?.Value
        }
    }
}
