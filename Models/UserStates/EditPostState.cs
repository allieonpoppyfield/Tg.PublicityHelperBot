using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Services;

namespace Tg.PublicityHelperBot.Models.UserStates
{
    public class EditPostState : ChatState
    {
        public EditPostState(IUpdateService _updateService, LocalChat localChatItem) : base(_updateService, localChatItem)
        {
        }


        public override async Task ProcessUpdate(Update update)
        {
            if (update.Message.Text == MenuItemNames.MainMenuVisible)
            {
                await localChatItem.SetState(UserStatesEnum.MainMenu);
            }
        }

        public override async Task StateSelected(long ChatId)
        {
            string txt = "находимся в Редактировании поста";
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
                        new KeyboardButton(MenuItemNames.PayForSubscription),
                    },

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
