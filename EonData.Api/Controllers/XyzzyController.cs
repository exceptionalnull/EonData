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
        private const string authep = "https://eonid.b2clogin.com/eonid.onmicrosoft.com/B2C_1A_SIGNUP_SIGNIN/v2.0/.well-known/openid-configuration";
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

        [HttpGet]
        [Route("gruffalo")]
        public async Task<IActionResult> Gruffalo(CancellationToken cancellationToken)
        {
            string result;
            try
            {
                result = (await callHttp("https://www.example.com/", cancellationToken)) ? "purple prickles" : "knobbly knees";
            }
            catch (Exception ex)
            {
                log.LogError(ex, "failed example dot com");
                result = ex.Message;
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("grue")]
        public async Task<IActionResult> Grue(CancellationToken cancellationToken)
        {
            string result;
            try
            {
                result = (await callHttp(authep, cancellationToken)) ? "There is a small mailbox here." : "It is pitch black.";
            }
            catch (Exception ex)
            {
                log.LogError(ex, "failed auth endpoint");
                result = ex.Message;
            }

            return Ok(result);
        }

        private async Task<bool> callHttp(string url, CancellationToken cancellationToken)
        {
            log.LogDebug($"calling http url: {url}");

            var http = httpFactory.CreateClient();
            log.LogDebug("created http factory...");

            var result = await http.GetAsync(url, cancellationToken);
            log.LogDebug($"http call result: {result}");

            return result.IsSuccessStatusCode;
        }
    }
}
