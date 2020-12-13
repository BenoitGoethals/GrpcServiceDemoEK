using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ChatCommon.model
{
  public  class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public Chatter Chatter { get; set; }


        protected bool Equals(Message other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Message) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Content)}: {Content}, {nameof(ChatRoom)}: {ChatRoom}, {nameof(Chatter)}: {Chatter}";
        }
    }
}
