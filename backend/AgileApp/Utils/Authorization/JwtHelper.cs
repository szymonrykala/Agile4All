using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgileApp.Models.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace AgileApp.Utils.Authorization
{
    public class JwtHelper : IJwtHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;

        public JwtHelper(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("JwtConfiguration:SecretKey");
            _issuer = configuration.GetValue<string>("JwtConfiguration:Issuer");
        }

        public JwtReverseResult ReverseTokenContent(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return JwtReverseResult.Invalid();
                }

                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = GetSymmetricSecurityKey(_secretKey),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _issuer
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityInfo);

                return JwtReverseResult.Valid(claims.Claims);
            }
            catch (Exception ex)
            {
                //_logger.LogCritical(ex.ToString());
                return JwtReverseResult.Invalid();
            }
        }

        public string GenerateTokenFromLoginData(string email, int id, int role) =>
            GenerateToken(new JwtProperties
            {
                Claims = new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Hash, id.ToString()),
                    new Claim(ClaimTypes.Role, role.ToString())
                }
            });

        #region Private methods

        private string GenerateToken(JwtProperties properties)
        {
            try
            {
                if (properties?.Claims == null || properties.Claims.Length == 0)
                {
                    return string.Empty;
                }

                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(properties.Claims),
                    Expires = DateTime.UtcNow.AddMinutes(properties.ExpireMinutes),
                    SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(_secretKey), properties.SecurityAlgorithm),
                    Issuer = _issuer,
                    Audience = _issuer
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);

                return tokenHandler.WriteToken(securityToken);
            }
            catch (Exception ex)
            {
                //_logger.LogCritical(ex.ToString());
                return string.Empty;
            }
        }

        private static SecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            byte[] symmetricKey = Encoding.ASCII.GetBytes(secretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        #endregion
    }
}
