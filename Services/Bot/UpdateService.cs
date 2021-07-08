using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Models.Database;
using Tg.PublicityHelperBot.Static;

namespace Tg.PublicityHelperBot.Services.Bot
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly TgDatabaseContext _context;

        public UpdateService(IBotService botService, TgDatabaseContext ctx)
        {
            _botService = botService;
            _context = ctx;
        }

        public async Task HandleAccountMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);

            await SetUserState(chatId, CallBackActionNames.Account, user);

            //await _botService.Client.DeleteMessageAsync(chatId, update.Message.MessageId);
            string txt = $"Ваш ID: {user.ChatID}{Environment.NewLine}" +
                $"Вы с нами с {user.RegisterDate}.{Environment.NewLine}" +
                $"Для управления каналом, нажмите на его название.{Environment.NewLine}" +
                $"Либо добавьте новый канал.";
            await _botService.Client.SendTextMessageAsync(chatId, txt, replyMarkup: KeyboardMarkups.Account);
        }


        public async Task HandleAddChannelAction(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            await SetUserState(chatId, CallBackActionNames.AddChannel);
            await _botService.Client.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, "Добавьте нашего бота (@publicity_helper_bot) в список администраторов" +
                " добавляемого канала и перешлите сюда любое сообщение с этого канала.", replyMarkup: KeyboardMarkups.BackButton(CallBackActionNames.MainMenu));
        }


        public async Task HandleMainMenulAction(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            await SetUserState(chatId, CallBackActionNames.MainMenu);
            await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Выберите требуемое действие", replyMarkup: KeyboardMarkups.MainMenu);
        }

        public async Task HandleMainMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            await SetUserState(chatId, CallBackActionNames.MainMenu);
            await _botService.Client.SendTextMessageAsync(chatId, "Выберите требуемое действие", replyMarkup: KeyboardMarkups.MainMenu);
        }

        public async Task HandleStartMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == update.Message.Chat.Id);
            if (user == null)
            {
                user = new()
                {
                    ChatID = update.Message.Chat.Id,
                    RegisterDate = DateTime.Now,
                };
                await _context.TgUsers.AddAsync(user);
            }
            await SetUserState(chatId, null, user);


            //TODO: написать нормальный текст
            string text = "Привет привет привет";
            await _botService.Client.SendTextMessageAsync(chatId, text, replyMarkup: KeyboardMarkups.Default);
        }

        private async Task SetUserState(long chatId, string stateName, TgUser user = null)
        {
            if (user == null)
                user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);
            user.CurrentAction = stateName;
            await _context.SaveChangesAsync();
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

        public async Task HandleForwardedMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);

            if (user.CurrentAction != CallBackActionNames.AddChannel) return;

            var task = _botService.Client.GetChatMemberAsync(update.Message.ForwardFromChat.Id, _botService.Client.BotId.Value);
            task.Wait();

            if (task.Result == null || task.Result.Status != ChatMemberStatus.Administrator)
            {
                await _botService.Client.SendTextMessageAsync(chatId, $"Бот @publicity_helper_bot не является админисратором указанного канала.");
                return;
            }

            if (_context.Channels.FirstOrDefault(x => x.ChannelId == update.Message.ForwardFromChat.Id && x.OwnerId == user.ID) != null)
            {
                await _botService.Client.SendTextMessageAsync(chatId, $"Этот канал уже добавлен.");
                return;
            }

            Channel channel = new()
            {
                ChannelId = update.Message.ForwardFromChat.Id,
                Title = update.Message.ForwardFromChat.Title,
                Owner = user,
                SubscriptionEndDate = DateTime.Now.AddDays(7) //TODO: понятно да
            };
            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            await _botService.Client.SendTextMessageAsync(chatId, $"Канал успешно добавлен.");
        }
    }
}
