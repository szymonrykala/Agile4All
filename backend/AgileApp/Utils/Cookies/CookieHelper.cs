using AgileApp.Models.Common;
using AgileApp.Models.Jwt;
using AgileApp.Utils.Authorization;

namespace AgileApp.Utils.Cookies
{
    public class CookieHelper : ICookieHelper
    {
        private readonly IJwtHelper _jwtHelper;

        public CookieHelper(IJwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        public void AddJwtToHttpOnlyResponseCookie(HttpContext context, string email, int id, int role)
        {
            string jwtToken = _jwtHelper.GenerateTokenFromLoginData(email, id, role);
            context.Response.Cookies.Append(AppSettings.JwtCookieKey,
                jwtToken,
                new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMonths(AppSettings.ValidCookieMonthsAmount),
                    SameSite = SameSiteMode.None
                });
        }

        public string ReturnJwtTokenString(HttpContext context, string email, int id, int role)
        {
            string jwtToken = _jwtHelper.GenerateTokenFromLoginData(email, id, role);
            return jwtToken;
        }

        public Response InvalidateJwtCookie(HttpContext context)
        {
            try
            {
                bool hasToken = context.Request.Cookies.ContainsKey(AppSettings.JwtCookieKey);

                if (!hasToken)
                {
                    return Response.Succeeded();
                }

                DeleteCookie(context, AppSettings.JwtCookieKey);
                return Response.Succeeded();
            }
            catch (Exception ex)
            {
                //_logger.LogCritical(ex.ToString());
                return Response.Failed("");
            }
        }

        public JwtReverseResult ReverseJwtFromRequest(HttpContext context)
        {
            string token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrWhiteSpace(token))
            {
                return JwtReverseResult.Invalid();
            }

            return _jwtHelper.ReverseTokenContent(token);
        }

        private void DeleteCookie(HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key,
                new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                });
        }
    }
}
