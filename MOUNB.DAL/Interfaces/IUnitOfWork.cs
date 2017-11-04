using MOUNB.DAL.Entities;
using System;

namespace MOUNB.DAL.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        void Commit();
    }
}
