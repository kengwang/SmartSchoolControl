using System.Threading;
using System.Threading.Tasks;

namespace SchoolComputerControl.Client.Interfaces;

public interface INotificationReceiver<TNotification>
{
    public Task HandleNotification(CancellationToken cancellationToken = default);
}