using System;

public class TemperatureConverter
{
    public double CelsiusToFahrenheit(double celsius)
    {
        return (celsius * 9 / 5) + 32;
    }

    public double FahrenheitToCelsius(double fahrenheit) 
    {
        return (fahrenheit - 32) * 5 / 9;
    }

    public double CelsiusToKelvin(double celsius)
    {
        return celsius + 273.15;
    }

    public double KelvinToCelsius(double kelvin)
    {
        return kelvin - 273.15;
    }

    public double FahrenheitToKelvin(double fahrenheit)
    {
        double celsius = FahrenheitToCelsius(fahrenheit);
        return CelsiusToKelvin(celsius);
    }

    public double KelvinToFahrenheit(double kelvin)
    {
        double celsius = KelvinToCelsius(kelvin);
        return CelsiusToFahrenheit(celsius);
    }

    public bool IsValidTemperature(double kelvin)
    {
        const double absoluteZero = 0.0; // 0 Kelvin is absolute zero
        return kelvin >= absoluteZero;
    }
}
