using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Grpc.Core;
using GrpcServiceUsers;
using GrpcServiceUsers.Protos;
using GrpcServiceUsers.Services;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using Xunit;
using XUnitTestProjectServiceUsers.Fixture;
using CallContext = ProtoBuf.Grpc.CallContext;

namespace XUnitTestProjectServiceUsers
{
  
    public class ServiceTest : FunctionalTestBase
    {
      
        public ServiceTest(GrpcServerFactory<Startup> factory) : base(factory)
        {
            
        }


        [Fact]
        public void GetAllUsers()
        {
            var client = new Greeter.GreeterClient(_channel);
            var rt=client.SayHello(new HelloRequest() { Name = "benoit"});
            rt.Message.Should().Contain("benoit");



    }

    }
}
