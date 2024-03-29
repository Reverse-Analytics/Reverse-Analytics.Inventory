﻿using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int NumberOfProducts { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public ProductCategory()
        {
            CategoryName = string.Empty;
            Products = new List<Product>();
        }

        public ProductCategory(string categoryName)
        {
            CategoryName = categoryName;
            Products = new List<Product>();
        }

        public ProductCategory(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName;
        }
    }
}
