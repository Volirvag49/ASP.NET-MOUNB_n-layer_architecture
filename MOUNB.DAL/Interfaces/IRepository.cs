using MOUNB.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PagedList;

namespace MOUNB.DAL.Interfaces
{
    public interface IRepository<T>: IDisposable where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                    string includeProperties = "");
        PagedList<T> GetWithPaging(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", int page = 1, int size = 5);

        T GetById(int id);      
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        int Count(Expression<Func<T, bool>> where = null);
        bool IsExist(Expression<Func<T, bool>> where = null);

    }
}
