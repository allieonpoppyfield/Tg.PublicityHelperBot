using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Tg.PublicityHelperBot.Static
{
    public static class KeyboardMarkups
    {
        public static IReplyMarkup Default => new ReplyKeyboardMarkup()
        {
            Keyboard =
                 new KeyboardButton[][]
                 {
                     new KeyboardButton[]
                    {
                        new KeyboardButton(KeyboardTitles.MainMenuTitle),
                        new KeyboardButton(KeyboardTitles.AccountTitle)
                    },
                 },
            ResizeKeyboard = true
        };




        public static InlineKeyboardButton BackButton(string callBackActionName) =>
            new()
            {
                Text = KeyboardTitles.Back,
                CallbackData = callBackActionName
            };


        public static IReplyMarkup MainMenu => new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
        {
            new()
            {
                Text =  KeyboardTitles.CreatePost,
                CallbackData = CallBackActionNames.CreatePost
            },

            new()
            {
                Text =  KeyboardTitles.EditPost,
                CallbackData = CallBackActionNames.EditPost
            }
        });


        public static IReplyMarkup Account => new InlineKeyboardMarkup(new List<InlineKeyboardButton>()
        {
            new()
            {
                Text =  KeyboardTitles.AddChannel,
                CallbackData = CallBackActionNames.AddChannel,
            }

        });

    }
}
