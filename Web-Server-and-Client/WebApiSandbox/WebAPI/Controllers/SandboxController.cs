using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SandboxController : ControllerBase
    {

        public SandboxController()
        {

        }

        [HttpGet]
        public async Task<FullResponse> Get()
        {
            var obj = new FullResponse();
            return await Task.FromResult<FullResponse>(obj);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> Post([FromBody] Payload payload)
        {
            if (payload == null) return BadRequest();
            //await Task.Delay(200);
            var obj = new Response();
            return new OkObjectResult(obj);
        }
    }
}
