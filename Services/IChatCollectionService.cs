using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tg.PublicityHelperBot.Models;

namespace Tg.PublicityHelperBot.Services
{
    public interface IChatCollectionService
    {
        public List<LocalChat> LocalChatsList { get; }
    }
}
