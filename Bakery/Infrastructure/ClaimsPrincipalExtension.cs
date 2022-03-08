using System.Security.Claims;

namespace Bakery.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            var currUser = user.FindFirstValue(ClaimTypes.NameIdentifier);
            return currUser;
        }
    }
}
