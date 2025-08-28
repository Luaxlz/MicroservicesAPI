using PlatformService.Dtos;
using System.Threading.Tasks;

namespace PlatformService.ASyncDataServicess
{
    public interface IMessageBusClient : IAsyncDisposable
    {
        Task ConnectAsync();
        Task PublishNewPlatformAsync(PlatformPublishedDto platformPublishedDto);
    }
}