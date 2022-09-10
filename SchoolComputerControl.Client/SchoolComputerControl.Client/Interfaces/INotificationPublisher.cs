using System.Threading;
using System.Threading.Tasks;

namespace SchoolComputerControl.Client.Interfaces;

public interface INotificationPublisher
{
    Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default);
}