using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Services;
using MOUNB.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using PagedList;

namespace MOUNB.WEB.Tests.BLL
{
    [TestClass]
    public class UserServiceTest
    {
        TestDBContext db;
        UnitOfWork unitOfWork;
        UserService userService;

        [TestInitialize]
        public void Initialize()
        {
            db = new TestDBContext();
            unitOfWork = new UnitOfWork(db);
            userService = new UserService(unitOfWork);           
        }

        [TestMethod]
        public void Users_GetAll()
        {
            // arrange
            // act
            var users = userService.GetAllUsers() as List<UserDTO>;

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
            string sortOrder = "Login";
            string searchString = "";
            string searchSelection = "";
            int page = 1;
            int pageSize = 4;
            // act
            StaticPagedList<UserDTO> usersPage_1 = userService.GetPagedUsers(sortOrder, searchString, searchSelection, page, pageSize);

            page = 2;
            StaticPagedList<UserDTO> usersPage_2 = userService.GetPagedUsers(sortOrder, searchString, searchSelection, page, pageSize);

            // assert
            Assert.IsNotNull(usersPage_1);
            Assert.AreNotEqual(0, usersPage_1.TotalItemCount);
            Assert.AreEqual("Пользователь по умолчанию 1", usersPage_1[0].Name);
            Assert.AreEqual("Давыдов Варфоломей Викторович", usersPage_1[3].Name);

            Assert.AreEqual("Дидиченко Арсений Александрович", usersPage_2[0].Name);
            Assert.AreEqual("Леонтьева Васса Ивановна", usersPage_2[3].Name);
        }

        [TestMethod]
        public void Users_FindById()
        {
            // arrange
            int? Id = 1;

            // act
            var user = userService.GetUserById(Id);

            // assert
            Assert.IsNotNull(user);
            Assert.AreEqual(Id, user.Id);
        }

        [TestMethod]
        public void Users_Create()
        {
            // arrange
            UserDTO newUser = new UserDTO()
            {
                Name = "UTest User",
                Login = "123123",
                Password = "123123",
                Position = "UTest",
                Role = UserRole.Нет
            };

            int oldUserCount = db.Users.Count();
            // act

            userService.RegisterUser(newUser);

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
            UserDTO user = userService.GetUserById(1);

            int oldUserCount = db.Users.Count();
            // act

            userService.DeleteUser(user);

            var users = db.Users.ToList();
            var emptyUser = db.Users.Find(1);

            // assert
            Assert.AreEqual(oldUserCount - 1, users.Count);
            Assert.IsNull(emptyUser);

        }             
    }
}
