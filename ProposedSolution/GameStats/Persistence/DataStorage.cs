using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStats.Model;

namespace GameStats.Persistence
{
    public class DataStorage
    {
        public List<User> Users { get; set; }
        public List<Duel> Duels { get; set; }
        public List<Character> Characters { get; set; }


    }
}
