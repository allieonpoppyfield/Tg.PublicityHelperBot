using Telegram.Bot;

namespace Tg.PublicityHelperBot.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
