using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.DAL.Entities;
using MOUNB.DAL.Repositories;
using MOUNB.DAL.EF;
using System.Linq;

namespace MOUNB.WEB.Tests.DAL.Repositories
{
    [TestClass]
    public class UOWTest
    {
        //UnitOfWork unitOfWork = new UnitOfWork("name = TestDBConnection");

        TestDBContext db;
        UnitOfWork unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            db = new TestDBContext();
            unitOfWork = new UnitOfWork(db);
        }

        [TestMethod]
        public void Users_GetAll()
        {
            // arrange
            // act
            var users = unitOfWork.Users.GetAll();

            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count());
        }

        [TestMethod]
        public void Users_FindById()
        {
            // arrange
            // act
            var user = unitOfWork.Users.GetById(1);

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Users_Create()
        {
            // arrange
            User newUser = new User()
            {
                Name = "UTest User",
                Login = "123123",
                Password = "123123"
            ,
                Position = "UTest",
                Role = UserRole.Нет
            };

            int oldUserCount = db.Users.Count();
            // act

            unitOfWork.Users.Create(newUser);
            unitOfWork.Commit();

            var users = db.Users.ToList();
            var newReaders = from u in db.Users
                             .Where(u => u.Name == "UTest User")
                             select u;

            // assert
            Assert.AreEqual(oldUserCount + 1, users.Count);
            Assert.AreNotEqual(0, newReaders.Count());
        }
    }
}
