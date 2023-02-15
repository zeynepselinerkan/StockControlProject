using StockControlProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public Status Status { get; set; }

        //Navigation Properties

        // Bir siparişin bir kullanıcısı olur.
        // Bir siparişin birden fazla sipariş detayı olur.

        public virtual User? User { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
    }
}
