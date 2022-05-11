using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Lab02.Server.Core.Interfaces;
using Lab02.Server.Core.Validators;

namespace Lab02.Server.Services {
    public class RegistrationServiceImplementation : RegistrationService.RegistrationServiceBase {
        private readonly RegisterRequestValidator validationRules;
        private readonly IPriceCalculator priceCalculator;
        private readonly IPictureRepository pictureRepository;
        private readonly IKeysGenerator keysGenerator;

        public RegistrationServiceImplementation(RegisterRequestValidator validationRules, IPriceCalculator priceCalculator, IPictureRepository pictureRepository, IKeysGenerator keysGenerator) {
            this.validationRules = validationRules;
            this.priceCalculator = priceCalculator;
            this.pictureRepository = pictureRepository;
            this.keysGenerator = keysGenerator;
        }
        public override Task<RegisterReply> Register(RegisterRequest request, ServerCallContext context) {
            var result = validationRules.Validate(request);
            if (!result.IsValid) {
                Metadata metadata = new Metadata();
                foreach (var item in result.Errors) {
                    metadata.Add(item.PropertyName, item.ErrorMessage);
                }
                throw new RpcException(new Status(StatusCode.Internal, "Invalid Request"),metadata);
            }
            
            pictureRepository.SavePicture(request);

            RegisterReply response = new RegisterReply();
            response.Price = priceCalculator.FinalPrice(request);
            response.ConfirmedAge = new Duration() { Seconds = request.BirthDate.Seconds};
            response.SubscriptionKeys.AddRange(keysGenerator.GenerateKeys(request));
            response.Welcome = $"Welcome {request.Name}";

            return Task.FromResult(response);
        }
    }
}
