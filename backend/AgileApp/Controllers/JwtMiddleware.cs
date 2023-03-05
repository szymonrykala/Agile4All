using AgileApp.Enums;

namespace AgileApp.Controllers
{
    public class JwtMiddleware
    {
        public static bool IsAdmin(Models.Jwt.JwtReverseResult reverseTokenResult)
        {
            int adminRole = (int)UserRoleEnum.ADMIN;
            return reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role)?.Value == adminRole.ToString();
        }

        public static UserRoleEnum CheckUserRole(Models.Jwt.JwtReverseResult reverseTokenResult)
        {
            UserRoleEnum userRoleResponse = UserRoleEnum.STUDENT;

            if (reverseTokenResult == null)
                return userRoleResponse;

            string userRole = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userRole))
                return userRoleResponse;

            int role = (int)UserRoleEnum.STUDENT;
            try
            {
                int.TryParse(userRole, out role);
                userRoleResponse = (UserRoleEnum)role;
            }
            catch (Exception)
            {
                return UserRoleEnum.STUDENT;
            }

            return userRoleResponse;
        }

        public static int GetCurrentUserId(Models.Jwt.JwtReverseResult reverseTokenResult)
        {
            int userIdResponse = 0;

            if (reverseTokenResult == null)
                return userIdResponse;

            string userRole = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            if (string.IsNullOrWhiteSpace(userRole))
                return userIdResponse;

            try
            {
                int id = 0;
                int.TryParse(userRole, out id);
                userIdResponse = id;
            }
            catch (Exception)
            {
                return 0;
            }

            return userIdResponse;
        }
    }
}
