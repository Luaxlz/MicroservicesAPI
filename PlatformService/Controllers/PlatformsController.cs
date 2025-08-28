using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.ASyncDataServicess;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(
  IPlatformRepo repository,
  IMapper mapper,
  ICommandDataClient commandDataClient,
  IMessageBusClient messageBusClient
  )
    {
      _repository = repository;
      _mapper = mapper;
      _commandDataClient = commandDataClient;
      _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
      Console.WriteLine("--> Getting Platforms...");

      var platformItems = _repository.GetAllPlatforms();

      return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
      Console.WriteLine($"--> Getting Platform by Id: {id}...");
      var platformItem = _repository.GetPlatformById(id);
      if (platformItem != null)
      {
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
      }

      return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> Createplatform(PlatformCreateDto platform)
    {
      Console.WriteLine($"--> Creating Platform: {platform.Name}...");
      var platformModel = _mapper.Map<Platform>(platform);
      _repository.CreatePlatform(platformModel);
      _repository.SaveChanges();

      var platformreadDto = _mapper.Map<PlatformReadDto>(platformModel);

      //Send Sync message
      try
      {
        await _commandDataClient.SendPlatformCommand(platformreadDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Could not send synchronously: {ex.Message}");
      }

      //Send Async message
      try
      {
        var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformreadDto);
        platformPublishedDto.Event = "Platform_Published";
        await _messageBusClient.PublishNewPlatformAsync(platformPublishedDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
        throw;
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformreadDto.Id }, platformreadDto);
    }
  }
}