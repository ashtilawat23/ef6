namespace DemoApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Demo App!");
        
        // Demonstrate string interpolation and basic input
        Console.Write("Please enter your name: ");
        string? name = Console.ReadLine();
        
        // Null coalescing operator demonstration
        string greeting = $"Hello, {name ?? "Anonymous"}!";
        Console.WriteLine(greeting);
        
        // Demonstrate basic collection and LINQ
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
        
        Console.WriteLine("\nEven numbers from the list:");
        foreach (var number in evenNumbers)
        {
            Console.WriteLine(number);
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
} 