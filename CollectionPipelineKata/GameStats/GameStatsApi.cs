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
            var user = FindUserById(userId);

            return (user == null)
                ? new List<User>()
                : user.Friends;
        }

        public Dictionary<string, double> GetCharacterClassPopularity()
        {
            var occurences = new Dictionary<string, int>();
            foreach (var ch in storage.Characters)
            {
                var className = ch.Class.Name;
                if (!occurences.ContainsKey(className))
                    occurences.Add(className, 0);

                occurences[className]++;
            }


            double total = storage.Characters.Count;
            var resultDict = new Dictionary<string, double>();
            foreach (var cl in occurences.Keys)
            {
                resultDict.Add(cl, Percent(occurences[cl], total));
            }

            return resultDict;
        }

        public Dictionary<User, int> GetVictoryCountPerUser()
        {
            var dict = new Dictionary<User, int>();

            foreach (var duel in storage.Duels)
            {
                foreach (var user in storage.Users)
                {
                    if (!dict.ContainsKey(user))
                        dict.Add(user, 0);

                    if (user.Characters.Contains(duel.Winner))
                        dict[user]++;

                }
            }

            return dict;
        }

        public List<Duel> GetCommonDuels(long userId, long opponentId)
        {
            var u1 = FindUserById(userId);
            var u2 = FindUserById(opponentId);

            List<Duel> commonDuels = new List<Duel>();

            foreach (var duel in storage.Duels)
            {
                bool u1CharFound = false;
                bool u2CharFound = false;
                foreach (var c in duel.Characters)
                {
                    if (u1.Characters.Contains(c))
                        u1CharFound = true;

                    if (u2.Characters.Contains(c))
                        u2CharFound = true;
                }

                if(u1CharFound && u2CharFound)
                    commonDuels.Add(duel);
            }

            return commonDuels;
        }

        public int GetClassRating(string className)
        {
            var points = GetClassHandicap(className);
            foreach (var duel in storage.Duels)
            {
                points += GetPointsForDuel(className, duel);
            }

            return points;
        }



        public GameStatsApi(DataStorage storage)
        {
            this.storage = storage;
        }

        private readonly DataStorage storage;

        private User FindUserById(long userId)
        {
            User user = null;
            foreach (var u in storage.Users)
            {
                if (u.Id == userId)
                {
                    user = u;
                    break;
                }
            }
            return user;
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
