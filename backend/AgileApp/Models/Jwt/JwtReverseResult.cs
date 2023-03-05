using System.Security.Claims;

namespace AgileApp.Models.Jwt
{
    public class JwtReverseResult
    {
        public bool IsValid { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public static JwtReverseResult Invalid() => new JwtReverseResult { IsValid = false };

        public static JwtReverseResult Valid(IEnumerable<Claim> claims) => new JwtReverseResult { IsValid = true, Claims = claims };
    }
}
