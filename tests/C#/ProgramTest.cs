using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ProgramTests
{
    [TestMethod]
    public void TestEvenNumbersExtraction_WithMixedNumbers_ReturnsOnlyEvenNumbers()
    {
        // Arrange
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var expectedEvenNumbers = new List<int> { 2, 4 };

        // Act
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

        // Assert
        CollectionAssert.AreEqual(expectedEvenNumbers, evenNumbers, "The even numbers extracted do not match the expected values.");
    }

    [TestMethod]
    public void TestEvenNumbersExtraction_WithAllOddNumbers_ReturnsEmptyList()
    {
        // Arrange
        var numbers = new List<int> { 1, 3, 5, 7, 9 };

        // Act
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

        // Assert
        Assert.AreEqual(0, evenNumbers.Count, "The even numbers list should be empty for an all-odd numbers list.");
    }

    [TestMethod]
    public void TestSumCalculation_WithPositiveNumbers_ReturnsCorrectSum()
    {
        // Arrange
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var expectedSum = 15;

        // Act
        var sum = numbers.Sum();

        // Assert
        Assert.AreEqual(expectedSum, sum, "The calculated sum does not match the expected sum.");
    }

    [TestMethod]
    public void TestSumCalculation_WithEmptyList_ReturnsZero()
    {
        // Arrange
        var numbers = new List<int>();

        // Act
        var sum = numbers.Sum();

        // Assert
        Assert.AreEqual(0, sum, "The sum of an empty list should be zero.");
    }

    [TestMethod]
    public void TestAverageCalculation_WithPositiveNumbers_ReturnsCorrectAverage()
    {
        // Arrange
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var expectedAverage = 3.0;

        // Act
        var average = numbers.Average();

        // Assert
        Assert.AreEqual(expectedAverage, average, "The calculated average does not match the expected average.");
    }

    [TestMethod]
    public void TestAverageCalculation_WithEmptyList_ThrowsInvalidOperationException()
    {
        // Arrange
        var numbers = new List<int>();

        // Act & Assert
        Assert.ThrowsException<InvalidOperationException>(() => numbers.Average(), "Calculating the average of an empty list should throw an InvalidOperationException.");
    }
}