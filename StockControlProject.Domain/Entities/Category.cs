using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products=new List<Product>(); // Boş geldiğinde hataya düşmemek için.
        }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navgation Properties

        // Bir kategorinin birden fazla ürünü olabilir.
        public virtual List<Product> Products{ get; set; }
    }
}
