using Grpc.Net.Client;
using Grpc.Core;
using Lab03.ServerSide;

GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7093");
Calculator.CalculatorClient client = new Calculator.CalculatorClient(channel);

await Excercise01(client);
Excercise02(client);
Console.ReadLine();
async Task Excercise01(Calculator.CalculatorClient client) {
    var SumClientCall = client.Sum();
    for (int i = 0; i < 100; i++) {
        await SumClientCall.RequestStream.WriteAsync(new SumRequest() { Number = i });
    }
    await SumClientCall.RequestStream.CompleteAsync();

    var reply = await SumClientCall;
    Console.WriteLine("The sum of the numbers between 0 and 99 is " + reply.Number);
}

void Excercise02(Calculator.CalculatorClient client) {
    var SumClientCall = client.PrimeFactors();
    Parallel.Invoke(
        async () => {
            for (int i = 0; i < 100; i++) {
                await SumClientCall.RequestStream.WriteAsync(new PrimeFactorsRequest() { Number = i });
            }
            await SumClientCall.RequestStream.CompleteAsync();
        },

        async () => {
            await foreach(var item in SumClientCall.ResponseStream.ReadAllAsync()) {
                Console.WriteLine("Received prime factor " + item.Number);
            }
        });
}
