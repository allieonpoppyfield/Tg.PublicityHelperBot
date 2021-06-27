using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;


        public UpdateService(IBotService botService, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        public async Task<bool> IsAdmin(long chatId)
        {
            try
            {
                var member =  await _botService.Client.GetChatMemberAsync(chatId, _botService.Client.BotId.Value);
                if (member != null)
                {
                    if (member.Status == ChatMemberStatus.Administrator)
                        return true;
                    return false;
                }
                else return false;
            }
            catch
            {
                return false;
            }

        }

        public async Task SendTextMessageAssync(long chatId, string text, IReplyMarkup replyMarkup)
        {
            await _botService.Client.SendTextMessageAsync(chatId: chatId, text: text, replyMarkup: replyMarkup);
        }
    }
}
