using api.aspnet4you.mvc5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.LinkedIn;
using System.Configuration;

namespace api.aspnet4you.mvc5
{
    public partial class Startup
    {
        private static string LinkedInClientId = ConfigurationManager.AppSettings[GlobalConstants.LinkedInClientId];
        private static string LinkedInClientSecrete = ConfigurationManager.AppSettings[GlobalConstants.LinkedInClientSecrete];
        private static string LinkedInCallbackPath = ConfigurationManager.AppSettings[GlobalConstants.LinkedInCallbackPath];
        private static string CookieLoginPath = ConfigurationManager.AppSettings[GlobalConstants.CookieLoginPath];

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.UseCustomJwtBearerToken();

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                AuthenticationMode = AuthenticationMode.Passive,
                LoginPath = new PathString(CookieLoginPath)

            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            LinkedInAuthenticationOptions inOptions = new LinkedInAuthenticationOptions();
            inOptions.ClientId = LinkedInClientId;
            inOptions.ClientSecret = LinkedInClientSecrete;
            inOptions.CallbackPath = new PathString(LinkedInCallbackPath);
            app.UseLinkedInAuthentication(inOptions);

            
        }
    }
}