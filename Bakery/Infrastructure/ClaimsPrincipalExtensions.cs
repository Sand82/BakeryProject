using System.Security.Claims;

namespace Bakery.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        { 
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
