

using Grpc.Core;
using Grpc.Net.Client;
using ProtobufExample.ServerSide.Services;

using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7006");

ProtoBufExampleService.ProtoBufExampleServiceClient client = new ProtoBufExampleService.ProtoBufExampleServiceClient(channel);

await ScalarValueTypesExample(client);

await EnumerationExample(client);

await ImportExample(client);

await AnyExample(client);

await OneOfExample(client);

await MapExample(client);

await ExceptionExample(client);



Console.ReadLine();

static async Task ScalarValueTypesExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Scalar Value Types Example *************");
    Example01Response response = await client.Example01Async(new Example01Request());
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
    Console.WriteLine($"BytesField: {response.BytesField.ToStringUtf8()}");
    foreach (string item in response.RepeatedStringField) {
        Console.WriteLine($"\tRepeatedFieldItem: {item}");
    }
}

static async Task EnumerationExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Enumeration Example *************");
    Example02Response response = await client.Example02Async(new Example02Request() { Id = 1, EnumField = Example02Request.Types.EnumerationDefinition.SecondOption });
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
    Console.WriteLine($"EnumField: {response.EnumField.ToString()}");
}

static async Task ImportExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Import Example *************");
    Example03Request request = new Example03Request() {
        Id = 1,
        ImportedField = new Example03Message() {
            Id = 1,
            IsItTrue = true,
            Name = "Imported Field Name"
        },
        TimestampField = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
    };
    Example03Response response = await client.Example03Async(request);
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
    TimeSpan timeSpan = response.DurationField.ToTimeSpan();
    Console.WriteLine($"DurationField: days {timeSpan.Days}, hours {timeSpan.Hours}, minutes {timeSpan.Minutes}, seconds {timeSpan.Seconds}");
}

static async Task AnyExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Any Example *************");
    Example04Request request = new Example04Request() {
        Id = 1,
        AnyField = Google.Protobuf.WellKnownTypes.Any.Pack(new Example03Message() {
            Id = 1,
            IsItTrue = true,
            Name = "Any Field Name"
        })
    };
    Example04Response response = await client.Example04Async(request);
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

    Example03Message message = response.AnyField.Unpack<Example03Message>();
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(message));
}

static async Task OneOfExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* OneOf Example *************");
    Example05Request example05Request = new Example05Request() {
        OrThisOne = new OrThisOne() {
            Id = 1
        }
    };
    Example05Response response = await client.Example05Async(example05Request);
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

    switch (response.OneofFieldCase) {
        case Example05Response.OneofFieldOneofCase.None:
            Console.WriteLine("The server did not set anything in the OneOfField");
            break;
        case Example05Response.OneofFieldOneofCase.ThisOne:
            Console.WriteLine($"The server set the OneOfField to ThisOne: {response.ThisOne.StringField}");
            break;
        case Example05Response.OneofFieldOneofCase.OrThisOne:
            Console.WriteLine($"The server set the OneOfField to OrThisOne: {response.OrThisOne.Id}");
            break;
    }
}

static async Task MapExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Map Example *************");
    Example06Request request = new Example06Request();
    request.MapOfMessages.Add(new Dictionary<string, Example03Message>() {
    {"First Key", new Example03Message(){Id = 1, IsItTrue = true, Name = "First Message" } },
    {"Second Key", new Example03Message(){Id = 2, IsItTrue = false, Name = "Second Message" } },
    {"Third Key", new Example03Message(){Id = 3, IsItTrue = true, Name = "Third Message" } },
});
    Example06Response response = await client.Example06Async(request);
    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));

    foreach (var item in response.MapField) {
        Console.WriteLine($"\tMap Key: {item.Key} - Map Item: {item.Value}");
    }
}

static async Task ExceptionExample(ProtoBufExampleService.ProtoBufExampleServiceClient client) {
    Console.WriteLine("********* Exception Example *************");
    try {
        Example07Response response = await client.Example07Async(new Example07Request());
    } catch (RpcException ex) when (ex.StatusCode == StatusCode.Unimplemented) {
        Console.WriteLine($"{ex.GetType().Name} - {ex.StatusCode} - {ex.Status.Detail}");
    } catch (Exception ex) {
        Console.WriteLine($"{ex.GetType().Name} - {ex.Message}");
    }
}