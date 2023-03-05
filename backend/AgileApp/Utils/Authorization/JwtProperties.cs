using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AgileApp.Utils.Authorization
{
    public class JwtProperties
    {
        public int ExpireMinutes { get; set; } = 2880;

        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public Claim[] Claims { get; set; }
    }
}
