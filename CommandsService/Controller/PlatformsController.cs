using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controlles
{
  [Route("api/commands/[controller]")]
  [ApiController]
  public class PlatformsController : ControllerBase
  {
    public PlatformsController()
    {

    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
      Console.WriteLine("--> Inbound POST # Command Service");

      return Ok("Inbound test ok from Platforms Controller");
    }
  }
}