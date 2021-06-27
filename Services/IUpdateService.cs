using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Services
{
    public interface IUpdateService
    {
        Task SendTextMessageAssync(long chatId, string text, IReplyMarkup replyMarkup = null);
        Task<bool> IsAdmin(long chatId);
    }
}