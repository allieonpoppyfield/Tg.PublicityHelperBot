using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tg.PublicityHelperBot.Models;

namespace Tg.PublicityHelperBot.Services
{
    public class ChatCollectionService : IChatCollectionService
    {
        public List<LocalChat> LocalChatsList { get; }

        public ChatCollectionService()
        {
            LocalChatsList = new List<LocalChat>();
        }
    }
}
