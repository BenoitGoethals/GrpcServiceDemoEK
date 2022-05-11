using ConfigurationDemo.ServerSide;
using Grpc.Core;

namespace ConfigurationDemo.ServerSide.Services {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            _logger.LogInformation($"SayHello invoked with {request.Name}");
            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name
            });
        }

        public override Task<UploadPictureReply> UploadPicture(UploadPictureRequest request, ServerCallContext context) {
            _logger.LogInformation($"UploadPicture invoked with a {request.Picture.Length / (1024 * 1024)} MB file");
            return Task.FromResult(new UploadPictureReply() { Id = request.Id, Picture = request.Picture, ContentType = request.ContentType });
        }
    }
}