using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Services;
using MOUNB.DAL.Repositories;
using System.Linq;
using MOUNB.WEB.Controllers;
using System.Collections.Generic;
using MOUNB.WEB.Models;
using System.Web.Mvc;

namespace MOUNB.WEB.Tests.WEB
{
    [TestClass]
    public class UserControllerTest
    {
        TestDBContext db;
        UnitOfWork unitOfWork;
        UserService userService;
        UsersController objController;

        [TestInitialize]
        public void Initialize()
        {
            db = new TestDBContext();
            unitOfWork = new UnitOfWork(db);
            userService = new UserService(unitOfWork);
            objController = new UsersController(userService);
        }

        [TestMethod]
        public void Users_GetAll()
        {
            //Arrange
            //Act
            var users = (objController.Index() as ViewResult).Model 
                as List<UserViewModel>;
            //Assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual("Пользователь по умолчанию 1", users[0].Name);
            Assert.AreEqual("Пользователь по умолчанию 2", users[1].Name);
        }
    }
}
