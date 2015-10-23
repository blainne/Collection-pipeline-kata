using System;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStats.Model
{
    public class Character : IEquatable<Character>
    {
        public string Name { get; set; }

        public CharacterClass Class { get; set; }

        public static Character Create(string name, CharacterClass @class, long id)
            => new Character {Name = name, Class = @class, Id = id};

        public long Id { get; set; }

        #region IEquatable implementation
        public bool Equals(Character other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Character)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
