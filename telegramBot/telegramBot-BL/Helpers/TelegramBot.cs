using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using telegramBot_Common;

namespace telegramBot_BL.Helpers
{
    public class TelegramBot
    {
        public static readonly TelegramBotClient Api;

        static TelegramBot()
        {
            string apiKey =CredentialsManager.Credentials.BotToken;
            Api =  new TelegramBotClient(apiKey);
        }
    }
}
