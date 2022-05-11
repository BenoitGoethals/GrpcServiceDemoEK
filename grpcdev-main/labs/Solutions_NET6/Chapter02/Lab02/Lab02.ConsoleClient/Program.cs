using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Lab02.Server.Services;

using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7064");


RegistrationService.RegistrationServiceClient serviceClient = new RegistrationService.RegistrationServiceClient(channel);

await CorrectData(serviceClient);
await IncorrectData(serviceClient);

Console.ReadLine();

async Task CorrectData(RegistrationService.RegistrationServiceClient serviceClient) {
    RegisterRequest request = new RegisterRequest() {
        Name = "Super Mario",
        BirthDate = Timestamp.FromDateTime(new DateTime(1980, 10, 10).ToUniversalTime()),
        Address = new Address() {
            City = "Tokyo",
            Number = 12,
            Street = "Street",
            Zip = "1234"
        },
        SubscriptionType = MemberType.Gold,
    };

    request.FamilyMembers.Add("Peach");
    request.FamilyMembers.Add("Luigi");

    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\..\..\Starter\Lab2\comic-character2.png");
    request.Picture = await Google.Protobuf.ByteString.FromStreamAsync(File.OpenRead(filePath));

    RegisterReply reply = await serviceClient.RegisterAsync(request);

    PrintReply(reply);
    
}

async Task IncorrectData(RegistrationService.RegistrationServiceClient serviceClient) {
    RegisterRequest request = new RegisterRequest() {
        Name = "Super Mario",
        BirthDate = Timestamp.FromDateTime(DateTime.Now.AddYears(10).ToUniversalTime()),
        Email = "supermario@nintendo.jp",
        SubscriptionType = MemberType.Basic
    };

    request.FamilyMembers.Add("Peach");
    request.FamilyMembers.Add("Luigi");
    request.FamilyMembers.Add("Daisy");
    request.FamilyMembers.Add("Wario");
    request.FamilyMembers.Add("Waluigi");
    request.FamilyMembers.Add("Toad");
    request.FamilyMembers.Add("Yoshi");

    try {
        RegisterReply reply = await serviceClient.RegisterAsync(request);
        PrintReply (reply);
    } catch (RpcException ex) {
        Console.WriteLine("ERROR(S) FOUND: " + ex.Message);
        foreach (var item in ex.Trailers) {
            Console.WriteLine($"\t{item.Key}: {item.Value}");
        }
    }
}

void PrintReply(RegisterReply reply) {
    Console.WriteLine(reply.Welcome);
    Console.WriteLine($"Total price: {reply.Price}");
    Console.WriteLine($"Confirmed Age: {reply.ConfirmedAge}");
    Console.WriteLine($"Generated number of keys: {reply.SubscriptionKeys.Count}");
    foreach (var item in reply.SubscriptionKeys) {
        Console.WriteLine("\tKey: " + item);
    }
}