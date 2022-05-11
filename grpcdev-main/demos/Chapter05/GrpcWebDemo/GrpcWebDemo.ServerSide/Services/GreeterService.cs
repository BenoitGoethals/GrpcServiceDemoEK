using Grpc.Core;
using GrpcWebDemo.ServerSide;

namespace GrpcWebDemo.ServerSide.Services {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name
            });
        }
        public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context) {
            await responseStream.WriteAsync(new HelloReply() { Message = "Hello " + request.Name });
            await Task.Delay(1000);
            await responseStream.WriteAsync(new HelloReply() { Message = "Nice to"});
            await Task.Delay(1000);
            await responseStream.WriteAsync(new HelloReply() { Message = "meet you!" });
        }
    }
}