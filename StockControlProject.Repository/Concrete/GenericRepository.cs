using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Repository.Abstract;
using StockControlProject.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StockControlProject.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        // BU İŞLEMLERİN HEPSİ VERİTABANI TARAFINDA YAPILAN İŞLEMLERDİR.
        private readonly StockControlContext _context;
        public GenericRepository(StockControlContext context)
        {
            _context = context;
        }
        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item); // Hangi entity/tablo olacağını bilmediğim için generic olarak yazıyorum.
                return Save() > 0; // Girdi sayısı 0'dan büyükse true döner. Ancak burada 1 döner 2 dönemez zaten.
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool Add(List<T> items)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope()) // REM üzerinde kullanılmayan dosyanın belirli süre geçtiğinde garbage collector tarafından toplanmasını söylüyor.Using bloğu garbage collectoru beklemeden bu skopedan çıkınca siler.10 veriden 2'si silinmezse diğerlerini de ekleme diyor. Eticaret sitesinde transaction olarak işlem yapılmazsa (vt tarafında commit rollback) örneğin sms kod yanlış olsa bile sipariş listesine düşer.
                {
                    //_context.Set<T>().AddRange(items); AddedDate'i eklemek istediğim için foreach kullandım.
                    foreach (T item in items)
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();
                }

                return Save() > 0; // Burada 1 veya daha fazla dönecektir.
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any(exp); // Bizim verdiğimiz LINQ ifadesine göre veriyor.
        public List<T> GetActive() => _context.Set<T>().Where(x => x.IsActive == true).ToList();
        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes) // Birden fazla tabloyu aldığımız yer.
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include)); // O an queryden dönen tablo =current,ilişkili olan tablosu ise include. Örnek : product aktif olanlar ile categorylerini de getir.
            }
            return query;
        }
        public List<T> GetAll() => _context.Set<T>().ToList();
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp); //LINQ Exp gelecek. Bizim sorgumuz yani.
        public T GetById(int id) => _context.Set<T>().Find(id); // Find methodu pk'ler ile çalışır.
        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.Id == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include)); // O an queryden dönen tablo =current,ilişkili olan tablosu ise include. Örnek : product aktif olanlar ile categorylerini de getir.
            }
            return query;
        }
        public List<T> GetDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().Where(exp).ToList();
        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item);
        }
        public bool Remove(int id)
        {
            try
            {
                using(TransactionScope ts= new TransactionScope()) // Birden fazla işlem yapmış olacağız. Id'ye göre item bul, aktifliğini false yap, update et.
                {
                    T item = GetById(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using(TransactionScope ts= new TransactionScope())
                {
                    var collection = GetDefault(exp); // Verilen ifadeye göre ilgili nesneleri collection'a atıyor.
                    int counter = 0;

                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool operationResult = Update(item);
                        if (operationResult) counter++; // Sıradaki item'ın silinme işlemi (yani silindi işaretleme) başarılı ise sayacı 1 arttırıyoruz.
                    }
                    if (collection.Count == counter) ts.Complete(); // Koleksiyondaki eleman sayısı ile silme işlemi gerçekleşen eleman sayısı(counter) eşit ise bu işlemlerin tümü başarılır.
                    else return false; // Yoksa hiçbiri silinmez.
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {
                return false;
            } 
        }
        public void DetachEntity(T item)
        {
            _context.Entry<T>(item).State=EntityState.Detached; // Bir entry'i takip etmeyi bırakmak için kullanılır. Arka tarafta change tracker çalışıyor. İşlemini bitirdiğimiz satırla bağlantıyı koparıyoruz. Change tracker'ı devredışı bırakıyormuşuz gibi düşünebiliriz.
        }
        public bool Activate(int id)
        {
            T item = GetById(id);
            item.IsActive = true;
            return Update(item);
        }
    }
}
