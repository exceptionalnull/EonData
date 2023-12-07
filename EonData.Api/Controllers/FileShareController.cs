using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public Task<IActionResult> GetFileShareDetails()
        {

        }
    }
}
