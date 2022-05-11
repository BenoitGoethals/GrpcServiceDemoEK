using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Lab2
{
    public class RegistrationService : Registration.RegistrationBase
    {
        private readonly ILogger<RegistrationService> _logger;
        public RegistrationService(ILogger<RegistrationService> logger)
        {
            _logger = logger;
        }

        public override Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context)
        {
            DateTime birthDate = request.Birthdate.ToDateTime();
            TimeSpan age = DateTime.Now.Date - birthDate.Date;
            if (DateTime.Now.Date.AddYears(-18) < birthDate.Date)
            {
                Status status = new Status(StatusCode.Internal, "You must be 18 years to register");
                throw new RpcException(status);
            }
            if (request.Familymember.Count > 4)
            {
                Status status = new Status(StatusCode.Internal, "Maximum of 5 family members exceeded");
                throw new RpcException(status);
            }
            RegisterReply reply = new RegisterReply();
            reply.Welcome = "Welcome to the family of " + request.Name;
            reply.Price = (uint)(request.SubscriptionType switch
            {
                MemberType.Basic => 10 + request.Familymember.Count * 1,
                MemberType.Gold => 15 + request.Familymember.Count * 2,
                MemberType.Platinum => 20 + request.Familymember.Count * 3,
                _ => 20 + request.Familymember.Count * 3
            });
            if (request.AddressTypeCase == RegisterRequest.AddressTypeOneofCase.Address)
                reply.Price += 5;
            FileStream fs = new FileStream(request.Name.Replace(' ', '_') + ".png", FileMode.Create);
            request.Picture.WriteTo(fs);
            fs.Close();
            Random rand = new Random();
            foreach (string s in request.Familymember)
                reply.SubscriptionKey.Add(s + rand.Next());
            reply.ConfirmedAge = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(age);
            return Task.FromResult(reply);
        }
    }
}
