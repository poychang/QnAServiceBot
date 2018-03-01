using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace QnAServiceBot.Controllers
{
    [Route("api/[controller]")]
    public class BotTokenController : Controller
    {
        public BotTokenController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // api/BotToken/generate
        /// <summary>取得使用 Direct Line 的 Token</summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://webchat.botframework.com/api/conversations");
                client.DefaultRequestHeaders.Add("Authorization", $"BotConnector {Configuration["BotFramework:Secret"]}");

                var response = await client.PostAsync("", null);
                var data = await response.Content.ReadAsStringAsync();

                return new JsonResult(JsonConvert.DeserializeObject<JObject>(data));
            }
        }
    }
}
