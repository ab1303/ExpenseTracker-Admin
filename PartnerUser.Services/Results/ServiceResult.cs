using System.Net;

namespace PartnerUser.Services.Results
{
    public enum ServiceResultStatus
    {
        Success = 0,
        Failure = 1
    }

    public class ServiceResult
    {
        public ServiceResultStatus Status { get; set; }

        public bool IsSuccess => Status == ServiceResultStatus.Success && Error == null;

        public Error Error { get; set; }

    }

    public class HttpServiceResult : ServiceResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
    }


    public class HttpServiceResult<T> : HttpServiceResult
    {
        public T Model { get; set; }
    }
}
