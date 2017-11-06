using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOUNB.BLL.DTO;
using MOUNB.BLL.Services;
using MOUNB.DAL.Repositories;
using System.Linq;

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
            var users = userService.GetAllUsers();

            // assert
            Assert.IsNotNull(users);
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
    }
}
