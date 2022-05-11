using Lab03.ServerSide.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace Lab03.ServerSide.Tests.Core; 
public class CalculatorServiceTests {
    [Fact]
    public void Sum_ShouldReturn_SumOfInput() {
        long expected = 55;
        CalculatorService sut = new CalculatorService();
        long actual = sut.Sum(new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Factor_ShouldReturn_SumOfInput() {
        IEnumerable<long> expected = new long[] { 2,3,3,5,5,11 };
        CalculatorService sut = new CalculatorService();
        IEnumerable<long> actual = sut.PrimeFactors(4950);
        IEnumerable<Action<long>> inspectors = expected.Select<long,Action<long>>(
            expectedNumber => actualNumber => {
                Assert.Equal(expectedNumber, actualNumber);
            });
        Assert.Collection(actual, inspectors.ToArray());
        //Assert.Collection(actual, 
        //    actualNumber => {
        //        Assert.Equal(2, actualNumber);
        //    },
        //    actualNumber => {
        //        Assert.Equal(3, actualNumber);
        //    },
        //    actualNumber => {
        //        Assert.Equal(3, actualNumber);
        //    },
        //    actualNumber => {
        //        Assert.Equal(5, actualNumber);
        //    },
        //    actualNumber => {
        //        Assert.Equal(5, actualNumber);
        //    },
        //    actualNumber => {
        //        Assert.Equal(11, actualNumber);
        //    });
    }
}
