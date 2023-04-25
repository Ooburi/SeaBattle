using Microsoft.EntityFrameworkCore;
using SeaBattleTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTelegramBot
{
    public class DBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(BotSettings.ConnectionString);
        }

        public DbSet<User> Users { get; set; }
    }
}
