using System;
using NUnit.Framework;
using Moq;

namespace Demo.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [SetUp]
        public void Setup()
        {
            // Setup code if needed
        }

        [TearDown]
        public void Teardown()
        {
            // Teardown code if needed
        }

        [Test]
        public void Main_WhenCalled_PrintsHelloWorld()
        {
            // Arrange
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            Program.Main(new string[] { });

            // Assert
            Assert.AreEqual("Hello World!\n", consoleOutput.ToString(), "Main should print 'Hello World!'");
        }
    }
}