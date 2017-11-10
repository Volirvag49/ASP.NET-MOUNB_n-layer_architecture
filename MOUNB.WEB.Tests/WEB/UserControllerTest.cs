using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Services;
using MOUNB.DAL.Repositories;
using System.Linq;
using MOUNB.WEB.Controllers;
using System.Collections.Generic;
using MOUNB.WEB.Models;
using System.Web.Mvc;
using PagedList;

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
        public void Users_Index()
        {
            //Arrange
            string sortOrder = null;
            string searchString = null;
            string searchSelection = null;
            string currentFilter = null;
            string currentSelection = null; 
            int page = 1;
            int pageSize = 5;
            //Act
            objController.Index(sortOrder, searchString, searchSelection, currentFilter, currentSelection, page, pageSize);

            IPagedList<UserViewModel> users = objController.ViewData.Model as IPagedList<UserViewModel>;
            //Assert
            Assert.IsNotNull(users);
            Assert.AreNotEqual(0, users.Count);
            Assert.AreEqual(9, users[0].Id);
            Assert.AreEqual(6, users[1].Id);
        }
    }
}
