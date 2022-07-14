using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinChatter.API.Controllers
{
    [Route("api/chat")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {

        [HttpGet("GetGroups")]
        public async Task<IActionResult> GetGroups()
        {
            return Ok();
        }

    }
}
