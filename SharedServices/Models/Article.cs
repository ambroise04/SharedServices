using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Models
{
    public class Article
    {
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        public Article()
        {
            var cat1 = new Category{Id = 1, Name = "Smartphone"};

            var cat2 = new Category{ Id = 2, Name = "Bureau"};

            var prod1 = new Product { Id = 1, Name = "Huawei P30 Pro", Category = cat1 };

            var prod2 = new Product{ Id = 2, Name = "Samsung G6", Category = cat1};

            var prod3 = new Product { Id = 3, Name = "Lave linge", Category = cat2};

            var prod4 = new Product { Id = 4, Name = "Aspirateur", Category = cat2};

            Categories.Add(cat1);
            Categories.Add(cat2);

            Products.Add(prod1);
            Products.Add(prod2);
            Products.Add(prod3);
            Products.Add(prod4);
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}