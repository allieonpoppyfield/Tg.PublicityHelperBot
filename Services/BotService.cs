using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Tg.PublicityHelperBot.Infrastructure;

namespace Tg.PublicityHelperBot.Services
{
    public class BotService : IBotService
    {
        private readonly BotConfiguration _config;

        public BotService(IOptions<BotConfiguration> config)
        {
            _config = config.Value;
            Client = new TelegramBotClient(_config.BotToken);
            //Client.SetWebhookAsync("https://tgpublicityhelperbot.azurewebsites.net/api/BotPublicityInvokerRun");
            Client.SetWebhookAsync("https://f06561a27e2d.ngrok.io/api/BotPublicityInvokerRun");
        }
        public TelegramBotClient Client { get; }
    }
}
