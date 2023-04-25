using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTelegramBot.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public string? Username { get; set; }
        public string Ships { get; set; }
        public string EnemyField { get; set; }
        public int Status { get; set; }
        public string? GameId { get; set; }
    }
}
