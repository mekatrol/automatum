using Mekatrol.Automatum.Models;
using System.Net;

namespace Mekatrol.Automatum.Middleware.Exceptions;

public class ServiceException(HttpStatusCode statusCode, IList<ServiceError> errors) : Exception
{
    public ServiceException(HttpStatusCode statusCode)
        : this(statusCode, [])
    {

    }

    public ServiceException(HttpStatusCode statusCode, string error)
        : this(statusCode, new List<ServiceError>([new ServiceError(error)]))
    {

    }

    public IList<ServiceError> Errors { get; set; } = errors;

    public HttpStatusCode StatusCode { get; set; } = statusCode;
}