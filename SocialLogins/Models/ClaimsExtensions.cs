#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace SocialLogins.Models
{
    public static class ClaimsExtensions
    {
        public static string? FirstClaim(this IEnumerable<Claim>? claims, string type)
        {
            return claims?
                .Where(c => c.Type == type)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static string? AccessToken(this ClaimsPrincipal principal) => 
            principal.Claims.FirstClaim("access_token");
    }
}