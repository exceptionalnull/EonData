using Amazon.DynamoDBv2.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EonData.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class XyzzyController : ControllerBase
    {
        private readonly IHttpClientFactory httpFactory;
        private readonly ILogger log;

        public XyzzyController(IHttpClientFactory http, ILogger<XyzzyController> logger)
        {
            httpFactory = http;
            log = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetVersion() => Ok(System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "Unknown");
    }
}
