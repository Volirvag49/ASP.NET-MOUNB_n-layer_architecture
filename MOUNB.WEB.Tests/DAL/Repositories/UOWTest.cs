using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.DAL.Entities;
using MOUNB.DAL.Repositories;
using MOUNB.DAL.EF;
using System.Linq;
using System.Collections.Generic;
using PagedList;

namespace MOUNB.WEB.Tests.DAL.Repositories
{
    [TestClass]
    public class UOWTest
    {
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
            var users = unitOfWork.Users.GetAll() as List<User>;
            var usersq = unitOfWork.Users.Get(orderBy: q => q.OrderBy(d => d.Name), filter: q => q.Id == 1);
            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual("Пользователь по умолчанию 1", users[0].Name);
            Assert.AreEqual("Пользователь по умолчанию 2", users[1].Name);
        }

        [TestMethod]
        public void Users_GetPagedUsers()
        {
            // arrange
            // act
            var users = unitOfWork.Users.GetWithPaging(orderBy: q => q.OrderBy(d => d.Name), page: 3, size: 3);

            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual("Пользователь по умолчанию 2", users[0].Name);
            Assert.AreEqual("Попова Татьяна Ильинична", users[1].Name);
        }

        [TestMethod]
        public void Users_Get_FindById()
        {
            // arrange
            int id = 1;
            // act
            var users = unitOfWork.Users.Get(orderBy: q => q.OrderBy(d => d.Name), filter: q => q.Id == id) as List<User>;
            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual(users.FirstOrDefault().Id, id);
            Assert.AreEqual("Пользователь по умолчанию 1", users.FirstOrDefault().Name);
        }

        [TestMethod]
        public void Users_Get_SortByName()
        {
            // arrange
            // act
            var users = unitOfWork.Users.Get(orderBy: q => q.OrderBy(d => d.Name)) as List<User>;
            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual("Андреева Елизавета Георгиевна ", users[0].Name);
            Assert.AreEqual("Давыдов Варфоломей Викторович", users[1].Name);
        }

        [TestMethod]
        public void Users_Count()
        {
            // arrange
            // act
            int? user = unitOfWork.Users.Count(where: q => q.Role == UserRole.Библиотекарь);

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Users_IsExist()
        {
            // arrange
            string userLogin = "111111";
            int userId = 1;

            // act: Проверка есть ли пользователь с данным логином и id
            var loginIsExist = unitOfWork.Users.IsExist(where: q => q.Login == userLogin && q.Id == userId);

            // assert
            Assert.IsTrue(loginIsExist);
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

        [TestMethod]
        public void Users_Delete()
        {
            // arrange
            User user = db.Users.Find(1);

            int oldUserCount = db.Users.Count();
            // act

            unitOfWork.Users.Delete(user);
            unitOfWork.Commit();

            var users = db.Users.ToList();
            var emptyUser = db.Users.Find(1);

            // assert
            Assert.AreEqual(oldUserCount - 1, users.Count);
            Assert.IsNull(emptyUser);

        }
    }
}
