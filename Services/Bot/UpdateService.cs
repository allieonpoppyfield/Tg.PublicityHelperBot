using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private const long PROXY_CHAT_ID = -1001556709432;
        private readonly IBotService _botService;
        private readonly TgDatabaseContext _context;

        public UpdateService(IBotService botService, TgDatabaseContext ctx)
        {
            _botService = botService;
            _context = ctx;
        }


        private static Dictionary<string, string> Methods = new Dictionary<string, string>()
        {
            { CallBackActionNames.MainMenu, nameof(HandleMainMenulAction) },
            { CallBackActionNames.Account, nameof(HandleAccountMenuAction) },
            { CallBackActionNames.AddChannel, nameof(HandleAddChannelAction) },
            { CallBackActionNames.ChannelInfo, nameof(HandleChannelInfoAction) },
            { CallBackActionNames.CreatePost, nameof(HandleCreatePostAction) },
            { CallBackActionNames.EditPostForChannel, nameof(HandleEditPostAction) },
            { CallBackActionNames.Publish, nameof(HandlePublishAction) },
            { CallBackActionNames.DeleteChannel, nameof(HandleDeleteChannelAction) },
            { CallBackActionNames.YesDelete, nameof(HandleYesDeleteChannelAction) },
            { CallBackActionNames.EditMessage, nameof(HandleEditMessageAction) },
            { CallBackActionNames.EditMessageText, nameof(HandleEditMessageTextAction) },
            { CallBackActionNames.EditedPublish, nameof(HandlePublishEditedMessageAction) },
        };



        public async Task InvokeCallBackMethod(Update update)
        {
            string methodName = Methods.FirstOrDefault(x => update.CallbackQuery.Data.StartsWith(x.Key)).Value;
            Type th = this.GetType();
            MethodInfo method = th.GetMethod(methodName, (BindingFlags)60);

            object[] par = new object[] { update };
            await (Task)method.Invoke(this, par);
        }

        private async Task SetUserState(long chatId, string stateName, TgUser user = null)
        {
            if (user == null)
                user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);
            user.CurrentAction = stateName;
            await _context.SaveChangesAsync();
        }


        #region PublicApiMethods
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
                    IsNewUser = true
                };
                await _context.TgUsers.AddAsync(user);
            }
            await SetUserState(chatId, null, user);


            //TODO: написать нормальный текст
            string text = "Привет привет привет";
            await _botService.Client.SendTextMessageAsync(chatId, text, replyMarkup: KeyboardMarkups.Default);
        }

        public async Task HandleMainMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            await SetUserState(chatId, CallBackActionNames.MainMenu);
            await _botService.Client.SendTextMessageAsync(chatId, "Выберите требуемое действие", replyMarkup: KeyboardMarkups.MainMenu);
        }

        public async Task HandleAccountMenuMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);

            await SetUserState(chatId, CallBackActionNames.Account, user);

            //await _botService.Client.DeleteMessageAsync(chatId, update.Message.MessageId);
            string txt = $"Ваш ID: {user.ChatID}{Environment.NewLine}" +
                $"Вы с нами с {user.RegisterDate}.{Environment.NewLine}" +
                $"Список ваших каналов приведен ниже.{Environment.NewLine}" +
                $"Для управления каналом, нажмите на его название.{Environment.NewLine}" +
                $"Либо добавьте новый канал.";

            var channelList = _context.Channels.Where(x => x.OwnerId == user.ID).ToListAsync();
            channelList.Wait();
            await _botService.Client.SendTextMessageAsync(chatId, txt, replyMarkup: KeyboardMarkups.Account(channelList.Result));
        }
        public async Task HandleForwardedMessage(Update update)
        {
            var chatId = update.Message.Chat.Id;
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);

            if (user.CurrentAction != CallBackActionNames.AddChannel) return;


            ChatMember member = null;
            try
            {
                member = await _botService.Client.GetChatMemberAsync(update.Message.ForwardFromChat.Id, _botService.Client.BotId.Value);
            }
            catch
            {
                await _botService.Client.SendTextMessageAsync(chatId, $"Бот @publicity_helper_bot не является участником указанного канала.");
                return;
            }

            if (member.Status != ChatMemberStatus.Administrator)
            {
                await _botService.Client.SendTextMessageAsync(chatId, $"Бот @publicity_helper_bot не является администратором указанного канала.");
                return;
            }

            if (_context.Channels.FirstOrDefault(x => x.ChatId == update.Message.ForwardFromChat.Id && x.OwnerId == user.ID) != null)
            {
                await _botService.Client.SendTextMessageAsync(chatId, $"Этот канал уже добавлен.");
                return;
            }

            Channel channel = new()
            {
                ChatId = update.Message.ForwardFromChat.Id,
                Title = update.Message.ForwardFromChat.Title,
                Owner = user,
                SubscriptionEndDate = user.IsNewUser ? DateTime.Now.AddDays(7) : null
            };
            _context.Channels.Add(channel);
            user.IsNewUser = false;

            await _context.SaveChangesAsync();

            await _botService.Client.SendTextMessageAsync(chatId, $"Канал успешно добавлен.");
        }

        public async Task HandleUserTextMessage(Update update)
        {
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == update.Message.Chat.Id);
            if (user == null) return;

            if (user.CurrentAction.StartsWith(CallBackActionNames.CreatePost + "|"))
            {
                string resultText = string.Empty;
                resultText += update.Message.Text;
                await _botService.Client.SendTextMessageAsync(update.Message.Chat.Id, resultText, replyMarkup: (InlineKeyboardMarkup)KeyboardMarkups.CreatingPost());
            }

            else if (user.CurrentAction.StartsWith(CallBackActionNames.EditMessageText))
            {
                var callBackData = user.CurrentAction;
                var chatId = long.Parse(callBackData[(callBackData.IndexOf("|") + 1)..(callBackData.LastIndexOf("|") - 1)]);
                var messageId = int.Parse(callBackData[(callBackData.LastIndexOf("|") + 1)..]);


                UserMessage userMessage = await _context.UserMessages.Include(x => x.Channel).FirstOrDefaultAsync(x => x.MessageId == messageId);
                if (userMessage == null)
                    return;
                try
                {
                    //копируем сообщение. чтобы хоть как то его найти - посылаем копию в наш прокси канал //publike - хз как это еще сделать
                    var msgProxy = await _botService.Client.ForwardMessageAsync(PROXY_CHAT_ID
                        , userMessage.Channel.ChatId, (int)userMessage.MessageId);

                    var msg = await _botService.Client.SendTextMessageAsync(PROXY_CHAT_ID
                        , msgProxy.Text, replyMarkup: msgProxy.ReplyMarkup);

                    msg = await _botService.Client.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, update.Message.Text, replyMarkup: msg.ReplyMarkup);

                    //показываем сообщение пользователю
                    await _botService.Client.SendTextMessageAsync(update.Message.Chat.Id, msg.Text,
                        entities: msg.Entities, replyMarkup: msg.ReplyMarkup);

                    var replymsg = await _botService.Client.SendTextMessageAsync(update.Message.Chat.Id, "Текст изменен.",
                        replyMarkup: KeyboardMarkups.WhatCanIdoEdited(chatId, messageId, (int)msg.Chat.Id, msg.MessageId));
                }
                catch (Exception ex)
                {
                    _ = ex;
                }
            }
        }
        #endregion


        #region PrivateApiMethods

        private async Task HandlePublishEditedMessageAction(Update update)
        {
            var user = await _context.TgUsers.FirstOrDefaultAsync(x => x.ChatID == update.CallbackQuery.Message.Chat.Id);
            var state = user.CurrentAction = update.CallbackQuery.Data;
            var ids = state.Split("|").ToList().GetRange(1, 4);
            if (ids.Count != 4)
                throw new Exception("Не четыре параметра, ошибка");
            var msg = await _botService.Client.ForwardMessageAsync(PROXY_CHAT_ID, ids[2], int.Parse(ids[3]));

            await _botService.Client.EditMessageTextAsync(ids[0], int.Parse(ids[1]), msg.Text, replyMarkup: msg.ReplyMarkup);

            var userMessage = await _context.UserMessages.Include(x => x.Channel).FirstOrDefaultAsync(x => x.MessageId == int.Parse(ids[1]) && x.Channel.ChatId == long.Parse(ids[0]));
            userMessage.Text = msg.Text.Substring(0, msg.Text.Length >= 30 ? 30 : msg.Text.Length) + (msg.Text.Length >= 30 ? "..." : string.Empty);

            await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Публикация сохранена.", replyMarkup: KeyboardMarkups.MainMenu);
        }

        private async Task HandleAccountMenuAction(Update update)
        {
            update.Message = new Message
            {
                Chat = new Chat { Id = update.CallbackQuery.Message.Chat.Id }
            };
            await HandleAccountMenuMessage(update);
        }


        private async Task HandleAddChannelAction(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            await SetUserState(chatId, CallBackActionNames.AddChannel);
            await _botService.Client.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, "Добавьте нашего бота (@publicity_helper_bot) в список администраторов" +
                " добавляемого канала и перешлите сюда любое сообщение с этого канала.", replyMarkup: KeyboardMarkups.BackButton(CallBackActionNames.Account));
        }


        private async Task HandleMainMenulAction(Update update)
        {
            update.Message = new Message
            {
                Chat = new Chat { Id = update.CallbackQuery.Message.Chat.Id }
            };
            await HandleMainMenuMessage(update);
        }

        private async Task HandleChannelInfoAction(Update update)
        {
            var callBackData = update.CallbackQuery.Data;
            string stringChannelId = callBackData.Substring(callBackData.IndexOf("|") + 1);
            if (int.TryParse(stringChannelId, out int _channelId))
            {
                Channel channel = _context.Channels.FirstOrDefault(x => x.ID == _channelId);
                await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.ChannelInfo);

                string subEndDateInfo = string.Empty;

                if (channel.SubscriptionEndDate.HasValue)
                    subEndDateInfo = $"Подписка активна до {channel.SubscriptionEndDate}.{Environment.NewLine}";
                else
                    subEndDateInfo = $"Подписка не активна.{Environment.NewLine}";

                var txt = $"Выбран канал {channel.Title}.{Environment.NewLine}" +
                    subEndDateInfo;

                await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, txt, replyMarkup: KeyboardMarkups.ChannelInfo(channel));
            }
        }

        private async Task HandleCreatePostAction(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);
            if (!update.CallbackQuery.Data.Contains("|"))
            {
                await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.CreatePost, user);
                var channelList = _context.Channels.Where(x => x.OwnerId == user.ID).ToListAsync();
                channelList.Wait();
                var txt = $"Выберите канал для создания публикации.";
                await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, txt, replyMarkup: (InlineKeyboardMarkup)KeyboardMarkups.CreatePost(channelList.Result));
            }
            else
            {
                var callBackData = update.CallbackQuery.Data;
                var chId = callBackData[(callBackData.IndexOf("|") + 1)..];
                Channel channel = await _context.Channels.FirstOrDefaultAsync(x => x.ID == int.Parse(chId));
                await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.CreatePost + "|" + channel.ChatId, user);
                var txt = "Пришлите сюда сообщение, которрое хотите опубликовать:";

                await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId, txt, replyMarkup: KeyboardMarkups.BackButton(CallBackActionNames.CreatePost));
            }
        }



        private async Task HandlePublishAction(Update update)
        {
            TgUser user = await _context.TgUsers.FirstOrDefaultAsync(x => x.ChatID == update.CallbackQuery.Message.Chat.Id);

            if (user == null || !user.CurrentAction.StartsWith(CallBackActionNames.CreatePost + "|")) return;
            var publishChannelId = long.Parse(user.CurrentAction[(user.CurrentAction.IndexOf("|") + 1)..]);
            var text = update.CallbackQuery.Message.Text;

            Channel channel = await _context.Channels.FirstOrDefaultAsync(x => x.ChatId == publishChannelId && x.OwnerId == user.ID);
            if (channel == null) return;


            var message = await _botService.Client.SendTextMessageAsync(publishChannelId, text);

            UserMessage userMessage = new()
            {
                MessageId = message.MessageId,
                Channel = channel,
                Text = text.Substring(0, text.Length >= 30 ? 30 : text.Length) + (text.Length >= 30 ? "..." : string.Empty)
            };

            await _context.UserMessages.AddAsync(userMessage);
            await _context.SaveChangesAsync();

            await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Сообщение опубликовано", replyMarkup: KeyboardMarkups.Default);
        }

        private async Task HandleDeleteChannelAction(Update update)
        {
            await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.DeleteChannel);

            var channelId = long.Parse(update.CallbackQuery.Data[(update.CallbackQuery.Data.IndexOf("|") + 1)..]);
            var channel = await _context.Channels.FirstOrDefaultAsync(x => x.ID == channelId);
            if (channel == null) return;
            await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, $"Ваша подписка по каналу {channel.Title} будет аннулирована. " +
                $"Вы действительно хотите удалить этот канал?", replyMarkup: KeyboardMarkups.DeleteChannel(channel)); ;
        }

        private async Task HandleYesDeleteChannelAction(Update update)
        {
            var channelId = long.Parse(update.CallbackQuery.Data[(update.CallbackQuery.Data.IndexOf("|") + 1)..]);
            var channel = await _context.Channels.FirstOrDefaultAsync(x => x.ID == channelId);
            if (channel == null) return;
            _context.Channels.Remove(channel);

            var task = _context.SaveChangesAsync();
            task.Wait();

            var channels = await _context.Channels.ToListAsync();

            await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, $"Канал {channel.Title} удален.",
                replyMarkup: KeyboardMarkups.BackButton(CallBackActionNames.Account));
        }

        private async Task HandleEditPostAction(Update update)
        {
            var chatId = update.CallbackQuery.Message.Chat.Id;
            TgUser user = _context.TgUsers.FirstOrDefault(x => x.ChatID == chatId);
            if (!update.CallbackQuery.Data.Contains("|"))
            {
                await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.EditPostForChannel, user);
                var channelList = _context.Channels.Where(x => x.OwnerId == user.ID).ToListAsync();
                channelList.Wait();
                var txt = $"Выберите канал для редактирования публикации.";
                await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, txt, replyMarkup: (InlineKeyboardMarkup)KeyboardMarkups.EditPost(channelList.Result));
            }
            else
            {
                var callBackData = update.CallbackQuery.Data;
                var chId = callBackData[(callBackData.IndexOf("|") + 1)..];
                Channel channel = await _context.Channels.FirstOrDefaultAsync(x => x.ID == int.Parse(chId));
                await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.EditPostForChannel + "|" + channel.ChatId, user);

                var messageList = _context.UserMessages.Where(x => x.ChannelId == channel.ID).ToListAsync();
                messageList.Wait();
                await _botService.Client.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id,
                    update.CallbackQuery.Message.MessageId, "Выберите редактируемое сообщение:", replyMarkup: (InlineKeyboardMarkup)KeyboardMarkups.MessageList(messageList.Result));
            }
        }

        private async Task HandleEditMessageAction(Update update)
        {
            if (!int.TryParse(update.CallbackQuery.Data[(update.CallbackQuery.Data.IndexOf("|") + 1)..], out int msgId)) return;
            UserMessage userMessage = await _context.UserMessages.Include(x => x.Channel).FirstOrDefaultAsync(x => x.Id == msgId);
            if (userMessage == null)
                return;
            await SetUserState(update.CallbackQuery.Message.Chat.Id, CallBackActionNames.EditMessage);
            try
            {
                //копируем сообщение. чтобы хоть как то его найти - посылаем копию в наш прокси канал //publike - хз как это еще сделать
                var msg = await _botService.Client.ForwardMessageAsync(PROXY_CHAT_ID
                    , userMessage.Channel.ChatId, (int)userMessage.MessageId);

                //показываем сообщение пользователю
                await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, msg.Text,
                    entities: msg.Entities, replyMarkup: msg.ReplyMarkup);

                var replymsg = await _botService.Client.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Что нужно сделать?",
                    replyMarkup: KeyboardMarkups.WhatCanIdo(userMessage.Channel.ChatId, userMessage.MessageId));
            }
            catch (Exception ex)
            {
                _ = ex;
            }
        }

        private async Task HandleEditMessageTextAction(Update update)
        {
            long ChatId = update.CallbackQuery.Message.Chat.Id;
            await SetUserState(ChatId, update.CallbackQuery.Data);

            await _botService.Client.EditMessageTextAsync(ChatId, update.CallbackQuery.Message.MessageId, "Пришлите боту новый текст:");
        }
        #endregion

        public async Task ErrorMessage(Update update, string er)
        {
            //return;
            var chatId = update.Message?.Chat.Id ?? update.CallbackQuery.Message.Chat.Id;
            await _botService.Client.SendTextMessageAsync(chatId, er);
        }
    }
}
