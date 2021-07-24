using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tg.PublicityHelperBot.Models.Database
{
    public class Channel
    {
        public long ID { get; set; }

        public long ChatId { get; set; }
        public string Title { get; set; }


        public TgUser Owner { get; set; }
        public long OwnerId { get; set; }

        public DateTime? SubscriptionEndDate { get; set; }
    }
}
