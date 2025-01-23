using System;
using System.ComponentModel.DataAnnotations;

namespace Demo.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsAvailable { get; set; }

        public Product()
        {
            CreatedDate = DateTime.UtcNow;
            IsAvailable = true;
        }

        public void ApplyDiscount(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100");

            Price = Price - (Price * percentage / 100);
        }

        public override string ToString()
        {
            return $"{Name} - ${Price:F2}";
        }
    }
} 