using MOUNB.DAL.EF;
using MOUNB.DAL.Entities;
using MOUNB.DAL.Interfaces;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

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
            dbContext.Entry(item).State = EntityState.Deleted;
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();  
            }
            else
            {
                return query.ToList();
            }
        }

        public PagedList<T> GetWithPaging(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", int page = 1, int size = 5)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return new PagedList<T>(orderBy(query), page, size);
            }
            else
            {
                throw new Exception("Get With Paging query must be sorted");
            }
        }

        public int Count(Expression<Func<T, bool>> where = null)
        {
            return dbSet.Count(where);
        }

        public bool IsExist(Expression<Func<T, bool>> where = null)
        {
            return dbSet.FirstOrDefault(where) != null ? true : false;
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
