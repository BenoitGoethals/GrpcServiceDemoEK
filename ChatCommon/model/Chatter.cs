using System;

namespace ChatCommon.model
{
    public class Chatter
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public bool Enabled { get; set; }

        protected bool Equals(Chatter other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Chatter) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }


        public override string ToString()
        {
            return $"{nameof(Guid)}: {Guid}, {nameof(Name)}: {Name}, {nameof(Enabled)}: {Enabled}";
        }
    }
}