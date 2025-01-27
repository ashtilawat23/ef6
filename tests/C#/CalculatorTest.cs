using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo;

namespace DemoTests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator calculator;

        [TestInitialize]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [TestMethod]
        public void Add_TwoPositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange
            double a = 5;
            double b = 10;

            // Act
            double result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(15, result, "Adding 5 and 10 should return 15");
        }

        [TestMethod]
        public void Add_PositiveAndNegativeNumber_ReturnsCorrectSum()
        {
            // Arrange
            double a = 5;
            double b = -3;

            // Act
            double result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(2, result, "Adding 5 and -3 should return 2");
        }

        [TestMethod]
        public void Subtract_TwoNumbers_ReturnsCorrectDifference()
        {
            // Arrange
            double a = 10;
            double b = 5;

            // Act
            double result = calculator.Subtract(a, b);

            // Assert
            Assert.AreEqual(5, result, "Subtracting 5 from 10 should return 5");
        }

        [TestMethod]
        public void Multiply_TwoNumbers_ReturnsCorrectProduct()
        {
            // Arrange
            double a = 4;
            double b = 5;

            // Act
            double result = calculator.Multiply(a, b);

            // Assert
            Assert.AreEqual(20, result, "Multiplying 4 and 5 should return 20");
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            // Arrange
            double a = 10;
            double b = 0;

            // Act
            calculator.Divide(a, b);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void Divide_TwoNumbers_ReturnsCorrectQuotient()
        {
            // Arrange
            double a = 10;
            double b = 2;

            // Act
            double result = calculator.Divide(a, b);

            // Assert
            Assert.AreEqual(5, result, "Dividing 10 by 2 should return 5");
        }

        [TestMethod]
        public void StoreInMemory_StoresValueCorrectly()
        {
            // Arrange
            double number = 8.5;

            // Act
            calculator.StoreInMemory(number);
            double result = calculator.RecallMemory();

            // Assert
            Assert.AreEqual(number, result, "Stored value should be recalled correctly");
        }

        [TestMethod]
        public void RecallMemory_WithoutStoring_ReturnsZero()
        {
            // Arrange
            // No value is stored in memory

            // Act
            double result = calculator.RecallMemory();

            // Assert
            Assert.AreEqual(0, result, "Initial memory recall should return 0");
        }

        [TestMethod]
        public void ClearMemory_AfterStoringValue_SetsMemoryToZero()
        {
            // Arrange
            double number = 8.5;
            calculator.StoreInMemory(number);

            // Act
            calculator.ClearMemory();
            double result = calculator.RecallMemory();

            // Assert
            Assert.AreEqual(0, result, "Memory should be cleared to 0");
        }
    }
}