using System;
using System.Collections.Generic;

namespace Inventory.Core.Models
{
    public class ProductCategory
    {
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public ProductCategory()
        {
            CategoryName = String.Empty;
            Products = new List<Product>();
        }

        public ProductCategory(string categoryName)
        {
            CategoryName = categoryName;
            Products = new List<Product>();
        }
    }
}
