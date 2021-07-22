using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Services.Bot
{
    public interface IUpdateService
    {
        Task HandleStartMessage(Update update);
        Task HandleMainMenuMessage(Update update);
        Task HandleAccountMenuMessage(Update update);
        Task HandleAddChannelAction(Update update);
        Task HandleMainMenulAction(Update update);
        Task HandleForwardedMessage(Update update);
        Task HandleChannelInfoAction(Update update);
        Task HandleCreatePostAction(Update update);
        Task HandleTextMessageAction(Update update);
        Task HandlePublishAction(Update update);
        Task HandleDeleteChannelAction(Update update);
        Task HandleYesDeleteChannelAction(Update update);
        Task HandleAccountMenuAction(Update update);
    }
}