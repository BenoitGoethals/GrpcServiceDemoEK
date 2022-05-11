namespace GrpcWebDemo.BlazorWebClient.Core.Interfaces; 
public interface IGreeterService {
    Task<string> Greet(string name);
    IAsyncEnumerable<string> GreetStream(string name);
}
