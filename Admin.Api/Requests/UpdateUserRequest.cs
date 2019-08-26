using System.Net;
using Admin.Services.Results;

namespace Admin.Api.Requests
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }

        public HttpServiceResult ValidateAllEditablePropertiesSet()
        {
            var serviceResult = new HttpServiceResult{ Status = ServiceResultStatus.Success };

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                serviceResult.HttpStatusCode = HttpStatusCode.BadRequest;
                serviceResult.Status = ServiceResultStatus.Failure;
                serviceResult.Error = new Error
                {
                    ErrorCode = ErrorCodes.RequestPropertyNotSet,
                    SystemMessage = $"The {nameof(FirstName)} field must have a value."
                };
            }
            return serviceResult;
        }
    }
}
