using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot.Models.UserStates
{
    public class AddChannelState : ChatState
    {
        public AddChannelState(IUpdateService _updateService, LocalChat localChatItem) : base(_updateService, localChatItem)
        {
        }


        public override async Task ProcessUpdate(Update update)
        {
            if (update.Message.Text == MenuItemNames.MainMenuVisible)
            {
                await localChatItem.SetState(UserStatesEnum.MainMenu);
            }

            if (update.Message.ForwardFromChat is Chat chat && chat.Type == Telegram.Bot.Types.Enums.ChatType.Channel)
            {
                await updateService.SendTextMessageAssync(update.Message.Chat.Id, $"Вы выбрали канал {chat.Title ?? "<без имени>"}");

                if (await updateService.IsAdmin(chat.Id))
                {
                    await updateService.SendTextMessageAssync(update.Message.Chat.Id, $"Отправляем туда сообщение с текстом");
                    await updateService.SendTextMessageAssync(chat.Id, "привет");
                }
                else
                {
                    await updateService.SendTextMessageAssync(update.Message.Chat.Id, $"Необходимо добавить нашего бота (@publicity_helper_bot) " +
                        $"в администраторы вашего канала и дать ему права на написание и редактирование сообщений!");
                }

            }
        }

        public override async Task StateSelected(long ChatId)
        {
            string txt = "Добавьте нашего бота (@publicity_helper_bot) в администраторы нужного канала. Затем перешлите сюда любое сообщение с этого канала.";
            await updateService.SendTextMessageAssync(ChatId, txt, ReplyMarkup());
        }

        protected override IReplyMarkup ReplyMarkup()
        {
            var result = new ReplyKeyboardMarkup()
            {
                Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(MenuItemNames.MainMenuVisible),
                    }
                }
            };

            return result;
        }
    }
}
