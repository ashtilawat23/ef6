using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class TemperatureConverterTests
{
    private TemperatureConverter _converter;

    [TestInitialize]
    public void Setup()
    {
        _converter = new TemperatureConverter();
    }

    [TestMethod]
    public void CelsiusToFahrenheit_ZeroCelsius_Returns32Fahrenheit()
    {
        // Arrange
        double celsius = 0.0;

        // Act
        double result = _converter.CelsiusToFahrenheit(celsius);

        // Assert
        Assert.AreEqual(32.0, result, "0 Celsius should be 32 Fahrenheit");
    }

    [TestMethod]
    public void CelsiusToFahrenheit_PositiveCelsius_ReturnsCorrectFahrenheit()
    {
        // Arrange
        double celsius = 100.0;

        // Act
        double result = _converter.CelsiusToFahrenheit(celsius);

        // Assert
        Assert.AreEqual(212.0, result, "100 Celsius should be 212 Fahrenheit");
    }

    [TestMethod]
    public void FahrenheitToCelsius_ZeroFahrenheit_ReturnsNegative17Point78Celsius()
    {
        // Arrange
        double fahrenheit = 0.0;

        // Act
        double result = _converter.FahrenheitToCelsius(fahrenheit);

        // Assert
        Assert.AreEqual(-17.7778, result, 0.0001, "0 Fahrenheit should be approximately -17.7778 Celsius");
    }

    [TestMethod]
    public void FahrenheitToCelsius_PositiveFahrenheit_ReturnsCorrectCelsius()
    {
        // Arrange
        double fahrenheit = 212.0;

        // Act
        double result = _converter.FahrenheitToCelsius(fahrenheit);

        // Assert
        Assert.AreEqual(100.0, result, "212 Fahrenheit should be 100 Celsius");
    }

    [TestMethod]
    public void CelsiusToKelvin_ZeroCelsius_Returns273Point15Kelvin()
    {
        // Arrange
        double celsius = 0.0;

        // Act
        double result = _converter.CelsiusToKelvin(celsius);

        // Assert
        Assert.AreEqual(273.15, result, "0 Celsius should be 273.15 Kelvin");
    }

    [TestMethod]
    public void KelvinToCelsius_ZeroKelvin_ReturnsNegative273Point15Celsius()
    {
        // Arrange
        double kelvin = 0.0;

        // Act
        double result = _converter.KelvinToCelsius(kelvin);

        // Assert
        Assert.AreEqual(-273.15, result, "0 Kelvin should be -273.15 Celsius");
    }

    [TestMethod]
    public void FahrenheitToKelvin_ZeroFahrenheit_Returns255Point37Kelvin()
    {
        // Arrange
        double fahrenheit = 0.0;

        // Act
        double result = _converter.FahrenheitToKelvin(fahrenheit);

        // Assert
        Assert.AreEqual(255.372, result, 0.001, "0 Fahrenheit should be approximately 255.372 Kelvin");
    }

    [TestMethod]
    public void KelvinToFahrenheit_ZeroKelvin_ReturnsNegative459Point67Fahrenheit()
    {
        // Arrange
        double kelvin = 0.0;

        // Act
        double result = _converter.KelvinToFahrenheit(kelvin);

        // Assert
        Assert.AreEqual(-459.67, result, 0.01, "0 Kelvin should be approximately -459.67 Fahrenheit");
    }

    [TestMethod]
    public void IsValidTemperature_ZeroKelvin_ReturnsTrue()
    {
        // Arrange
        double kelvin = 0.0;

        // Act
        bool isValid = _converter.IsValidTemperature(kelvin);

        // Assert
        Assert.IsTrue(isValid, "0 Kelvin is a valid temperature");
    }

    [TestMethod]
    public void IsValidTemperature_NegativeKelvin_ReturnsFalse()
    {
        // Arrange
        double kelvin = -1.0;

        // Act
        bool isValid = _converter.IsValidTemperature(kelvin);

        // Assert
        Assert.IsFalse(isValid, "Negative Kelvin should be invalid");
    }

    [TestMethod]
    public void IsValidTemperature_PositiveKelvin_ReturnsTrue()
    {
        // Arrange
        double kelvin = 100.0;

        // Act
        bool isValid = _converter.IsValidTemperature(kelvin);

        // Assert
        Assert.IsTrue(isValid, "Positive Kelvin should be valid");
    }
}