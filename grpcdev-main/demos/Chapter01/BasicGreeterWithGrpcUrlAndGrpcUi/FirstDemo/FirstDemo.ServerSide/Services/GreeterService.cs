//https://docs.microsoft.com/en-us/aspnet/core/grpc/test-tools?view=aspnetcore-6.0
//C:\CM\Web\gRpcDev>
//grpcurl -proto "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos\greet.proto" -import-path "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos" describe
//grpcurl -proto "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos\greet.proto" -import-path "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos" list
//grpcurl -proto "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos\greet.proto" -import-path "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos" -d "{\"name\":\"Simona\"}" localhost:7056 greet.Greeter.SayHello 

//grpcui -proto "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos\greet.proto" -import-path "C:\grpcdev\demos\first\FirstDemo\FirstDemo.ServerSide\Protos" localhost:7056 

//after adding reflection:
//grpcurl -d "{\"name\":\"Simona\"}" localhost:7056 greet.Greeter.SayHello
//grpcui localhost:7056

using FirstDemo.ServerSide;
using Grpc.Core;

namespace FirstDemo.ServerSide.Services {
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
    }
}