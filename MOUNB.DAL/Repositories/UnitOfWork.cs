using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOUNB.DAL.EF;
using MOUNB.DAL.Entities;
using MOUNB.DAL.Interfaces;

namespace MOUNB.DAL.Repositories
{
    public class UnitOfWork :IUnitOfWork
    {
        private ApplicationDBContext dbContext;

        private Repository<User> userRepository;

        public UnitOfWork(string connectionString)
        {
            dbContext = new ApplicationDBContext(connectionString);
        }

        public UnitOfWork(ApplicationDBContext context)
        {
            dbContext = context;
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new Repository<User>(dbContext);
                }
                return userRepository;
            }

        }

        public void Commit()
        {
            dbContext.SaveChanges();
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
