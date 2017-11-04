using MOUNB.DAL.Entities;
using System;
using System.Collections.Generic;

namespace MOUNB.DAL.Interfaces
{
    public interface IRepository<T>: IDisposable where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
