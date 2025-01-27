public class UnitConverter
{
    // Length conversions
    public double MetersToFeet(double meters)
    {
        return meters * 3.28084;
    }

    public double KilometersToMiles(double kilometers)
    {
        return kilometers * 0.621371;
    }

    // Weight conversions
    public double KilogramsToPounds(double kilograms)
    {
        return kilograms * 2.20462;
    }

    public double GramsToOunces(double grams)
    {
        return grams * 0.035274;
    }

    // Temperature conversions
    public double CelsiusToFahrenheit(double celsius)
    {
        return (celsius * 9/5) + 32;
    }

    // Volume conversions
    public double LitersToGallons(double liters)
    {
        return liters * 0.264172;
    }

    public double MillilitersToFluidOunces(double milliliters)
    {
        return milliliters * 0.033814;
    }
}
