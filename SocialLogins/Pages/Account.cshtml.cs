using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using SocialLogins.Models;

namespace SocialLogins.Pages
{
    [Authorize]
    public class Account : PageModel
    {
        public async Task OnGet()
        {
            // 6. We are reading claims that were
            //    supplied from our OpenID provider
            Claims = User.Claims.ToList();
            
            // 6. From GitHub
            if (User.AccessToken() is { } accessToken)
            {
                var client = new GitHubClient(new ProductHeaderValue("test")) {
                    Credentials = new Credentials(accessToken)
                };
                GitHubUser = await client.User.Get(User.Identity?.Name);
            }
        }

        public User GitHubUser { get; set; }
        public List<Claim> Claims { get; set; }
    }
}