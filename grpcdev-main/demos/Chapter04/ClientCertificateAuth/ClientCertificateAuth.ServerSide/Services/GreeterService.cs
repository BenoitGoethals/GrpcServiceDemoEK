using ClientCertificateAuth.ServerSide;
using Grpc.Core;
using System.Security.Claims;

namespace ClientCertificateAuth.ServerSide.Services {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            ClaimsPrincipal? user = context.GetHttpContext().User;
            _logger.LogInformation($"{user?.Identity?.Name} {user?.Identity?.AuthenticationType}");
            foreach (var claim in user?.Claims) {
                _logger.LogInformation($"\t{claim.Type} - {claim.Value}");
            }
            return Task.FromResult(new HelloReply {
                Message = "Hello " + user?.Identity?.Name
            });
        }
    }
}