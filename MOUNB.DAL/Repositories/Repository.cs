using MOUNB.DAL.EF;
using MOUNB.DAL.Entities;
using MOUNB.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MOUNB.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected ApplicationDBContext dbContext;

        protected DbSet<T> dbSet;

        public Repository(ApplicationDBContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Create(T item)
        {
            dbSet.Add(item);
        }

        public void Update(T item)
        {
            dbContext.Entry(item).State = EntityState.Modified;
        }

        public void Delete(T item)
        {
            dbSet.Remove(item);
        }

        // Реализация IRepository<T> : IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
