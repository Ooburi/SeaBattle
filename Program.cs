using Microsoft.EntityFrameworkCore;
using SeaBattleTelegramBot.Services;
using System;

namespace SeaBattleTelegramBot
{
    class Program
    {
        private static DBContext _dbContext;
        private static DBService _db;
        static void Main(string[] args)
        {
            try
            {
                _dbContext = new DBContext();
                try
                {
                    _dbContext.Database.Migrate();
                }
                catch
                {

                }

                _db = new DBService(_dbContext);

                Bot.Get(_db);

                Console.WriteLine("Бот запущен, работает, всё нормально");
                Console.ReadLine();
            }
            catch
            {


            }
            finally
            {
                _dbContext.Database.CloseConnection();
            }
        }
    }
}
