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
                CallbackData = CallBackActionNames.EditPostForChannel
            }
        });

        public static IReplyMarkup CreatingPost()
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();

            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.Publish,
                    CallbackData = CallBackActionNames.Publish
                }
            });

            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.MainMenuTitle,
                    CallbackData = CallBackActionNames.MainMenu
                }
            });

            return new InlineKeyboardMarkup(keyboardList);
        }

        public static IReplyMarkup CreatePost(List<Channel> channels)
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
                                CallbackData = $"{CallBackActionNames.CreatePost}|{channel.ID}"
                            }
                        }
                    );
            }
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
                    CallbackData = $"{CallBackActionNames.DeleteChannel}|{channel.ID}"
                }
            });
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.Back,
                    CallbackData = CallBackActionNames.Account
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup DeleteChannel(Channel channel)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            keyboardList.Add
                (
                    new List<InlineKeyboardButton>()
                    {
                        new InlineKeyboardButton()
                        {
                            Text = KeyboardTitles.YesDelete,
                            CallbackData = $"{CallBackActionNames.YesDelete}|{channel.ID}"
                        }
                    }
                );
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.NoDelete,
                    CallbackData = $"{CallBackActionNames.ChannelInfo}|{channel.ID}"
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }

        internal static InlineKeyboardMarkup EditPost(List<Channel> channels)
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
                                CallbackData = $"{CallBackActionNames.EditPostForChannel}|{channel.ID}"
                            }
                        }
                    );
            }
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


        internal static InlineKeyboardMarkup MessageList(List<UserMessage> messages)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            foreach (UserMessage message in messages)
            {
                keyboardList.Add
                    (
                        new List<InlineKeyboardButton>()
                        {
                            new InlineKeyboardButton()
                            {
                                Text = message.Text,
                                CallbackData = $"{CallBackActionNames.EditMessage}|{message.Id}"
                            }
                        }
                    );
            }
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.Back,
                    CallbackData = CallBackActionNames.EditPostForChannel
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }


        internal static InlineKeyboardMarkup WhatCanIdo(long chatId, int messageId)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.EditMessageText,
                    CallbackData = $"{CallBackActionNames.EditMessageText}|{chatId}|{messageId}",
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }

        internal static InlineKeyboardMarkup WhatCanIdoEdited(long chatId, int messageId, int newMessageChatId, int newMessageId)
        {
            var keyboardList = new List<List<InlineKeyboardButton>>();
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.EditMessageText,
                    CallbackData = $"{CallBackActionNames.EditMessageText}|{chatId}|{messageId}",
                }
            });
            keyboardList.Add(new()
            {
                new()
                {
                    Text = KeyboardTitles.EditedPublish,
                    CallbackData = $"{CallBackActionNames.EditedPublish}|{chatId}|{messageId}|{newMessageChatId}|{newMessageId}",
                }
            });
            return new InlineKeyboardMarkup(keyboardList);
        }
    }
}
