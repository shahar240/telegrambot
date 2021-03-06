﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace telegramBot.Controllers
{
    public class WebHookController : ApiController
    {

        public async Task<IHttpActionResult> Post(Update update)
        {
            var message = update.Message;
            
            Console.WriteLine("Received Message from {0}", message.Chat.Id);

            if (message.Type == MessageType.Text)
            {
                // Echo each Message
                //await Bot.Api.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
            else if (message.Type == MessageType.Photo)
            {
                // Download Photo
                //var file = await Bot.Api.GetFileAsync(message.Photo.LastOrDefault()?.FileId);

                //var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                //using (var saveImageStream = File.Open(filename, FileMode.Create))
                //{
                   // await Bot.Api.DownloadFileAsync(file.FilePath, saveImageStream);
                //}

                //await Bot.Api.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
            }

            return Ok();
        }

    }
}
