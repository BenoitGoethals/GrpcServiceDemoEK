using Lab02.Server.Core;
using Lab02.Server.Core.Interfaces;
using Moq;
using System;
using Xunit;

namespace Lab02.Server.Tests.Core;
public class AgeCalculatorTests {
    [Fact]
    public void CalculateAge_ReturnsAgeInYears_GivenBirthDate() { 
        Mock<IDateProvider> dateProvider = new Mock<IDateProvider>();
        dateProvider.SetupGet(dp=>dp.Now).Returns(new System.DateTime(2000,1,1));
        DateTime birthDate = new DateTime(1900, 1, 1);
        int expected = 100;

        AgeCalculator ageCalculator = new AgeCalculator(dateProvider.Object);

        int actual = ageCalculator.CalculateAge(birthDate);

        Assert.Equal(expected, actual);
    }
}
