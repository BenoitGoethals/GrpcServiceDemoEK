using Xunit;
using Moq;
using Lab02.Server.Core.Validators;
using Lab02.Server.Core.Interfaces;
using Lab02.Server.Services;
using Grpc.Core;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;

namespace Lab02.Server.Tests.Services; 
public class RegistrationServiceImplementationTests {
    [Fact]
    public async Task Register_Returns_Reply() {
        uint expectedPrice = 30;
        uint expectedConfirmedAgeInSeconds = 10;
        List<string> expectedKeys = new List<string>() {
            "Main123", "One345"
        };
        Mock<IAgeCalculator> mockCalculator = new Mock<IAgeCalculator>();
        Mock<IPriceCalculator> mockPriceCalculator = new Mock<IPriceCalculator>();
        Mock<IPictureRepository> mockPictureRepository = new Mock<IPictureRepository>();
        Mock<IKeysGenerator> mockKeysGenerator = new Mock<IKeysGenerator>();
        Mock<ServerCallContext> mockServerCallContext = new Mock<ServerCallContext>();

        RegisterRequest registerRequest = new RegisterRequest();
        registerRequest.Name = "string";
        registerRequest.BirthDate = new Google.Protobuf.WellKnownTypes.Timestamp() { Seconds = expectedConfirmedAgeInSeconds };
        registerRequest.FamilyMembers.Add("One");
        
        mockCalculator.Setup(a => a.CalculateAge(It.IsAny<DateTime>())).Returns(20);
        mockPriceCalculator.Setup(p=>p.FinalPrice(registerRequest)).Returns(expectedPrice);
        mockKeysGenerator.Setup(k => k.GenerateKeys(registerRequest)).Returns(expectedKeys);
        RegistrationServiceImplementation sut = new RegistrationServiceImplementation(new RegisterRequestValidator(mockCalculator.Object),mockPriceCalculator.Object,mockPictureRepository.Object,mockKeysGenerator.Object);

        RegisterReply? actual = await sut.Register(registerRequest, mockServerCallContext.Object);

        Assert.Equal(expectedPrice, actual.Price);
        Assert.Equal(expectedConfirmedAgeInSeconds, actual.ConfirmedAge.Seconds);
        Assert.Equal(2, actual.SubscriptionKeys.Count);
        Assert.Collection(actual.SubscriptionKeys,
            item => Assert.Equal("Main123", item),
            item => Assert.Equal("One345", item)
        );
        Assert.Equal("Welcome string", actual.Welcome);
    }
}
