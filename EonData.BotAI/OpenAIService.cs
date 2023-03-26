using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;
using EonData.OpenAI;

namespace EonData.BotAI
{
    public class OpenAIService
    {
        private OpenAIClient aiClient;
        public OpenAIService(IOptions<OpenAIOptions> options, IHttpClientFactory httpFactory)
        {
            aiClient = new OpenAIClient(httpFactory.CreateClient(), options.Value.ApiKey);
            if (!string.IsNullOrEmpty(options.Value.OrgKey))
            {
                aiClient.SetOrgKey(options.Value.OrgKey);
            }
        }

        public IAsyncEnumerable<string>
    }
}
