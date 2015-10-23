using System;
using System.Collections.Generic;

namespace GameStats.Model
{
    public class CharacterClass : IEquatable<CharacterClass>
    {
        public string Name { get; set; }

        public List<string> Abilities { get; set; }

        #region some ready definitions
        public static CharacterClass Rogue =>
            new CharacterClass
            {
                Name = "Rogue",
                Abilities = new List<string>() {"Lockpicking", "Silent walk", "Archery"}
            };

        public static CharacterClass Knight =>
            new CharacterClass
            {
                Name = "Knight",
                Abilities = new List<string> {"Shield fighting", "Command", "Sword fighting", "Archery"}
            };

        public static CharacterClass Wizard =>
            new CharacterClass
            {
                Name = "Wizard",
                Abilities = new List<string> { "Spellcasting", "Sorcery", "Advisory", "Silent walk" }
            };

        public static CharacterClass Barbarian =>
            new CharacterClass
            {
                Name = "Barbarian",
                Abilities = new List<string> { "Axe fighting", "Athletics", "Stamina", "Sword fighting" }
            };
        #endregion

        #region IEquatable implementation
        public bool Equals(CharacterClass other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CharacterClass)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        #endregion
    }
}