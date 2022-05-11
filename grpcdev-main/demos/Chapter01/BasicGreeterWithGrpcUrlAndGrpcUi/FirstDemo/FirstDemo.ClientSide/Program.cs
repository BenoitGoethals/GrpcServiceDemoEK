
using FirstDemo.ServerSide;
using Grpc.Net.Client;


GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7005");
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

HelloReply? response = await client.SayHelloAsync(new HelloRequest { Name= "hallo man"});

Console.WriteLine(response.Message);

Console.ReadLine();

