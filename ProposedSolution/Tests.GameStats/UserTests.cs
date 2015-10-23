using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStats;
using GameStats.Model;
using GameStats.Persistence;
using Xunit;

namespace Tests.GameStats
{

    public class UserApiTests
    {
        private DataStorage storage;

        public UserApiTests()
        {
            storage = new DataStorage();
            storage.Characters = GenerateCharacters();
            storage.Duels = GenerateDuels();
            storage.Users = GenerateUsers();


            GenerateUsers();
        }

        private List<Character> GenerateCharacters()
        {
            return
                new List<Character>
                {
                    Character.Create("Conan", CharacterClass.Barbarian, 1),
                    Character.Create("Irenicus", CharacterClass.Wizard, 2),
                    Character.Create("Milva", CharacterClass.Rogue, 3),
                    Character.Create("Grom Hellscream", CharacterClass.Barbarian, 4),
                    Character.Create("Saruman the wise", CharacterClass.Wizard, 5),
                    Character.Create("Kedrigern", CharacterClass.Wizard, 6),
                    Character.Create("Lobo", CharacterClass.Barbarian, 7),
                    Character.Create("Zawisza Czarny", CharacterClass.Knight, 8),
                    Character.Create("Robin Hood", CharacterClass.Rogue, 9),
                    Character.Create("Uther the Lightbringer", CharacterClass.Knight, 10),
                    Character.Create("Yennefer", CharacterClass.Wizard, 11),
                    Character.Create("Janosik", CharacterClass.Rogue, 12),
                    Character.Create("Merlin", CharacterClass.Wizard, 13),
                    Character.Create("Sir Lancelot", CharacterClass.Knight, 14),
                };
        } 

        private List<Duel> GenerateDuels()
        {
            var duel1 = new Duel
            {
                Winner = storage.Characters[0],
                Characters = new List<Character>
                {
                    storage.Characters[0],
                    storage.Characters[1],
                    storage.Characters[4]
                },
                Id = 1
            };

            var duel2 = new Duel
            {
                Winner = storage.Characters[6],
                Characters = new List<Character>
                {
                    storage.Characters[5],
                    storage.Characters[6]
                },
                Id = 2
            };

            var duel3 = new Duel
            {
                Winner = storage.Characters[3],
                Characters = new List<Character>
                {
                    storage.Characters[2],
                    storage.Characters[3],
                    storage.Characters[5]
                },
                Id = 3
            };

            var duel4 = new Duel
            {
                Winner = storage.Characters[2],
                Characters = new List<Character>
                {
                    storage.Characters[2],
                    storage.Characters[1],
                },
                Id = 4
            };

            var duel5 = new Duel
            {
                Winner = storage.Characters[5],
                Characters = new List<Character>
                {
                    storage.Characters[2],
                    storage.Characters[4],
                    storage.Characters[5]
                },
                Id = 5
            };

            var duel6 = new Duel
            {
                Winner = storage.Characters[2],
                Characters = new List<Character>
                {
                    storage.Characters[1],
                    storage.Characters[3],
                    storage.Characters[2]
                },
                Id = 6
            };

            return new List<Duel> {duel1, duel2, duel3, duel4, duel5, duel6};
        } 

        private List<User> GenerateUsers()
        {
            var users = Enumerable.Range(0, 5)
                            .Select(i => UserExtensions.InitUser(i))
                            .ToList();

            var characters = storage.Characters;

            users[0].AddFriends(new [] {users[1], users[4]});
            users[0].AddCharacters(new [] {characters[0], characters[5]});
            users[1].AddCharacters(new[] { characters[1], characters[6] });
            users[2].AddCharacters(new[] { characters[2], characters[7] });
            users[3].AddCharacters(new[] { characters[3]});
            users[4].AddCharacters(new[] { characters[4]});

            return users;
        }

        


        [Fact]
        public void GetFriendsOnlineGivesListOfContacts()
        {
            var api = new GameStatsApi(storage);
            var friends = api.GetFriendsOnline(0);

            Assert.Equal(storage.Users[0].Friends, friends);
        }

        [Fact]
        public void GetFriendsOnlineGivesEmptyWhenNoFriends()
        {
            var api = new GameStatsApi(storage);
            var friends = api.GetFriendsOnline(1);

            Assert.Equal(new List<User>(), friends);
        }

        [Fact]
        public void GetCommonDuelsGivesListOfDuels()
        {
            var api = new GameStatsApi(storage);
            var duels = api.GetCommonDuels(0, 1);

            Assert.Equal(new [] {storage.Duels[0], storage.Duels[1]}, duels);
        }

        [Fact]
        public void GetCommonDuelsGivesGivesEmptyWhenNoCommonDuels()
        {
            var api = new GameStatsApi(storage);
            var duels = api.GetCommonDuels(3, 4);

            Assert.Equal(new Duel[] {}, duels);
        }

        [Fact]
        public void GetClassPopularityReturnsCorrectValues()
        {
            var api = new GameStatsApi(storage);
            var classPop = api.GetCharacterClassPopularity();

            Assert.Equal(Percent(3, 14), classPop["Rogue"]);
            Assert.Equal(Percent(3, 14), classPop["Knight"]);
            Assert.Equal(Percent(5, 14), classPop["Wizard"]);
            Assert.Equal(Percent(3, 14), classPop["Barbarian"]);
            
        }

        [Fact]
        public void GetAllUsersVictoriesReturnsCorrectValues()
        {
            var api = new GameStatsApi(storage);
            var usersVictories = api.GetVictoryCountPerUser();

            Assert.Equal(2, usersVictories[storage.Users[0]]);
            Assert.Equal(1, usersVictories[storage.Users[1]]);
            Assert.Equal(2, usersVictories[storage.Users[2]]);
            Assert.Equal(1, usersVictories[storage.Users[3]]);
            Assert.Equal(0, usersVictories[storage.Users[4]]);
        }

        [Fact]
        public void GetClassRatingReturnsCorrectValues()
        {
            var api = new GameStatsApi(storage);

            Assert.Equal(30, api.GetClassRating("Barbarian"));
            Assert.Equal(31, api.GetClassRating("Rogue"));
            Assert.Equal(31, api.GetClassRating("Wizard"));
            Assert.Equal(0, api.GetClassRating("Knight"));

        }

        private static double Percent(double x, double total)
        {
            return (x/total)*100.0;
        }
    }
}
