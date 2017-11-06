using MOUNB.DAL.Entities;
using System;

namespace MOUNB.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        void Commit();
    }
}
