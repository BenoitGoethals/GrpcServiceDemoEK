using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ChatCommon.Annotations;

namespace ChatCommon.model
{
  public  class ChatRoom :INotifyPropertyChanged
  {
      public Guid Guid { get; set; } = Guid.NewGuid();
        private  ObservableCollection<Chatter> _chatters=new ObservableCollection<Chatter>();
        private  ObservableCollection<Message> _messages=new ObservableCollection<Message>();
        private string _name;


        public ObservableCollection<Chatter> Chatters //Binds with the listbox
        {
            get { return _chatters; }
            set { _chatters = value;
                RaisePropertyChanged(nameof(Chatters));
            }
        }

        public ObservableCollection<Message> Messages //Binds with the listbox
        {
            get { return _messages; }
            set
            {
                _messages = value;
                RaisePropertyChanged(nameof(Messages));
            }
        }

        public string Name
        {
            get => _name;
            set { _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public ChatRoom()
        {
         //   Chatters.Add(new Chatter() { Enabled = true, Guid = Guid.NewGuid(), Name = "sfdfdsfsd" });
         
        }


        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }

        public void AddEventHandeler(NotifyCollectionChangedEventHandler chatters_CollectionChanged, NotifyCollectionChangedEventHandler message_CollectionChanged)
        {
            _chatters.CollectionChanged += chatters_CollectionChanged; ;
           _messages.CollectionChanged += message_CollectionChanged;
        }

        

        public IList<Message> GetAllMessages()
        {
            return _messages.ToList();
        }


        public void AddChatter(Chatter chatter)
        {
            _chatters.Add(chatter);
        }

        public IList<Chatter> GetChatters()
        {
            return _chatters.ToList();
        }

        public override string ToString()
        {
            return $"{nameof(Guid)}: {Guid}, {nameof(Chatters)}: {Chatters}";
        }

        protected bool Equals(ChatRoom other)
        {
            return Guid.Equals(other.Guid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChatRoom) obj);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
