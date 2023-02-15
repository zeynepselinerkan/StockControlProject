using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class OrderDetails : BaseEntity
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }

        //Navigation Properties
        // Bir sipariş detayının bir siparişi olur.
        // Bir sipariş detayının bir ürünü olur.
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
