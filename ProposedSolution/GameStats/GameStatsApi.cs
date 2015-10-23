using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStats.Model;
using GameStats.Persistence;

namespace GameStats
{
    public class GameStatsApi
    {
        public List<User> GetFriendsOnline(long userId)
        {
            var user =
                storage
                .Users
                .SingleOrDefault(u => u.Id == userId);
            
            return (user == null)
                ? new List<User>()
                : user.Friends;
        }

        public Dictionary<string, double> GetCharacterClassPopularity()
        {
            double total = storage.Characters.Count;

            var result =
                storage.Characters
                .GroupBy(ch => ch.Class.Name)
                .ToDictionary(
                            group => group.Key, 
                            group => Percent(group.Count(), total));


            return result;
        }

        public Dictionary<User, int> GetVictoryCountPerUser()
        {
            var duels =
                storage
                    .Duels
                    .SelectMany(
                        duel => storage.Users,
                        (d, u) => new {Duel = d, User = u}
                    )
                    .GroupBy(pair => pair.User)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Count(pair => UserHasDuelWinner(pair.User, pair.Duel)));

            return duels;
        }

        private static bool UserHasDuelWinner(User u, Duel d)
        {
            return u.Characters.Contains(d.Winner);
        }


        public List<Duel> GetCommonDuels(long userId, long opponentId)
        {
            var u1 = FindUserById(userId);
            var u2 = FindUserById(opponentId);

            var result = 
                storage.Duels
                .Where(duel => UserWasTakingPartInDuel(duel, u1))
                .Where(duel => UserWasTakingPartInDuel(duel, u2))
                .ToList();
            
            return result;
        }

        private static bool UserWasTakingPartInDuel(Duel duel, User u1)
        {
            return duel.Characters.Any(c => u1.Characters.Contains(c));
        }


        public int GetClassRating(string className)
        {
            var result = 
                storage
                .Duels
                .Aggregate(
                    GetClassHandicap(className),
                    (p, d) => p + GetPointsForDuel(className, d));

            
            return result;
        }



        public GameStatsApi(DataStorage storage)
        {
            this.storage = storage;
        }

        private readonly DataStorage storage;

        private User FindUserById(long userId)
        {
            return storage.Users.SingleOrDefault(u => u.Id == userId);
        }

        // 3 points for each non-winning character of given class that fought in duel
        // 7 points for winner of the duel when there were 2 characters fighting
        // 10 points for winner of the duel when there were 3 or more characters fighting
        private int GetPointsForDuel(string className, Duel duel)
        {
            var points = 0;
            foreach (var character in duel.Characters)
            {
                if (character.Class.Name != className) continue;
                if (duel.Winner.Equals(character))
                {
                    if (duel.Characters.Count >= 3) points += 10;
                    else points += 7;
                }
                else
                {
                    points += 3;
                }
            }
            return points;
        }

        private int GetClassHandicap(string className)
        {
            if (className == CharacterClass.Rogue.Name)
            {
                return 8;
            }
            return 0;
        }

        private static double Percent(double x, double total)
        {
            return (x/total)*100.0;
        }
    }

}
