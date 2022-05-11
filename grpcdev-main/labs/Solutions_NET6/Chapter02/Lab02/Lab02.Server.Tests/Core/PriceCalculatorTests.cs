using Lab02.Server.Core;
using Lab02.Server.Services;
using System;
using Xunit;

namespace Lab02.Server.Tests.Core;
public class PriceCalculatorTests {
    [Theory]
    [InlineData(MemberType.Basic, 10)]
    [InlineData(MemberType.Gold, 15)]
    [InlineData(MemberType.Platinum, 20)]
    public void BasicPrice_ReturnsNumber_GivenMembershipType(MemberType memberType, uint expected) {
        PriceCalculator priceCalculator = new();
        uint actual = priceCalculator.BasicPrice(memberType);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void BasicPrice_Throws_WhenMemberTypeUnspecified() {
        PriceCalculator priceCalculator = new();
        Assert.Throws<ArgumentException>(() => { 
            uint actual = priceCalculator.BasicPrice(MemberType.Unspecified);
        });
    }

    [Theory]
    [InlineData(MemberType.Basic, 1)]
    [InlineData(MemberType.Gold, 2)]
    [InlineData(MemberType.Platinum, 3)]
    public void PriceMultiplier_ReturnsNumber_GivenMembershipType(MemberType memberType, uint expected) {
        PriceCalculator priceCalculator = new();
        uint actual = priceCalculator.PriceMultiplier(memberType);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void PriceMultiplier_Throws_WhenMemberTypeUnspecified() {
        PriceCalculator priceCalculator = new();
        Assert.Throws<ArgumentException>(() => {
            uint actual = priceCalculator.PriceMultiplier(MemberType.Unspecified);
        });
    }

    [Theory]
    [InlineData(MemberType.Basic, 0, 10)]
    [InlineData(MemberType.Basic, 1, 11)]
    [InlineData(MemberType.Basic, 2, 12)]
    [InlineData(MemberType.Basic, 3, 13)]
    [InlineData(MemberType.Gold, 0, 15)]
    [InlineData(MemberType.Gold, 1, 17)]
    [InlineData(MemberType.Gold, 2, 19)]
    [InlineData(MemberType.Gold, 3, 21)]
    [InlineData(MemberType.Platinum, 0, 20)]
    [InlineData(MemberType.Platinum, 1, 23)]
    [InlineData(MemberType.Platinum, 2, 26)]
    [InlineData(MemberType.Platinum, 3, 29)]
    public void PriceWithoutShipping_CalculatesPriceWithoutShippingCosts(MemberType memberType, uint familyMembers, uint expected) {
        PriceCalculator priceCalculator = new();
        uint actual = priceCalculator.PriceWithoutShipping(memberType, familyMembers);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(MemberType.Basic, 0, RegisterRequest.ContactOnOneofCase.Email, 10)]
    [InlineData(MemberType.Basic, 0, RegisterRequest.ContactOnOneofCase.Address, 15)]
    [InlineData(MemberType.Basic, 1, RegisterRequest.ContactOnOneofCase.Email, 11)]
    [InlineData(MemberType.Basic, 1, RegisterRequest.ContactOnOneofCase.Address, 16)]
    [InlineData(MemberType.Basic, 2, RegisterRequest.ContactOnOneofCase.Email, 12)]
    [InlineData(MemberType.Basic, 2, RegisterRequest.ContactOnOneofCase.Address, 17)]
    [InlineData(MemberType.Basic, 3, RegisterRequest.ContactOnOneofCase.Email, 13)]
    [InlineData(MemberType.Basic, 3, RegisterRequest.ContactOnOneofCase.Address, 18)]
    [InlineData(MemberType.Gold, 0, RegisterRequest.ContactOnOneofCase.Email, 15)]
    [InlineData(MemberType.Gold, 0, RegisterRequest.ContactOnOneofCase.Address, 20)]
    public void FinalPrice_CalculatesPriceWithShippingCosts(MemberType memberType, uint familyMembers, RegisterRequest.ContactOnOneofCase contactOn, uint expected) {
        PriceCalculator priceCalculator = new();
        uint actual = priceCalculator.FinalPrice(memberType, familyMembers, contactOn);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(MemberType.Basic, 0, RegisterRequest.ContactOnOneofCase.Email, 10)]
    [InlineData(MemberType.Basic, 0, RegisterRequest.ContactOnOneofCase.Address, 15)]
    [InlineData(MemberType.Basic, 1, RegisterRequest.ContactOnOneofCase.Email, 11)]
    [InlineData(MemberType.Basic, 1, RegisterRequest.ContactOnOneofCase.Address, 16)]
    [InlineData(MemberType.Basic, 2, RegisterRequest.ContactOnOneofCase.Email, 12)]
    [InlineData(MemberType.Basic, 2, RegisterRequest.ContactOnOneofCase.Address, 17)]
    [InlineData(MemberType.Basic, 3, RegisterRequest.ContactOnOneofCase.Email, 13)]
    [InlineData(MemberType.Basic, 3, RegisterRequest.ContactOnOneofCase.Address, 18)]
    [InlineData(MemberType.Gold, 0, RegisterRequest.ContactOnOneofCase.Email, 15)]
    [InlineData(MemberType.Gold, 0, RegisterRequest.ContactOnOneofCase.Address, 20)]
    public void FinalPrice_Request_CalculatesPriceWithShippingCosts(MemberType memberType, uint familyMembers, RegisterRequest.ContactOnOneofCase contactOn, uint expected) {
        RegisterRequest request = new RegisterRequest();
        request.SubscriptionType = memberType;
        for (int i = 0; i < familyMembers; i++) {
            request.FamilyMembers.Add("");
        }
        if (contactOn == RegisterRequest.ContactOnOneofCase.Address) {
            request.Address = new Address();
        } else {
            request.Email = "";
        }
        PriceCalculator priceCalculator = new();
        uint actual = priceCalculator.FinalPrice(request);
        Assert.Equal(expected, actual);
    }
}
