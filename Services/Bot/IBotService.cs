using Telegram.Bot;

namespace Tg.PublicityHelperBot.Services.Bot
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
