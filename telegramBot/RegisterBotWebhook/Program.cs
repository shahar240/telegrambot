using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.InputFiles;
using telegramBot_BL.Helpers;

namespace RegisterBotWebhook
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("-register"))
            {
                string url = ConfigurationManager.AppSettings["url"];
                if (Boolean.Parse(ConfigurationManager.AppSettings["UseUntrustedCa"]))
                {
                    string certPath = ConfigurationManager.AppSettings["certificatePath"];
                    using (var file = File.Open(certPath, FileMode.Open))
                    {
                        InputFileStream cert = new InputFileStream(file);
                        TelegramBot.Api.SetWebhookAsync("url", cert).Wait();
                    }
                }
                else
                    TelegramBot.Api.SetWebhookAsync("url").Wait();
                Console.WriteLine("webhook has been registered");
            }
            else if (args.Contains("-unregister"))
            {
                TelegramBot.Api.DeleteWebhookAsync().Wait();
                Console.WriteLine("webhook has been unregistered");
            }
            else
            {
                Console.WriteLine("invalid call. usage:\r\n " +
                    "register: RegisterBotWebhook -regiester " +
                    "unregister: RegisterBotWebhook -unregiester");
            }
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
