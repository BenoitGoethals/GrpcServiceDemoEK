using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatCommon.model;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceChatter;

namespace ChatClient.Service
{
    public class RoomService : IDisposable, INotifyPropertyChanged
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private ObservableCollection<string> _chatters = new ObservableCollection<string>();
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();

        private readonly ConcurrentQueue<Message> _concurrentQueue = new ConcurrentQueue<Message>();

        private readonly AsyncDuplexStreamingCall<MsgChat, MsgChat> _call;
        private string _name;

        private readonly GrpcChannel _channelQAddress = GrpcChannel.ForAddress("https://localhost:5001");
        private readonly ChatRoomService.ChatRoomServiceClient _client;

        public RoomService()
        {

            _client = new GrpcServiceChatter.ChatRoomService.ChatRoomServiceClient(_channelQAddress);
           _call = _client.SendMessage();

        }



        public ObservableCollection<string> Chatters //Binds with the listbox
        {
            get => _chatters;
            set
            {
                _chatters = value;
                RaisePropertyChanged(nameof(Chatters));
            }
        }

        public ObservableCollection<Message> Messages //Binds with the listbox
        {
            get => _messages;
            set
            {
                _messages = value;
                RaisePropertyChanged(nameof(Messages));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }


        public IList<Message> GetAllMessages()
        {
            return _messages.ToList();
        }


        public void AddChatter(string chatter)
        {
            if (!Chatters.Contains(chatter))
            {
                _chatters.Add(chatter);
            }

        }

        public IList<string> GetChatters()
        {
            return _chatters.ToList();
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void Start()
        {
            var messagesAsync = GetMessagesAsync();

        }

        public void Stop()
        {

            _cancellationTokenSource.Cancel();

        }


        public async Task GetMessagesAsync()
        {
            var token = _cancellationTokenSource.Token;
        

            var ret = Task.Run(async () =>
             {
               //  using var call = _client.SendMessage();
                 while (!token.IsCancellationRequested)
                 {
                 
                     //   await Task.Delay(TimeSpan.FromSeconds(1), token);
                     _concurrentQueue.TryDequeue(out Message msgSend);
                     if (msgSend != null)
                     {
                         await _call.RequestStream.WriteAsync(new MsgChat()
                         {
                             Chatroon = msgSend.ChatRoom,
                             Chatter = msgSend.Chatter,
                             Id = Guid.NewGuid().ToString(),
                             Msg = msgSend.Content
                         });
                     }

                 }
             }, token);

            while (_call != null && await _call.ResponseStream.MoveNext())
            {
                var msg = _call.ResponseStream.Current;
                this._messages.Add(new Message()
                    { Content = msg.Msg, ChatRoom = msg.Chatroon, Chatter = msg.Chatter });
            }

        }

        public void Dispose()
        {
            _channelQAddress?.Dispose();
        }

        public void AddMessage(Message message)
        {
            _concurrentQueue.Enqueue(message);
        }
    }
}