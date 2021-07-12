using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.PublicityHelperBot.Models.Database;

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


        public static IReplyMarkup Account(List<Channel> channels)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            foreach (Channel channel in channels)
            {
                keyboardList.Add
                    (
                        new List<InlineKeyboardButton>()
                        {
                            new InlineKeyboardButton()
                            {
                                Text = channel.Title,
                                CallbackData = $"{CallBackActionNames.ChannelInfo}|{channel.ID}"
                            }
                        }
                    );
            }
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.AddChannel,
                    CallbackData = CallBackActionNames.AddChannel
                }
            });
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.Back,
                    CallbackData = CallBackActionNames.MainMenu
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup ChannelInfo(Channel channel)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            keyboardList.Add
                (
                    new List<InlineKeyboardButton>()
                    {
                        new InlineKeyboardButton()
                        {
                            Text = KeyboardTitles.BuySubscription,
                            CallbackData = $"{CallBackActionNames.BuySubscription}|{channel.ID}"
                        }
                    }
                );
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.DeleteChannel,
                    CallbackData = CallBackActionNames.DeleteChannel
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }
    }
}
