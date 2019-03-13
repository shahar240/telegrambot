using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using telegramBot_Common;

namespace telegramBot_BL.Managers
{
    public class TelegramManager
    {
        private readonly TelegramBotClient bot;

        public TelegramManager()
        {
            string botToken = CredentialsManager.Credentials.BotToken;
            bot = new TelegramBotClient(botToken);
        }

    }
}
