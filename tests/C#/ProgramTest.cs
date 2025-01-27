using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DemoApp.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void Main_ShouldPrintWelcomeMessage()
        {
            // Arrange
            var input = new StringReader("John\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.Main(new string[] { });

            // Assert
            var consoleOutput = output.ToString();
            Assert.IsTrue(consoleOutput.Contains("Welcome to the Demo App!"), "Console did not print welcome message.");
        }

        [TestMethod]
        public void Main_ShouldGreetWithName_WhenNameIsProvided()
        {
            // Arrange
            var input = new StringReader("John\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.Main(new string[] { });

            // Assert
            var consoleOutput = output.ToString();
            Assert.IsTrue(consoleOutput.Contains("Hello, John!"), "Console did not greet with the provided name.");
        }

        [TestMethod]
        public void Main_ShouldGreetAsAnonymous_WhenNameIsNotProvided()
        {
            // Arrange
            var input = new StringReader("\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.Main(new string[] { });

            // Assert
            var consoleOutput = output.ToString();
            Assert.IsTrue(consoleOutput.Contains("Hello, Anonymous!"), "Console did not greet as Anonymous when no name was provided.");
        }

        [TestMethod]
        public void Main_ShouldPrintEvenNumbers()
        {
            // Arrange
            var input = new StringReader("John\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.Main(new string[] { });

            // Assert
            var consoleOutput = output.ToString();
            Assert.IsTrue(consoleOutput.Contains("2"), "Console did not print even number 2.");
            Assert.IsTrue(consoleOutput.Contains("4"), "Console did not print even number 4.");
        }

        [TestMethod]
        public void Main_ShouldPromptForExit()
        {
            // Arrange
            var input = new StringReader("John\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            Program.Main(new string[] { });

            // Assert
            var consoleOutput = output.ToString();
            Assert.IsTrue(consoleOutput.Contains("Press any key to exit..."), "Console did not prompt for exit.");
        }
    }
}