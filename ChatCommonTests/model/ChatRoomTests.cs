using System;
using System.Linq;
using ChatCommon.model;
using FluentAssertions;
using Xunit;

namespace ChatCommonTests.model
{
    public class ChatRoomTests
    {
        [Fact()]
        public void AddMessageTest()
        {
            ChatRoom chatRoom = new ChatRoom();
            


            chatRoom.AddMessage(new Message(){ChatRoom="sdfdsfds",Content="loilsel",Id=Guid.NewGuid(),Chatter="dsfdsfds"});
            chatRoom.AddMessage(new Message() { ChatRoom = "sdfsdf", Content = "loilselfdffs", Id = Guid.NewGuid(), Chatter = "sdfds" });
            chatRoom.AddMessage(new Message() { ChatRoom = "sdfsdf", Content = "loilsedsfdsfdsfdsfdsl", Id = Guid.NewGuid(), Chatter = "sdfds" });
            chatRoom.AddMessage(new Message() { ChatRoom = "sdfsdf", Content = "loilsedsfdsfdsfdsfdsl", Id = Guid.NewGuid(), Chatter = "sdfds" });
            chatRoom.AddMessage(new Message() { ChatRoom = "sdfds", Content = "ddddd", Id = Guid.NewGuid(), Chatter = "sdfdsf" });
            chatRoom.GetAllMessages().Count.Should().Be(5);
            chatRoom.GetAllMessages().Count(i => i.Chatter.Equals("sdfsdf")).Should().Be(0);
        //    chatRoom.GetAllMessages().Count(i => i.ChatRoom.Equals("sdfds")).Should().Be(3);
        }


    }
}