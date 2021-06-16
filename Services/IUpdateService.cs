﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
        Task SendTextMessageAssync(long chatId, string text, IReplyMarkup replyMarkup);
    }
}