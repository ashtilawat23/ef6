using System;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Demo
{
    /// <summary>
    /// A demo product class that shows basic C# features and EF Core annotations
    /// </summary>
    public class DemoProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsAvailable { get; set; }

        public DemoProduct(string name, decimal price)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Price = price;
            IsAvailable = true;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero", nameof(newPrice));
            
            Price = newPrice;
        }

        public override string ToString()
        {
            return $"Product: {Name}, Price: ${Price:F2}, Available: {IsAvailable}";
        }
    }
} 