using System.Net;

namespace Mekatrol.Automatum.Middleware.Exceptions;

public class NotFoundException : ServiceException
{
    public NotFoundException() : base(HttpStatusCode.NotFound)
    {
    }

    public NotFoundException(string error) : base(HttpStatusCode.NotFound, error)
    {
    }
}
