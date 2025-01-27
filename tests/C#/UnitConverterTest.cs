using NUnit.Framework;

[TestFixture]
public class UnitConverterTests
{
    private UnitConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new UnitConverter();
    }

    // Length Conversion Tests
    [Test]
    public void MetersToFeet_WithPositiveValue_ReturnsCorrectFeet()
    {
        // Arrange
        double meters = 1.0;
        
        // Act
        double result = _converter.MetersToFeet(meters);
        
        // Assert
        Assert.AreEqual(3.28084, result, 0.00001, "Meters to Feet conversion failed for positive value.");
    }

    [Test]
    public void MetersToFeet_WithZero_ReturnsZero()
    {
        // Arrange
        double meters = 0.0;
        
        // Act
        double result = _converter.MetersToFeet(meters);
        
        // Assert
        Assert.AreEqual(0.0, result, "Meters to Feet conversion failed for zero value.");
    }

    [Test]
    public void KilometersToMiles_WithPositiveValue_ReturnsCorrectMiles()
    {
        // Arrange
        double kilometers = 1.0;
        
        // Act
        double result = _converter.KilometersToMiles(kilometers);
        
        // Assert
        Assert.AreEqual(0.621371, result, 0.000001, "Kilometers to Miles conversion failed for positive value.");
    }

    [Test]
    public void KilometersToMiles_WithZero_ReturnsZero()
    {
        // Arrange
        double kilometers = 0.0;
        
        // Act
        double result = _converter.KilometersToMiles(kilometers);
        
        // Assert
        Assert.AreEqual(0.0, result, "Kilometers to Miles conversion failed for zero value.");
    }

    // Weight Conversion Tests
    [Test]
    public void KilogramsToPounds_WithPositiveValue_ReturnsCorrectPounds()
    {
        // Arrange
        double kilograms = 1.0;
        
        // Act
        double result = _converter.KilogramsToPounds(kilograms);
        
        // Assert
        Assert.AreEqual(2.20462, result, 0.00001, "Kilograms to Pounds conversion failed for positive value.");
    }

    [Test]
    public void KilogramsToPounds_WithZero_ReturnsZero()
    {
        // Arrange
        double kilograms = 0.0;
        
        // Act
        double result = _converter.KilogramsToPounds(kilograms);
        
        // Assert
        Assert.AreEqual(0.0, result, "Kilograms to Pounds conversion failed for zero value.");
    }

    [Test]
    public void GramsToOunces_WithPositiveValue_ReturnsCorrectOunces()
    {
        // Arrange
        double grams = 1.0;
        
        // Act
        double result = _converter.GramsToOunces(grams);
        
        // Assert
        Assert.AreEqual(0.035274, result, 0.000001, "Grams to Ounces conversion failed for positive value.");
    }

    [Test]
    public void GramsToOunces_WithZero_ReturnsZero()
    {
        // Arrange
        double grams = 0.0;
        
        // Act
        double result = _converter.GramsToOunces(grams);
        
        // Assert
        Assert.AreEqual(0.0, result, "Grams to Ounces conversion failed for zero value.");
    }

    // Temperature Conversion Tests
    [Test]
    public void CelsiusToFahrenheit_WithPositiveValue_ReturnsCorrectFahrenheit()
    {
        // Arrange
        double celsius = 0.0;
        
        // Act
        double result = _converter.CelsiusToFahrenheit(celsius);
        
        // Assert
        Assert.AreEqual(32.0, result, "Celsius to Fahrenheit conversion failed for zero value.");
    }

    [Test]
    public void CelsiusToFahrenheit_WithNegativeValue_ReturnsCorrectFahrenheit()
    {
        // Arrange
        double celsius = -40.0;
        
        // Act
        double result = _converter.CelsiusToFahrenheit(celsius);
        
        // Assert
        Assert.AreEqual(-40.0, result, "Celsius to Fahrenheit conversion failed for negative value.");
    }

    // Volume Conversion Tests
    [Test]
    public void LitersToGallons_WithPositiveValue_ReturnsCorrectGallons()
    {
        // Arrange
        double liters = 1.0;
        
        // Act
        double result = _converter.LitersToGallons(liters);
        
        // Assert
        Assert.AreEqual(0.264172, result, 0.000001, "Liters to Gallons conversion failed for positive value.");
    }

    [Test]
    public void LitersToGallons_WithZero_ReturnsZero()
    {
        // Arrange
        double liters = 0.0;
        
        // Act
        double result = _converter.LitersToGallons(liters);
        
        // Assert
        Assert.AreEqual(0.0, result, "Liters to Gallons conversion failed for zero value.");
    }

    [Test]
    public void MillilitersToFluidOunces_WithPositiveValue_ReturnsCorrectFluidOunces()
    {
        // Arrange
        double milliliters = 1.0;
        
        // Act
        double result = _converter.MillilitersToFluidOunces(milliliters);
        
        // Assert
        Assert.AreEqual(0.033814, result, 0.000001, "Milliliters to Fluid Ounces conversion failed for positive value.");
    }

    [Test]
    public void MillilitersToFluidOunces_WithZero_ReturnsZero()
    {
        // Arrange
        double milliliters = 0.0;
        
        // Act
        double result = _converter.MillilitersToFluidOunces(milliliters);
        
        // Assert
        Assert.AreEqual(0.0, result, "Milliliters to Fluid Ounces conversion failed for zero value.");
    }
}