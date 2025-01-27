using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

namespace System.Data.Entity.Tests
{
    [TestClass]
    public class DbFunctionAttributeTests
    {
        [TestMethod]
        public void Constructor_Should_Initialize_Properties_Correctly()
        {
            // Arrange
            string namespaceName = "TestNamespace";
            string functionName = "TestFunction";

            // Act
            var attribute = new DbFunctionAttribute(namespaceName, functionName);

            // Assert
            Assert.AreEqual(namespaceName, attribute.NamespaceName, "NamespaceName property was not initialized correctly.");
            Assert.AreEqual(functionName, attribute.FunctionName, "FunctionName property was not initialized correctly.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Should_Throw_Exception_When_NamespaceName_Is_Null()
        {
            // Arrange
            string functionName = "TestFunction";

            // Act
            var attribute = new DbFunctionAttribute(null, functionName);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_Should_Throw_Exception_When_FunctionName_Is_Null()
        {
            // Arrange
            string namespaceName = "TestNamespace";

            // Act
            var attribute = new DbFunctionAttribute(namespaceName, null);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public void GetFunctionName_Should_Return_Correct_FunctionName_With_Schema()
        {
            // Arrange
            string namespaceName = "TestNamespace";
            string functionName = "TestFunction";
            string schema = "dbo";
            var attribute = new DbFunctionAttribute(namespaceName, functionName);

            // Act
            var result = attribute.GetFunctionName(schema);

            // Assert
            Assert.AreEqual("dbo.TestFunction", result, "GetFunctionName did not return the correct value with schema.");
        }

        [TestMethod]
        public void GetFunctionName_Should_Return_Correct_FunctionName_Without_Schema()
        {
            // Arrange
            string namespaceName = "TestNamespace";
            string functionName = "TestFunction";
            var attribute = new DbFunctionAttribute(namespaceName, functionName);

            // Act
            var result = attribute.GetFunctionName(null);

            // Assert
            Assert.AreEqual("TestFunction", result, "GetFunctionName did not return the correct value without schema.");
        }

        [TestMethod]
        public void GetFunctionName_Should_Handle_Null_FunctionName()
        {
            // Arrange
            string namespaceName = "TestNamespace";
            string functionName = null;
            var attribute = new DbFunctionAttribute(namespaceName, functionName);

            // Act
            var result = attribute.GetFunctionName(null);

            // Assert
            Assert.IsTrue(result.StartsWith("func_"), "GetFunctionName did not handle null FunctionName correctly.");
        }

        [TestMethod]
        public void IsValidSchema_Should_Return_True_For_Valid_Schema()
        {
            // Arrange
            string schema = new string('a', 128);
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");

            // Act
            var result = attribute.IsValidSchema(schema);

            // Assert
            Assert.IsTrue(result, "IsValidSchema did not return true for a valid schema.");
        }

        [TestMethod]
        public void IsValidSchema_Should_Return_False_For_Null_Schema()
        {
            // Arrange
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");

            // Act
            var result = attribute.IsValidSchema(null);

            // Assert
            Assert.IsFalse(result, "IsValidSchema did not return false for a null schema.");
        }

        [TestMethod]
        public void IsValidSchema_Should_Return_False_For_Too_Long_Schema()
        {
            // Arrange
            string schema = new string('a', 130);
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");

            // Act
            var result = attribute.IsValidSchema(schema);

            // Assert
            Assert.IsFalse(result, "IsValidSchema did not return false for a too long schema.");
        }

        [TestMethod]
        public void ValidateParameters_Should_Return_True_For_Valid_Parameters()
        {
            // Arrange
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");
            object[] parameters = { 1, "test", DateTime.Now };

            // Act
            var result = attribute.ValidateParameters(parameters);

            // Assert
            Assert.IsTrue(result, "ValidateParameters did not return true for valid parameters.");
        }

        [TestMethod]
        public void ValidateParameters_Should_Return_False_For_Null_Parameter()
        {
            // Arrange
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");
            object[] parameters = { 1, null, DateTime.Now };

            // Act
            var result = attribute.ValidateParameters(parameters);

            // Assert
            Assert.IsFalse(result, "ValidateParameters did not return false for parameters containing null.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidateParameters_Should_Throw_Exception_For_Null_Parameters()
        {
            // Arrange
            var attribute = new DbFunctionAttribute("TestNamespace", "TestFunction");

            // Act
            attribute.ValidateParameters(null);

            // Assert is handled by ExpectedException
        }
    }
}