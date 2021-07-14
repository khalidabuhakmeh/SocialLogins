using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SocialLogins
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Step 0.
            //  From the root of the solution, use the terminal to
            //  Add github:clientId and github:clientSecret to Project User Secrets
            // ‣ dotnet user-secrets set github:clientId "<from github>" --project SocialLogins
            // ‣ dotnet user-secrets set github:clientSecret "<from github>" --project SocialLogins
            // ‣ dotnet user-secrets list
            //    github:clientSecret = <secret>
            //    github:clientId = <secret>
            
            // Step 1. Add Authentication Mechanisms
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(o =>
                {
                    // set the path for the authentication challenge
                    o.LoginPath = "/signin";
                    // set the path for the sign out
                    o.LogoutPath = "/signout";
                })
                .AddGitHub(o =>
                {
                    o.ClientId = Configuration["github:clientId"];
                    o.ClientSecret = Configuration["github:clientSecret"];
                    o.CallbackPath = "/signin-github";
                    
                    // Grants access to read a user's profile data.
                    // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
                    o.Scope.Add("read:user");

                    // Optional
                    // if you need an access token to call GitHub Apis
                    o.Events.OnCreatingTicket += context =>
                    {
                        if (context.AccessToken is { })
                        {
                            context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));
                        }
                        
                        return Task.CompletedTask;
                    };
                });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            
            // 2. add Authentication and Authorization Middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                
                // 3. Sign out Endpoint
                // go to ./Pages/Signin.cshtml for 4.
                endpoints.MapGet("/signout", async ctx =>
                {
                    await ctx.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new AuthenticationProperties
                        {
                            RedirectUri = "/"
                        });
                });

            });
        }
    }
}