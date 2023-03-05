using AgileApp.Utils;

namespace AgileApp.Models.Requests
{
    public class TokenRequest : BaseDataRequest
    {
        public string Token { get; set; }

        public override bool IsValid =>
            base.IsValid && !string.IsNullOrWhiteSpace(Token) && Token.Length == AppSettings.TokenLength;
    }
}
