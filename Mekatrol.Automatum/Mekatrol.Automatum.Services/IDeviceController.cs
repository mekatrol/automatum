using Mekatrol.Automatum.Entities;

namespace Mekatrol.Automatum.Services;

public interface IDeviceController : IDisposable
{
    Task<dynamic> GetDeviceInfo(ICommunicationChannel comm, CancellationToken cancellationToken);

    Task UpdatePoints(ICommunicationChannel comm, IList<Point> points, CancellationToken cancellationToken);
}
