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
        Task InvokeCallBackMethod(Update update);

        Task HandleStartMessage(Update update);
        Task HandleMainMenuMessage(Update update);
        Task HandleAccountMenuMessage(Update update);
        Task HandleForwardedMessage(Update update);
        Task HandleUserTextMessage(Update update);


        Task ErrorMessage(Update update,string er);
        //Task HandleAddChannelAction(Update update);
        //Task HandleMainMenulAction(Update update);
        //Task HandleChannelInfoAction(Update update);
        //Task HandleCreatePostAction(Update update);
        //Task HandlePublishAction(Update update);
        //Task HandleDeleteChannelAction(Update update);
        //Task HandleYesDeleteChannelAction(Update update);
        //Task HandleAccountMenuAction(Update update);
        //Task HandleEditPostAction(Update update);
        //Task HandleEditMessageAction(Update update);
        //Task HandleEditMessageTextAction(Update update);
    }
}