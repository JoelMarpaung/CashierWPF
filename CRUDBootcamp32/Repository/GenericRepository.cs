using CRUDBootcamp32.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDBootcamp32.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private MyContext myContext = null;
        private DbSet<T> table = null;

        public GenericRepository()
        {
            this.myContext = new MyContext();
            table = myContext.Set<T>();
        }

        public GenericRepository(MyContext myContext)
        {
            this.myContext = myContext;
            table = myContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetbyId(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            myContext.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            myContext.SaveChanges();
        }
    }
}
