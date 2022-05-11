using Grpc.Core;
using Lab01.ServerSide;
using System.Text;

namespace Lab01.ServerSide.Services {
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
        public override async Task<HelloReply> ClientStreaming(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context) {
            StringBuilder builder = new StringBuilder();
            await foreach (var msg in requestStream.ReadAllAsync()) {
                builder.Append(msg.Name + ",");
            }
            return new HelloReply { Message = builder.ToString() };
        }
        public override async Task ServerStreaming(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context) {
            for (int i = 0; i < 10; i++) {
                await responseStream.WriteAsync(new HelloReply() { Message = $"Hi {request.Name} {i}" });
                await Task.Delay(10);
            }
        }
        public override Task Duplex(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context) {
            Parallel.Invoke(
                async () => {
                    StringBuilder builder = new StringBuilder();
                    await foreach (var msg in requestStream.ReadAllAsync()) {
                        builder.Append(msg.Name + ",");
                    }
                },
                async () => {
                    for (int i = 0; i < 10; i++) {
                        try {
                            await responseStream.WriteAsync(new HelloReply() { Message = $"Hi {i}" });
                            await Task.Delay(10);
                        } catch (Exception ex) {
                            _logger.LogError(ex, "nope");
                        }
                    }
                }
            );
            return Task.CompletedTask;
        }
    }
}