using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStats.Model
{
    public class User : IEquatable<User>
    {
        public long Id { get; set; }
        public List<User> Friends { get; set; } 
        public List<Character> Characters { get; set; }

        #region IEquatable implementation
        public bool Equals(User u)
        {
            if (ReferenceEquals(null, u)) return false;
            if (ReferenceEquals(this, u)) return true;
            return Id == u.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion
    }
}
