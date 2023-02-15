using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Domain.Entities
{
    public class BaseEntity // Ortak Propertyler
    {
        [Column(Order = 1)] // Bütün entitylerde Id ilk sırada gözükecek şekilde ayarlandı. Sonra entitylerdeki proplar ardından buradaki diğer proplar eklenir kolon olarak.
        public int Id { get; set; }
        public bool IsActive { get; set; } // False'a çekince kullanıcı görmeyecek ancak veritabanında duracak. Örnek stok yok görürüz ancak sepete ekleyemeyiz. Eski yazışmaların görünmesi gibi... KVKK=GDPR silmek isteyen kullanıcı silinmeli ve belirtilmelidir.
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
