using System.Collections.Generic;
using GameStats.Model;

namespace Tests.GameStats
{
    public static class UserExtensions
    {
        public static User InitUser(long id)
        {
            return new User
            {
                Characters = new List<Character>(),
                Id = id,
                Friends = new List<User>(),
            };
        }

        public static void AddFriends(this User u, IEnumerable<User> friends)
        {
            u.Friends.AddRange(friends);
        }

        public static void AddCharacters(this User u, IEnumerable<Character> characters)
        {
            u.Characters.AddRange(characters);
        }

    }
}