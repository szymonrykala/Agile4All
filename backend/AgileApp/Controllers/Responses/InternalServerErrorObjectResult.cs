using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers.Responses
{
    /// <summary>
    /// An <see cref="ObjectResult"/> that when executed will produce an Internal Server Error (500) response.
    /// </summary>
    [DefaultStatusCode(DefaultStatusCode)]
    public class InternalServerErrorObjectResult : ObjectResult
    {
        private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;

        /// <summary>
        /// Creates a new <see cref="InternalServerErrorObjectResult"/> instance.
        /// </summary>
        /// <param name="error">Contains the errors to be returned to the client.</param>
        public InternalServerErrorObjectResult([ActionResultObjectValue] object? error)
            : base(error)
        {
            StatusCode = DefaultStatusCode;
        }

        /// <summary>
        /// Creates a new <see cref="InternalServerErrorObjectResult"/> instance.
        /// </summary>
        /// <param name="modelState"><see cref="ModelStateDictionary"/> containing the validation errors.</param>
        public InternalServerErrorObjectResult([ActionResultObjectValue] ModelStateDictionary modelState)
            : base(new SerializableError(modelState))
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            StatusCode = DefaultStatusCode;
        }
    }
}
