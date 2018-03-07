using System.Collections.Generic;
using QnAServiceBot.Models;

namespace QnAServiceBot.Services
{
    public class UserService
    {
        public readonly IEnumerable<UserModel> UserList = new List<UserModel>()
        {
            new UserModel(){ Id="1", Username = "Poy", Image = "./assets/images/man.svg", Role = "user" },
            new UserModel(){ Id="2", Username = "Mary", Image = "./assets/images/woman.svg", Role = "user" },
            new UserModel(){ Id="3", Username = "IT", Image = "./assets/images/it-guy.svg", Role = "admin" },
            new UserModel(){ Id="4", Username = "Bot", Image = "./assets/images/bot.svg", Role = "bot" },
        };
    }
}
