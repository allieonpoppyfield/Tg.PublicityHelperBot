using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tg.PublicityHelperBot.Models.Database
{
    public class UserMessage
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long ChannelId { get; set; }
        public Channel Channel { get; set; }

        public int MessageId { get; set; }
    }
}
