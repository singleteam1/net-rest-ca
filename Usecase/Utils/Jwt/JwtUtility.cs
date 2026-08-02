using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Auth;
using Shared.Common.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Usecase.Utils.Jwt
{
    public interface IJwtUtility
    {
        JwtTokenResponse CreateToken(IEnumerable<Claim> claims);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }

    public class JwtUtility: IJwtUtility
    {
        private readonly JwtEnvironment _jwtEnvironment;

        public JwtUtility(IOptions<JwtEnvironment> jwtEnvironment)
        {
            _jwtEnvironment = jwtEnvironment.Value;
        }

        public JwtTokenResponse CreateToken(IEnumerable<Claim> claims)
        {
            string jwtPrivate = this.GenerateSecurityToken(claims);
            string refreshToken = Guid.NewGuid().ToString();
            return new JwtTokenResponse
            {
                AccessToken = jwtPrivate,
                RefreshToken = refreshToken,
            };
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _jwtEnvironment.Audience,
                ValidIssuer = _jwtEnvironment.Issuer,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtEnvironment.PrivateKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        private string GenerateSecurityToken(IEnumerable<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtEnvironment.PrivateKey));

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtEnvironment.Issuer,
                audience: _jwtEnvironment.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddSeconds(int.Parse(_jwtEnvironment.TokenValiditySeconds)),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256));

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
