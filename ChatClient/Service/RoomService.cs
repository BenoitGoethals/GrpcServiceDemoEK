using ChatCommon.model;

namespace ChatClient.Service
{
    public class RoomService
    {
        private ChatRoom _chatRoom;

        public RoomService(ChatRoom chatRoom)
        {
            _chatRoom = chatRoom;
        }


        public ChatRoom GetChatRoom()
        {
            return _chatRoom;
        }

    }
}