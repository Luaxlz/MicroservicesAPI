using PlatformService.Dtos;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient : IAsyncDisposable
    {
        Task ConnectAsync();
        Task PublishNewPlatformAsync(PlatformPublishedDto platformPublishedDto);
    }
}