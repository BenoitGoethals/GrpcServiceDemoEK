using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ChatClient.model;
using ChatClient.Service;
using ChatCommon.model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace ChatClient.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifySettings
    {
        private IDialogService _dialogService;
        private string _title = "GRPC CHAT Application";
        public Settings SettingsApp { get; }
        public  RoomService RoomService { get; }
        private string _localUser;

        private string _chatRoom;
        private string _messageText;

        public MainWindowViewModel(IDialogService dialogService, Settings settings, RoomService roomService)
        {
            settings.Subscribe(this);
            this._dialogService = dialogService;
            SettingsApp = settings;
            RoomService = roomService;

            LoadedCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog("Login", new DialogParameters(), r => { });
                RoomService.Start();
            });

            SendCommand = new DelegateCommand(() =>
            {
                RoomService.AddMessage(new Message()
                    {ChatRoom = settings.ChatRoomName, Chatter = SettingsApp.ChatterLocal, Content = MessageText, Id = Guid.NewGuid()});
                
                MessageText = "";
            });

            RoomService.Chatters.CollectionChanged += Chatters_CollectionChanged;
            RoomService.Messages.CollectionChanged += Messages_CollectionChanged;

        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RoomService));
        }

        private void Chatters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(RoomService.Chatters));
        }

       

        public string MessageText
        {
            get => _messageText;
            set { _messageText = value; SetProperty(ref _messageText, value); RaisePropertyChanged(nameof(MessageText)); }
        }

        public string LocalUser
        {
            get => _localUser;
            set
            {
                _localUser = value;

                SetProperty(ref _localUser, value);
                RaisePropertyChanged(nameof(LocalUser));
            }
        }

        public string ChatRoom
        {
            get => _chatRoom;
            set
            {
                _chatRoom = value;

                SetProperty(ref _chatRoom, value);
                RaisePropertyChanged(nameof(ChatRoom));
            }
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public DelegateCommand LoadedCommand { get; private set; }

        public DelegateCommand SendCommand { get; private set; }


      



        public void SettingChanged(Settings settings)
        {
            LocalUser = settings.ChatterLocal;
            ChatRoom = settings.ChatRoomName;
          
            RoomService.AddChatter(LocalUser);
        }
    }
}
