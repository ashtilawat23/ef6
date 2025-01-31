using System;
using System.Collections.Generic;
using Microsoft.DotNet.Cli.CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CommandLineApplicationTests
{
    [TestClass]
    public class CommandLineApplicationTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeDefaults()
        {
            // Arrange & Act
            var app = new CommandLineApplication();

            // Assert
            Assert.IsNotNull(app.Options);
            Assert.IsNotNull(app.Arguments);
            Assert.IsNotNull(app.Commands);
            Assert.IsNotNull(app.RemainingArguments);
            Assert.AreEqual(0, app.Invoke());
        }

        [TestMethod]
        public void Command_ShouldAddSubCommand()
        {
            // Arrange
            var app = new CommandLineApplication();

            // Act
            var command = app.Command("subcommand");

            // Assert
            Assert.AreEqual("subcommand", command.Name);
            Assert.AreEqual(app, command.Parent);
            Assert.AreEqual(1, app.Commands.Count);
        }

        [TestMethod]
        public void Option_ShouldAddOption()
        {
            // Arrange
            var app = new CommandLineApplication();

            // Act
            var option = app.Option("-o|--option", "An option", CommandOptionType.NoValue);

            // Assert
            Assert.AreEqual("-o|--option", option.Template);
            Assert.AreEqual("An option", option.Description);
            Assert.AreEqual(CommandOptionType.NoValue, option.OptionType);
            Assert.AreEqual(1, app.Options.Count);
        }

        [TestMethod]
        public void Argument_ShouldAddArgument()
        {
            // Arrange
            var app = new CommandLineApplication();

            // Act
            var argument = app.Argument("arg", "An argument");

            // Assert
            Assert.AreEqual("arg", argument.Name);
            Assert.AreEqual("An argument", argument.Description);
            Assert.AreEqual(1, app.Arguments.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Argument_ShouldThrowException_WhenAddingAfterMultipleValuesArgument()
        {
            // Arrange
            var app = new CommandLineApplication();
            app.Argument("arg1", "An argument", multipleValues: true);

            // Act
            app.Argument("arg2", "Another argument");
        }

        [TestMethod]
        public void Execute_ShouldInvokeCommand()
        {
            // Arrange
            var app = new CommandLineApplication();
            var executed = false;
            app.OnExecute(() => { executed = true; return 0; });

            // Act
            var result = app.Execute();

            // Assert
            Assert.IsTrue(executed);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Execute_ShouldHandleHelpOption()
        {
            // Arrange
            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");

            // Act
            var result = app.Execute("--help");

            // Assert
            Assert.IsTrue(app.IsShowingInformation);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Execute_ShouldHandleVersionOption()
        {
            // Arrange
            var app = new CommandLineApplication();
            app.VersionOption("--version", "1.0.0");

            // Act
            var result = app.Execute("--version");

            // Assert
            Assert.IsTrue(app.IsShowingInformation);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandParsingException))]
        public void Execute_ShouldThrowException_OnUnexpectedArgument()
        {
            // Arrange
            var app = new CommandLineApplication();

            // Act
            app.Execute("unexpected");
        }

        [TestMethod]
        public void Execute_ShouldStoreRemainingArguments_WhenNotThrowingOnUnexpectedArg()
        {
            // Arrange
            var app = new CommandLineApplication(throwOnUnexpectedArg: false);

            // Act
            app.Execute("unexpected", "args");

            // Assert
            CollectionAssert.AreEqual(new List<string> { "unexpected", "args" }, app.RemainingArguments);
        }

        [TestMethod]
        public void ShowHint_ShouldDisplayHelpMessage()
        {
            // Arrange
            var app = new CommandLineApplication();
            app.HelpOption("--help");
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                // Act
                app.ShowHint();

                // Assert
                var result = sw.ToString().Trim();
                Assert.AreEqual("Specify --help for a list of available options and commands.", result);
            }
        }

        [TestMethod]
        public void ShowVersion_ShouldDisplayVersionInfo()
        {
            // Arrange
            var app = new CommandLineApplication
            {
                FullName = "TestApp",
                LongVersionGetter = () => "1.0.0"
            };
            using (var sw = new System.IO.StringWriter())
            {
                Console.SetOut(sw);

                // Act
                app.ShowVersion();

                // Assert
                var result = sw.ToString().Trim();
                Assert.AreEqual("TestApp\n1.0.0", result);
            }
        }
    }
}