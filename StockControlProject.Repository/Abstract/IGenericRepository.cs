using StockControlProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Repository.Abstract
{
    public interface IGenericRepository<T> where T : BaseEntity // BaseEntity'den kalıtım alan classları temsil eden bir T
    {
        // Method(kendi methodlarımız EF'inkiler değil.) başlıkları tanımlanır. 

        bool Add(T item);
        bool Add(List<T> items);
        bool Update(T item);
        bool Remove(T item);
        bool Remove(int id);
        bool RemoveAll(Expression<Func<T, bool>> exp); // Filtre için, LINQ sorgusu yazmak için tanımlama bu.
        T GetById(int id); // T tipi döner.
        IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes); // İçerisine gönderilen queryleri birleştirip döndüren.IQueryable'ın sorgulanabilir olması gerekir. Ürünü getirirken kategoriyi de getirmek için(bağlı olduğu verileri). Bağlantılı entity sayısını bilmediğim için (kaç tane include edileceğini bilmediğim için) params-->[] kullandım. Object tipinde bir nesne alıyorum.Params ilk parametreye yazılmaz.
        T GetByDefault(Expression<Func<T, bool>> exp); // First or default methodu çalışsın diyebiliriz.
        List<T> GetActive();
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes); // İlişkili aktifleri de getirsin.
        List<T> GetDefault(Expression<Func<T, bool>> exp);// Where ile liste döndürecek.
        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes); // İlişkililerin de tümünü getirsin.

        bool Activate(int id); // Pasif nesneyi aktifleştirmek için.
        bool Any(Expression<Func<T, bool>> exp); // Şu tarih aralığında ürün var mı gibi filtre
        int Save(); // SaveChanges methoduna eklenecek.
        void DetachEntity(T item); // İlgili entityi takip etmeyi bırakan method. Bir işlemden sonra tekrar yapmak için. (?)
    }
}
