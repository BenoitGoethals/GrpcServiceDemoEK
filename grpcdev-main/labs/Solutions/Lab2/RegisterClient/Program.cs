using System;
using Grpc.Net.Client;
using System.IO;
using System.Threading.Tasks;
using Lab2;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace RegisterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RegisterRequest request = new RegisterRequest();
            request.Name = "Info Support Kenniscentrum";
            request.Email = "kenniscentrum@infosupport.com";
            request.SubscriptionType = MemberType.Basic;
            request.Familymember.Add("Docent1");
            request.Familymember.Add("Receptie");
            FileStream fs = new FileStream("comic-character1.png", FileMode.Open);
            request.Picture = ByteString.FromStream(fs);
            fs.Close();            
            request.Birthdate = Timestamp.FromDateTime(new DateTime(1996, 01, 01).ToUniversalTime());
            using GrpcChannel grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
            try
            {
                Registration.RegistrationClient registrationClient = new Registration.RegistrationClient(grpcChannel);
                RegisterReply reply = registrationClient.Register(request);
                Console.WriteLine(reply.Welcome);
                Console.WriteLine(reply.Price);
                foreach (string s in reply.SubscriptionKey)
                    Console.WriteLine(s);
                Console.WriteLine(reply.ConfirmedAge.ToTimeSpan());
            }
            catch (RpcException re)
            {
                Console.WriteLine(re.Message);
            }
            finally
            {
                await grpcChannel.ShutdownAsync();
            }

        }
    }
}
