using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataService.Http
{
  public class HttpCommandDataClient : ICommandDataClient
  {
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
      _httpClient = httpClient;
      _configuration = configuration;
    }

    public async Task SendPlatformCommand(PlatformReadDto plat)
    {
      var httpContent = new StringContent(
        JsonSerializer.Serialize(plat),
        Encoding.UTF8,
        "application/json"
      );

      var response = await _httpClient.PostAsync($"{_configuration["CommandServiceUrl"]}/api/commands/platforms", httpContent);

      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine("--> Sync POST to CommandService was OK!");
      }
      else
      {
        Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
      }
    }
  }
}