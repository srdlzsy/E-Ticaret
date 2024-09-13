
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace Domain.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public int ProductId { get; set; }

        // Bir kategori birçok ürüne sahip olabilir
        public ICollection<Product> Product { get; set; } = new List<Product>();
    }

}
