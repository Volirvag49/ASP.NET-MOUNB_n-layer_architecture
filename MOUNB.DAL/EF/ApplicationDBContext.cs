using MOUNB.DAL.Entities;
using System.Data.Entity;

namespace MOUNB.DAL.EF
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() : base("name=DefaultConnection")
        {
        }
        public ApplicationDBContext(string dbConnection) : base(dbConnection)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
