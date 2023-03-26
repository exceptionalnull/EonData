using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EonData.BotAI;

namespace EonData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotsController : ControllerBase
    {
        private readonly OpenAIService aiService;

        public BotsController(OpenAIService openai) => aiService = openai;

        [HttpGet]
        [Route("/models")]
        public async Task<IActionResult> ListModelNames()
        {
            return Ok();
        }
    }
}
