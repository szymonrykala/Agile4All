using AgileApp.Models.Jwt;

namespace AgileApp.Utils.Authorization
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Validates token and if is valid returns claims
        /// </summary>
        /// <param name="token">Token given to validation</param>
        /// <returns>Result of reverse operation</returns>
        JwtReverseResult ReverseTokenContent(string token);

        /// <summary>
        /// Generates token by given email
        /// </summary>
        /// <param name="email">Email used to log in</param>
        /// <param name="hash">Hash to identify user in database</param>
        /// <returns>Generated token or empty string if error occured</returns>
        string GenerateTokenFromLoginData(string email, int id, int role);
    }
}
