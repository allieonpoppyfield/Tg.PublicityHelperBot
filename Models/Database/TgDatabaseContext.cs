using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tg.PublicityHelperBot.Models.Database
{
    public class TgDatabaseContext : DbContext
    {
        public TgDatabaseContext(DbContextOptions<TgDatabaseContext> options) : base(options)
        { }
        public DbSet<TgUser> TgUsers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
