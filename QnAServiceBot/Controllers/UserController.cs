using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QnAServiceBot.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using QnAServiceBot.Services;

namespace QnAServiceBot.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // api/user/login
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {
            var user = _userService.UserList.SingleOrDefault(p =>
                string.Equals(p.Username, login.Username, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                return Unauthorized();
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            Response.Cookies.Append("username", user.Username, new CookieOptions { Expires = FetchCookieExpiration() });

            return new JsonResult(FetchUserInfo(user.Username));
        }

        // api/user/logout
        [Authorize]
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("username");
            return Ok();
        }

        //api/user/info
        [Authorize]
        [HttpGet]
        [Route("info")]
        public IActionResult GetUserInfo() => new JsonResult(FetchUserInfo());

        private UserModel FetchUserInfo(string username = null)
        {
            return _userService.UserList.SingleOrDefault(p => string.Equals(
                p.Username,
                username ?? Request.Cookies["username"],
                StringComparison.OrdinalIgnoreCase)
            );
        }

        private DateTimeOffset FetchCookieExpiration()
        {
            var expiresMinutes = double.Parse(_configuration["Cookie:Expiration"]);
            return DateTime.Now.AddMinutes(expiresMinutes);
        }
    }
}
