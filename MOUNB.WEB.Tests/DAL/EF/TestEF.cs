using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using MOUNB.DAL.Entities;


namespace MOUNB.WEB.Tests.DAL.EF
{
    [TestClass]
    public class TestEF
    {
        private TestDBContext db = new TestDBContext();

        [TestMethod]
        public void Users_GetAll()
        {
            // arrange
            // act
            var users =  db.Users.ToList();

            // assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
        }

        [TestMethod]
        public void Users_FindById()
        {
            // arrange
            // act
            var user = db.Users.Find(1);


            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Users_Create()
        {
            // arrange
            User newUser = new User()
            { Name = "UTest User", Login = "123123", Password = "123123"
            , Position = "UTest", Role = UserRole.Нет };

            int userCount = db.Users.Count();
            // act

            var user = db.Users.Add(newUser);
            db.SaveChanges();

            var users = db.Users.ToList();
            var newReaders = from u in db.Users
                             .Where(u => u.Name == "UTest User")
                             select u;

            // assert
            Assert.AreEqual(userCount + 1, users.Count);
            Assert.AreNotEqual(0, newReaders.Count());
        }
    }
}
