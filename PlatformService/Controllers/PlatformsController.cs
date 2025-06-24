using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformsController(
      IPlatformRepo repository,
      IMapper mapper,
      ICommandDataClient commandDataClient)
    {
      _repository = repository;
      _mapper = mapper;
      _commandDataClient = commandDataClient;
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

      try
      {
        await _commandDataClient.SendPlatformCommand(platformreadDto);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Could not send synchronously: {ex.Message}");
      }

      return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformreadDto.Id }, platformreadDto);
    }
  }
}