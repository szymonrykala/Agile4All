using AgileApp.Utils;
using System.Text.RegularExpressions;

namespace AgileApp.Models.Requests
{
    public class BaseDataRequest
    {
        public string Email { get; set; }

        public virtual bool IsValid =>
            !string.IsNullOrWhiteSpace(Email) && Regex.IsMatch(Email, AppSettings.EmailExpression);
    }
}
