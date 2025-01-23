using System;
using Demo.Models;
using Demo.Services;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create a new product service
                var productService = new ProductService();

                // Add some products
                var laptop = productService.AddProduct("Laptop", 999.99m, "High-performance laptop");
                var phone = productService.AddProduct("Smartphone", 599.99m, "Latest model smartphone");
                var tablet = productService.AddProduct("Tablet", 299.99m, "10-inch tablet");

                Console.WriteLine("All products:");
                foreach (var product in productService.GetAllProducts())
                {
                    Console.WriteLine(product);
                }

                // Update a product
                productService.UpdateProduct(laptop.Id, price: 899.99m);
                Console.WriteLine($"\nUpdated laptop price: {productService.GetProduct(laptop.Id)}");

                // Apply discount to all products
                Console.WriteLine("\nApplying 10% discount to all products...");
                productService.ApplyDiscountToAll(10);

                Console.WriteLine("\nProducts after discount:");
                foreach (var product in productService.GetAllProducts())
                {
                    Console.WriteLine(product);
                }

                // Delete a product
                productService.DeleteProduct(tablet.Id);
                Console.WriteLine($"\nDeleted tablet. Remaining products: {productService.GetAllProducts().Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
} 