using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace SocialLogins.Pages
{
    public class SignIn : PageModel
    {
        public IEnumerable<AuthenticationScheme> Schemes { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public async Task OnGet()
        {
            Schemes = await GetExternalProvidersAsync(HttpContext);
        }

        /// <summary>
        /// 5. Handle the chosen provider by triggering the Challenge
        /// request for the user. After authenticating, try going to
        /// https://localhost:5001/Account to see claims and values from GitHub
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPost([FromForm] string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }

            return await IsProviderSupportedAsync(HttpContext, provider) is false
                ? BadRequest()
                : Challenge(new AuthenticationProperties
                {
                    RedirectUri = Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/"
                }, provider);
        }

        private static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(HttpContext context)
        {
            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            return (await schemes.GetAllSchemesAsync())
                .Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName))
                .ToArray();
        }

        private static async Task<bool> IsProviderSupportedAsync(HttpContext context, string provider) =>
            (await GetExternalProvidersAsync(context))
            .Any(scheme => string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase));
    }
}