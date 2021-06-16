using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Tg.PublicityHelperBot.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
        Task SendMessage(string message, long chatId);
    }
}
