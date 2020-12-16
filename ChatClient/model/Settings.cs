using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.ViewModels;
using ChatCommon.model;
using Prism.Mvvm;

namespace ChatClient.model
{
    public  class Settings
    {
        private readonly IList<INotifySettings> _bindableBases=new List<INotifySettings>();
        private string _chatter;
        private string _chatRoomName;

        public string ChatterLocal
        {
            get => _chatter;
            set
            {
                _chatter = value;
               
            }
        }


        public string ChatRoomName
        {
            get => _chatRoomName;
            set { _chatRoomName = value; Notify(this); }
        }

        
        public void SetSettings(string room, string chatter)
        {
            this.ChatRoomName = room;
            this.ChatterLocal = chatter;
            Notify(this);
        }
        

        
        public  void Subscribe(INotifySettings obj)
        {
            _bindableBases.Add(obj);
        }

        public  void Notify(Settings set)
        {
            foreach (var notifySettingse in _bindableBases)
            {
                notifySettingse.SettingChanged(this);
            }
        }
    }
}
