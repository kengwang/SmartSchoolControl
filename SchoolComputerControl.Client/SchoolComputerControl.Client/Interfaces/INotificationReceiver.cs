using System.Threading;
using System.Threading.Tasks;

namespace SchoolComputerControl.Client.Interfaces;

public interface INotificationReceiver<in TNotification>
{
    public Task HandleNotificationAsync(TNotification notification,CancellationToken cancellationToken = default);
}