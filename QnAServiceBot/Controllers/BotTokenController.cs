using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QnAServiceBot.Models;
using QnAServiceBot.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QnAServiceBot.Controllers
{
    [Route("api/[controller]")]
    public class BotTokenController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public BotTokenController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // api/BotToken/generate/poy
        [HttpPost]
        [Route("generate/{username}")]
        public async Task<IActionResult> GenerateToken(string username)
        {
            if (_userService.UserList.Count(p => string.Equals(p.Username, username, StringComparison.CurrentCultureIgnoreCase)) != 0)
            {
                return new JsonResult(await Generate());
            }
            return new EmptyResult();
        }

        // api/BotToken/generate
        /// <summary>取得使用 Direct Line 的 Token</summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateToken()
        {
            return new JsonResult(await Generate());
        }

        private async Task<BotTokenModel> Generate()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://webchat.botframework.com/api/conversations");
                client.DefaultRequestHeaders.Add("Authorization", $"BotConnector {_configuration["BotFramework:Secret"]}");

                var response = await client.PostAsync("", null);
                var data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<BotTokenModel>(data);
            }
        }
    }
}
