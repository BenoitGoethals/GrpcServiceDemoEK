//https://developers.google.com/protocol-buffers/docs/proto3
//https://docs.microsoft.com/en-us/aspnet/core/grpc/protobuf?view=aspnetcore-6.0
//https://docs.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/error-handling
using Grpc.Core;

namespace ProtobufExample.ServerSide.Services {
    public class ProtoExampleServiceImplementation : ProtoBufExampleService.ProtoBufExampleServiceBase {
        private readonly ILogger<ProtoExampleServiceImplementation> logger;

        public ProtoExampleServiceImplementation(ILogger<ProtoExampleServiceImplementation> logger) {
            this.logger = logger;
        }
        public override Task<Example01Response> Example01(Example01Request request, ServerCallContext context) {
            logger.LogInformation("Example 1: Scalar Value Types");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            Example01Response response = new Example01Response() { 
                DoubleField = double.MaxValue,
                FloatField = float.MaxValue,
                //https://stackoverflow.com/questions/765916/is-there-ever-a-good-time-to-use-int32-instead-of-sint32-in-google-protocol-buff
                Int32Field = int.MinValue,
                Int64Field = long.MinValue,
                Uint32Field = uint.MaxValue,
                Uint64Field = ulong.MaxValue,
                Sint32Field = int.MinValue,
                Sint64Field = long.MinValue,
                //https://developers.google.com/protocol-buffers/docs/encoding
                Fixed32Field = uint.MinValue,
                Fixed64Field = ulong.MinValue,
                Sfixed32Field = int.MinValue,
                Sfixed64Field = long.MinValue,

                BoolField = true,
                StringField = string.Empty,
                BytesField = Google.Protobuf.ByteString.CopyFromUtf8("Hi")
            };
            response.RepeatedStringField.Add("First item in the repeated string field");
            response.RepeatedStringField.AddRange(new[] {"Second item in the repeated string field", "Third item in the repeated string field"});
            return Task.FromResult(response);
        }

        public override Task<Example02Response> Example02(Example02Request request, ServerCallContext context) {
            logger.LogInformation("Example 2: Enumerations");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            logger.LogInformation(request.EnumField.ToString());
            Example02Response res = new Example02Response() {
                Id = 1,
                EnumField = EnumerationWithAliases.AliasForFirstOption
            };
            return Task.FromResult(res);
        }

        public override Task<Example03Response> Example03(Example03Request request, ServerCallContext context) {
            logger.LogInformation("Example 3: Import");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            logger.LogInformation(request.ImportedField.Id.ToString());
            logger.LogInformation(request.ImportedField.Name);
            logger.LogInformation(request.ImportedField.IsItTrue.ToString());
            logger.LogInformation(request.TimestampField.ToDateTime().ToLongDateString());

            var response = new Example03Response() { 
                Id = 1, 
                DurationField = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(new TimeSpan(days: 3, hours: 5, minutes: 7, seconds: 9)) 
            };
            return Task.FromResult(response);
        }

        public override Task<Example04Response> Example04(Example04Request request, ServerCallContext context) {
            logger.LogInformation("Example 4: Any");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            logger.LogInformation("Type Name Of The Any Field: " + Google.Protobuf.WellKnownTypes.Any.GetTypeName(request.AnyField.TypeUrl));
            logger.LogInformation("Any Field is of type Example03Message: " + request.AnyField.Is(Example03Message.Descriptor));
            Example03Message message = request.AnyField.Unpack<Example03Message>();
            logger.LogInformation($"\t Id: {message.Id}");
            logger.LogInformation($"\t Name: {message.Name}");
            logger.LogInformation($"\t IsItTrue: {message.IsItTrue}");

            Example04Response response = new Example04Response() { 
                Id=1,
                AnyField = Google.Protobuf.WellKnownTypes.Any.Pack(new Example03Message() {Id = message.Id, IsItTrue = message.IsItTrue, Name = message.Name })
            };
            return Task.FromResult(response);
        }

        public override Task<Example05Response> Example05(Example05Request request, ServerCallContext context) {
            logger.LogInformation("Example 5: OneOf");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            switch (request.OneofFieldCase) {
                case Example05Request.OneofFieldOneofCase.None:
                    logger.LogInformation("No OneOfField has been set by the client");
                    break;
                case Example05Request.OneofFieldOneofCase.ThisOne:
                    logger.LogInformation($"OneOfField Has been set by the client as ThisOne: {request.ThisOne.StringField}");
                    break;
                case Example05Request.OneofFieldOneofCase.OrThisOne:
                    logger.LogInformation($"OneOfField Has been set by the client as OrThisOne: {request.OrThisOne.Id}");
                    break;
            }

            Example05Response response = new Example05Response() { 
                ThisOne = new EitherThisOne() { 
                    StringField = "This One Field Value"
                }
            };
            return Task.FromResult(response);
        }

        public override Task<Example06Response> Example06(Example06Request request, ServerCallContext context) {
            logger.LogInformation("Example 6: Map");
            logger.LogInformation(System.Text.Json.JsonSerializer.Serialize(request));
            foreach (var item in request.MapOfMessages) {
                logger.LogInformation($"\tKey: {item.Key} - Value: {item.Value.Id}, {item.Value.IsItTrue}, {item.Value.Name}");
            }
            Example06Response response = new Example06Response();
            response.MapField.Add(new Dictionary<int, string>() { 
                {1, "First Entry" },
                {2, "Second Entry" },
                {3, "Third Entry" }
            });
            return Task.FromResult(response);
        }

        public override Task<Example07Response> Example07(Example07Request request, ServerCallContext context) {
            /*
                Status code	Problem
                GRPC_STATUS_UNIMPLEMENTED	Method hasn't been written.
                GRPC_STATUS_UNAVAILABLE	Problem with the whole service.
                GRPC_STATUS_UNKNOWN	Invalid response.
                GRPC_STATUS_INTERNAL	Problem with encoding/decoding.
                GRPC_STATUS_UNAUTHENTICATED	Authentication failed.
                GRPC_STATUS_PERMISSION_DENIED	Authorization failed.
                GRPC_STATUS_CANCELLED	Call was canceled, usually by the caller.
            */
            throw new RpcException(new Status(StatusCode.Unimplemented, "This method has not been implemented yet, please come back later, we're working on it"));
        }
    }
}
