using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceUsers.Protos;
using Microsoft.Extensions.Configuration;

namespace UserClienAuth
{
    class Program
    {
        private const string Address = "localhost:5001";
        private static string _token;
   
        static async Task Main(string[] args)
        {

            _token = await Authenticate();
            var channel = CreateAuthenticatedChannel($"https://{Address}");
           
            var client = new UsersSecurity.UsersSecurityClient(channel);
            var listUsers = client.GetUsersAsync(new FilterActive() { Active = true });
            var userGui = listUsers.ResponseAsync.Result.User.Last().Guid;
            var last = client.GetUserAsync(new Filter() { Guid = userGui });
            Console.WriteLine(last.ResponseAsync.Result);
            Console.ReadKey();


        }


        private static async Task<string> Authenticate()
        {
            Console.WriteLine($"Authenticating as {Environment.UserName}...");
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://{Address}/generateJwtToken?name={HttpUtility.UrlEncode(Environment.UserName)}"),
                Method = HttpMethod.Get,
                Version = new Version(2, 0)
            };
            var tokenResponse = await httpClient.SendAsync(request);
            tokenResponse.EnsureSuccessStatusCode();

            var token = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine("Successfully authenticated.");

            return token;
        }

        private static GrpcChannel CreateAuthenticatedChannel(string address)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token))
                {
                    metadata.Add("Authorization", $"Bearer {_token}");
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // Channels that aren't using TLS should use ChannelCredentials.Insecure instead.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }
      
    }
}
