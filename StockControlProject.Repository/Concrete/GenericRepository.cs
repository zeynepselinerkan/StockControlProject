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
        private readonly StockControlContext _context;
        public GenericRepository(StockControlContext context)
        {
            _context=context;
        }
        public bool Activate(int id)
        {
            throw new NotImplementedException();
        }

        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item); // Hangi entity/tablo olacağını bilmediğim için generic olarak yazıyorum.
                return Save()>0; // Girdi sayısı 0'dan büyükse true döner. Ancak burada 1 döner 2 dönemez zaten.
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

        public bool Any(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public void DetachEntity(T item)
        {
            throw new NotImplementedException();
        }

        public List<T> GetActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T GetByDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public bool Update(T item)
        {
            throw new NotImplementedException();
        }
    }
}
