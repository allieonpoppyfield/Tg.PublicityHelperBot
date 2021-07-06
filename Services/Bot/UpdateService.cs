using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Static;

namespace Tg.PublicityHelperBot.Services.Bot
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

        public async Task HandleAccountMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            await _botService.Client.DeleteMessageAsync(chatId, update.Message.MessageId);
            string txt = $"Ваш ID: 1111111111111\n" +
                $"Вы с нами с {DateTime.Now.ToShortDateString()}.{Environment.NewLine}" +
                $"Для управления каналом, нажмите на его название.{Environment.NewLine}" +
                $"Либо добавьте новый канал.";
            await _botService.Client.SendTextMessageAsync(chatId, txt, replyMarkup: KeyboardMarkups.Account);
        }

        public async Task HandleAddChannelAction(Update update)
        {
            await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, "Для того чтобы добавить канал перешлите сюда блаблабла", replyMarkup: KeyboardMarkups.BackButton(CallBackActionNames.MainMenu));
        }

        public async Task HandleMainMenulAction(Update update)
        {
            await _botService.Client.DeleteMessageAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId);
            await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Выберите требуемое действие", replyMarkup: KeyboardMarkups.MainMenu);
        }

        public async Task HandleMainMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;

            await _botService.Client.DeleteMessageAsync(chatId, update.Message.MessageId);
            await _botService.Client.SendTextMessageAsync(chatId, "Выберите требуемое действие", replyMarkup: KeyboardMarkups.MainMenu);

        }

        public async Task HandleStartMessage(Update update)
        {
            await _botService.Client.DeleteMessageAsync(update.Message.Chat.Id, update.Message.MessageId);

            //TODO: написать нормальный текст
            string text = "Привет привет привет";
            var replyKeyboardMarkup =
            await _botService.Client.SendTextMessageAsync(update.Message.Chat.Id, text, replyMarkup: KeyboardMarkups.Default);
        }




        public async Task<bool> IsAdmin(long chatId)
        {
            try
            {
                var member = await _botService.Client.GetChatMemberAsync(chatId, _botService.Client.BotId.Value);
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
