using System;
using System.Collections.Specialized;
using System.ComponentModel;
using ChatClient.model;
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
        private Settings _settings;


        public MainWindowViewModel(ChatRoom chatRoom, IDialogService dialogService, Settings settings)
        {
            settings.Subscribe(this);
            this._dialogService = dialogService;
            _settings = settings;

            LoadedCommand = new DelegateCommand(() =>
            {
                _dialogService.ShowDialog("Login", new DialogParameters(), r => { });
            });

            SendCommand = new DelegateCommand(() =>
              {
                  Room.AddMessage(new Message() { ChatRoom = Room, Chatter = _settings.ChatterLocal, Content = MessageText, Id = Guid.NewGuid() });
                  MessageText = "";
              });
            _room = chatRoom;
            _room.AddEventHandeler(ChattersEvent, MessagesEvent);
        }

        private void MessagesEvent(object? sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void ChattersEvent(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Room));
        }

        public string MessageText
        {
            get => _messageText;
            set { _messageText = value; SetProperty(ref _messageText, value); }
        }

        public Chatter LocalUser
        {
            get => _localUser;
            set
            {
                _localUser = value;

                SetProperty(ref _localUser, value);
                RaisePropertyChanged(nameof(LocalUser));
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public DelegateCommand LoadedCommand { get; private set; }

        public DelegateCommand SendCommand { get; private set; }


        public ChatRoom Room
        {
            get => _room;
            set => _room = value;
        }

        private Chatter _localUser;
        private ChatRoom _room;
        private string _messageText;


        public void SettingChanged(Settings settings)
        {
            LocalUser = settings.ChatterLocal;
            Room.AddChatter(LocalUser);
        }
    }
}
