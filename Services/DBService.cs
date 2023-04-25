using SeaBattleTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SeaBattleTelegramBot.Models.SeaBattleAdjustments;

namespace SeaBattleTelegramBot.Services
{
    public class DBService
    {
        private readonly DBContext _context;
        public DBService(DBContext context)
        {
            _context = context;
        }

        public User FindUser(long id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }
        public User FindUserByGameId(long userId, string id)
        {
            return _context.Users.FirstOrDefault(u => u.GameId == id && u.UserId!=userId);
        }
        public async Task SetStatus(long id, Status status)
        {
            User us = _context.Users.FirstOrDefault(u => u.UserId == id);
            us.Status = (int)status;
            await _context.SaveChangesAsync();
        }
        public async Task SetGameId(long id, string gameid)
        {
            User us = _context.Users.FirstOrDefault(u => u.UserId == id);
            us.GameId = gameid;
            await _context.SaveChangesAsync();
        }
        public async Task SetEnemyField(long id, string field)
        {
            User us = _context.Users.FirstOrDefault(u => u.UserId == id);
            us.EnemyField = field;
            await _context.SaveChangesAsync();
        }
        public async Task SetShips(long id, string field)
        {
            User us = _context.Users.FirstOrDefault(u => u.UserId == id);
            us.Ships = field;
            await _context.SaveChangesAsync();
        }
        public User FindFree(long id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId != id && u.Status==(int)Status.NotInGame);
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
