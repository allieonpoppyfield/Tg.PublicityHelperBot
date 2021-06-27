using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot.Models.UserStates
{
    public abstract class ChatState
    {
        protected readonly IUpdateService updateService;

        protected LocalChat localChatItem;

        public ChatState(IUpdateService _updateService, LocalChat _localChatItem)
        {
            updateService = _updateService;
            localChatItem = _localChatItem;
        }

        protected abstract IReplyMarkup ReplyMarkup();
        public abstract Task ProcessUpdate(Update update);
        public abstract Task StateSelected(long ChatId);
    }
}
