using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace api.aspnet4you.mvc5
{
    internal class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public bool UseCustomValidator { get; set; }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            //TODO:Check the token against cache and/or with issuer.
            return BaseValidateToken(securityToken, validationParameters, out validatedToken);
        }

        public ClaimsPrincipal BaseValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            ClaimsPrincipal claimsPrincipal=  base.ValidateToken(securityToken, validationParameters, out validatedToken);

            return claimsPrincipal;
        }
    }
}