using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                if (message.Text[0] == '/')
                {
                    string[] cmd = message.Text.Split(' ');
                    switch (cmd[0].ToLower())
                    {
                        case "/enablegithub":
                            await EnableGithub(message, cmd);
                            break;
                        case "/disablegithub":
                            await DisableGithub(message, cmd);
                            break;
                        case "/setfrequency":
                            await SetFrequency(message, cmd);
                            break;
                        case "/help":
                            await Help(message, cmd);
                            break;
                        case "/settings":
                            await Settings(message, cmd);
                            break;
                    }
                }
                // Echo each Message
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
            return Ok();
        }

        private async Task Settings(Message message, string[] cmd)
        {
            string settings = await _manager.GetUserSettingsString(message.From.Id);
            await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, settings);
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
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "registration failed. please try agian later");
            }
        }

        private async Task SetFrequency(Message message, string[] cmd)
        {
            if (cmd.Length != 3)
            {
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "invalid command");
                return;
            }
            int num, multiplier;
            if (!int.TryParse(cmd[1], out num))
            {
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "invalid number");
                return;
            }
            switch (cmd[2].ToLower())
            {
                case "minutes":
                    multiplier = 1;
                    break;
                case "hours":
                    multiplier = 60;
                    break;
                case "days":
                    multiplier = 1440;
                    break;
                default:
                    await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "invalid interval value");
                    return;
            }
            int freq = multiplier * num;
            if (await _manager.SetUpdateFrequency(message.From.Id, freq))
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "update frequency has been updated");
            else
                await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, "update failed. please try agian later");
        }

        private async Task Help(Message message, string[] cmd)
        {
            StringBuilder helpString = new StringBuilder();
            helpString.AppendLine("this bot will send you notifications about things that might intrest you from registered services.");
            helpString.AppendLine("avialable commands:");
            helpString.AppendLine("\t /help - shows this help");
            helpString.AppendLine("\t /settings - show your current update frequency and registered services");
            helpString.AppendLine("\t /setFrequency <amount(number)> <interval(minutes/hours/days)> - chage update frequency to given frequency");
            helpString.AppendLine("\t /enablegithub <github user token> - register to github update. see https://help.github.com/en/articles/creating-a-personal-access-token-for-the-command-line to learn how to get token.");
            helpString.AppendLine("\t /disablegithub - unregister from github notifications");

            await TelegramBot.Api.SendTextMessageAsync(message.Chat.Id, helpString.ToString());
        }
    }
}

