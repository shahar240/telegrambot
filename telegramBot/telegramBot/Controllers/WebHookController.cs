using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using telegramBot_BL.Helpers;
using telegramBot_BL.Managers;

namespace telegramBot.Controllers
{
    public class WebHookController : ApiController
    {
        private UsersManager _manager;

        public WebHookController()
        {
            _manager = new UsersManager();
        }

        public async Task<IHttpActionResult> Post(Update update)
        {
            var message = update.Message;
            if (message.Type == MessageType.Text)
            {
                if(message.Text[0]=='/')
                {
                    string[] cmd = message.Text.Split(' ');
                    switch(cmd[0])
                    {
                        case "/EnableGithub":
                            await EnableGithub(message, cmd);
                            break;
                        case "/DisableGithub":
                            await DisableGithub(message,cmd);
                            break;
                    }
                }
                // Echo each Message
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
            return Ok();
        }

        private async Task EnableGithub(Message message, string[] cmd)
        {
            if (cmd.Length != 2)
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "invalid command");
            else
            {
                if (await _manager.EnableGitHub(message.From.Id, cmd[1]))
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "you will now recive github updates");
                else
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "registration failed. please try agia later");
            }
        }

        private async Task DisableGithub(Message message, string[] cmd)
        {
            if (cmd.Length != 1)
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "invalid command");
            else
            {
                if (await _manager.EnableGitHub(message.From.Id, cmd[1]))
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "you will now recive github updates");
                else
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "registration failed. please try agia later");
            }
        }
    }
}
