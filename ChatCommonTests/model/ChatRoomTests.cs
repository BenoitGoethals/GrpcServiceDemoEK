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
            var chatter = new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "benoit" };
            var chatter1 = new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "ilse" };
            var chatter2 = new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "liv" };
            chatRoom.AddChatter(chatter);
            chatRoom.AddChatter(chatter1);
            chatRoom.AddChatter(chatter2);


            chatRoom.AddMessage(new Message(){ChatRoom=chatRoom,Content="loilsel",Id=Guid.NewGuid(),Chatter=chatter2});
            chatRoom.AddMessage(new Message() { ChatRoom = chatRoom, Content = "loilselfdffs", Id = Guid.NewGuid(), Chatter = chatter2 });
            chatRoom.AddMessage(new Message() { ChatRoom = chatRoom, Content = "loilsedsfdsfdsfdsfdsl", Id = Guid.NewGuid(), Chatter = chatter2 });
            chatRoom.AddMessage(new Message() { ChatRoom = chatRoom, Content = "loilsedsfdsfdsfdsfdsl", Id = Guid.NewGuid(), Chatter = chatter });
            chatRoom.AddMessage(new Message() { ChatRoom = chatRoom, Content = "ddddd", Id = Guid.NewGuid(), Chatter = chatter });
            chatRoom.GetAllMessages().Count.Should().Be(5);
            chatRoom.GetAllMessages().Count(i => i.Chatter.Equals(chatter2)).Should().Be(3);
            chatRoom.GetAllMessages().Count(i => i.ChatRoom.Equals(chatRoom)).Should().Be(5);
        }


        [Fact()]
        public void AddChatterTest()
        {
            ChatRoom chatRoom = new ChatRoom();
            chatRoom.AddChatter(new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "benoit" });
            chatRoom.AddChatter(new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "ilse" });
            chatRoom.AddChatter(new Chatter() { Guid = Guid.NewGuid(), Enabled = true, Name = "liv" });
            chatRoom.GetChatters().Count.Should().Be(3);
        }
    }
}