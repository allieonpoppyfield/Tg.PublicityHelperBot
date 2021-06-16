using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Models
{
    public static class MenuItemMarkups
    {
        public static IReplyMarkup MainMenuItems =>
            new ReplyKeyboardMarkup()
            {
                Keyboard =
                new KeyboardButton[][]
                {
                    new KeyboardButton[]
                    {
                        new KeyboardButton(MenuItemNames.CreatePost),
                        new KeyboardButton(MenuItemNames.EditPost),
                    },

                    new KeyboardButton[]
                    {
                        new KeyboardButton(MenuItemNames.AddChannel)
                    }
                }
            };
        public static IReplyMarkup EditPostItems =>
            new ReplyKeyboardMarkup()
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

    }
}
