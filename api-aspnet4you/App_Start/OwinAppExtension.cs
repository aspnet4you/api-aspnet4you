using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Owin;
using System;
using System.Text;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.Configuration;

namespace api.aspnet4you.mvc5
{
    public static class OwinAppExtension
    {
        private static string AllowedAudiences = ConfigurationManager.AppSettings[GlobalConstants.AllowedAudiences];
        private static string AllowedIssuers = ConfigurationManager.AppSettings[GlobalConstants.AllowedIssuers];

        public static void UseCustomJwtBearerToken(this IAppBuilder app)
        {
            string[] audiences = AllowedAudiences.Split(new char[] {',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] issuers = AllowedIssuers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string audienceSecreteKey = GlobalConstants.JwtTopSecrete512;

            

            string audienceSecreteBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(audienceSecreteKey));
            byte[] audienceSecrete = Convert.FromBase64String(audienceSecreteBase64);

            app.UseJwtBearerAuthentication(
               new JwtBearerAuthenticationOptions()
               {
                   AuthenticationMode = AuthenticationMode.Active,
                   TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters()
                   {
                       IssuerSigningKey = new InMemorySymmetricSecurityKey(audienceSecrete),
                       ValidateIssuerSigningKey = true,
                       ValidateLifetime = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidAudiences = audiences,
                       ValidIssuers = issuers,
                       ValidateActor = true,
                       CertificateValidator = X509CertificateValidator.None,
                       IssuerSigningKeyResolver = CustomSigningResolver
                   },
                   TokenHandler = new CustomJwtSecurityTokenHandler() { UseCustomValidator = false}      
               }
            );
        }

        private static SecurityKey CustomSigningResolver(string token, SecurityToken securityToken, SecurityKeyIdentifier keyIdentifier, TokenValidationParameters validationParameters)
        {
            SecurityKey securityKey = validationParameters.IssuerSigningKey;

            return securityKey;
        }
    }
}