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
        private  IList<INotifySettings> bindableBases=new List<INotifySettings>();
        private  Chatter _chatter;
        private  string _chatRoom;

        public Chatter ChatterLocal
        {
            get => _chatter;
            set
            {
                _chatter = value;
                Notify(this);
            }
        }


        public string ChatRoomName
        {
            get => _chatRoom;
            set
            {
                _chatRoom = value;
               
            }
        }

        public  void Subscribe(INotifySettings obj)
        {
            bindableBases.Add(obj);
        }

        public  void Notify(Settings set)
        {
            foreach (var notifySettingse in bindableBases)
            {
                notifySettingse.SettingChanged(this);
            }
        }
    }
}
