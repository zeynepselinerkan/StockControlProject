using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetails>();
        }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }

        // Navigation Properties

        // Bir ürünün bir kategorisi olur.
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; } // Loading çeşitleri var. Lazy Loading uygulayabilmek için virtual yazıyoruz.

        // Bir ürünün bir tedarikçisi olur.
        [ForeignKey("Supplier")] // Virtual proptaki nesne adı istiyor.
        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        // Bir ürün birden çok sipariş detayında olabilir.
        public virtual List<OrderDetails> OrderDetails { get; set; } // Hata vermemesi için constructorde listeyi oluşturuyorum.

    }
}
