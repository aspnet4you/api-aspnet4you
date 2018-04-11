using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Text;
using System;
using System.IdentityModel.Tokens;

namespace api.aspnet4you.mvc5
{
    public class JwtTokenHelper
    {
        private readonly System.IdentityModel.Tokens.JwtSecurityTokenHandler jwtSecurityTokenHandler;

        public JwtTokenHelper()
        {
            this.jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateJwtToken(string issuer, string audience, IEnumerable<Claim> claims, DateTime? notBefore, DateTime? expires)
        {
            string audienceSecreteKey = GlobalConstants.JwtTopSecrete512;

            string audienceSecreteBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(audienceSecreteKey));
            byte[] audienceSecrete = Convert.FromBase64String(audienceSecreteBase64);

            SymmetricSecurityKey securityKey = new InMemorySymmetricSecurityKey(audienceSecrete);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);    // HmacSha256: to be able to debug @jwt.io but you can go higher

            JwtPayload payload = new JwtPayload(issuer, audience, claims, notBefore, expires);
            System.IdentityModel.Tokens.JwtHeader header = new System.IdentityModel.Tokens.JwtHeader(signingCredentials);
            System.IdentityModel.Tokens.JwtSecurityToken securityToken = new System.IdentityModel.Tokens.JwtSecurityToken(header, payload);
            
            return this.jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}