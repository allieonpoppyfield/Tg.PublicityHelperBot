using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tg.PublicityHelperBot.Models;

namespace Tg.PublicityHelperBot.Services
{
    public class ChatCollectionService : IChatCollectionService
    {
        public List<ChatElement> ChatElements { get; }

        public ChatCollectionService()
        {
            ChatElements = new List<ChatElement>();
        }
    }
}
