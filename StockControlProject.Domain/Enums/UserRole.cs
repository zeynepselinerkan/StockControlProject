using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Enums
{
    public enum UserRole
    {
        Admin=1, // onay
        Supplier=3, // stok ekleyen
        User=5 // stok düşen
    }
}
