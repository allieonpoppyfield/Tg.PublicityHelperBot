using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tg.PublicityHelperBot.Models.Database
{
    public class TgUser
    {
        public long ID { get; set; }
        
        
        public long ChatID { get; set; }
        
        public DateTime RegisterDate { get; set; }

        public List<Channel> Channels { get; set; }

        public string CurrentAction { get; set; }
    }
}
