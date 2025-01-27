using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Demo App!");

        // Create a list of numbers
        var numbers = new List<int> { 1, 2, 3, 4, 5 };

        // Use LINQ to get even numbers
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();

        // String interpolation
        Console.WriteLine($"Original numbers: {string.Join(", ", numbers)}");
        Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");

        // Simple calculation
        var sum = numbers.Sum();
        var average = numbers.Average();

        Console.WriteLine($"Sum of all numbers: {sum}");
        Console.WriteLine($"Average: {average:F2}");

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}