using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Models;

namespace Demo.Services
{
    public class ProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>();
        }

        public Product AddProduct(string name, decimal price, string description = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty");

            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero");

            var product = new Product
            {
                Id = _products.Count + 1,
                Name = name.Trim(),
                Price = price,
                Description = description?.Trim()
            };

            _products.Add(product);
            return product;
        }

        public Product GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public IEnumerable<Product> GetAvailableProducts()
        {
            return _products.Where(p => p.IsAvailable).ToList();
        }

        public bool UpdateProduct(int id, string name = null, decimal? price = null, string description = null)
        {
            var product = GetProduct(id);
            if (product == null)
                return false;

            if (name != null)
                product.Name = name.Trim();

            if (price.HasValue)
            {
                if (price.Value <= 0)
                    throw new ArgumentException("Price must be greater than zero");
                product.Price = price.Value;
            }

            if (description != null)
                product.Description = description.Trim();

            return true;
        }

        public bool DeleteProduct(int id)
        {
            var product = GetProduct(id);
            if (product == null)
                return false;

            return _products.Remove(product);
        }

        public void ApplyDiscountToAll(decimal percentage)
        {
            foreach (var product in _products)
            {
                product.ApplyDiscount(percentage);
            }
        }
    }
} 