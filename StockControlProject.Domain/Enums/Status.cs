using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Enums
{
    public enum Status // Order Status bekliyor, tamamlanmış, onay bekliyor gibi
    {
        Pending=0,
        Cancelled=1,
        Confirmed=3
    }
}
