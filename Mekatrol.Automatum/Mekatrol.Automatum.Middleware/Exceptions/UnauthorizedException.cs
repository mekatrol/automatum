using System.Net;

namespace Mekatrol.Automatum.Middleware.Exceptions;

public class UnauthorizedException : ServiceException
{
    public UnauthorizedException() : base(HttpStatusCode.Unauthorized)
    {
    }
}