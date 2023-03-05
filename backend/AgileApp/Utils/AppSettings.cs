namespace AgileApp.Utils
{
    public class AppSettings
    {
        #region RegularExpressionss

        public const string EmailExpression = @"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        #endregion

        #region JWT Configuration

        public const string JwtCookieKey = "AgileApiJWT";

        #endregion

        #region Cookies configuration

        public const int TokenLength = 64;

        public const int ValidCookieMonthsAmount = 3;

        #endregion
    }
}
