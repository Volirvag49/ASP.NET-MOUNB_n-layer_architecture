using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MOUNB.BLL.DTO;
using MOUNB.DAL.Interfaces;
using MOUNB.DAL.Entities;
using MOUNB.BLL.Infrastructure;
using MOUNB.BLL.Interfaces;

namespace MOUNB.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork unitOfWork{ get; set; }

        public UserService(IUnitOfWork uow)
        {
            this.unitOfWork = uow;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
            return Mapper.Map<IEnumerable<User>, List<UserDTO>>(unitOfWork.Users.GetAll());
        }

        public UserDTO GetUserById(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Требуется id пользователя", "");
            }


            var user = unitOfWork.Users.GetById(id.Value);

            if (user == null)
            {
                throw new ValidationException("Пользователь не найден", "");
            }

            Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());

            return Mapper.Map<User, UserDTO>(user);

        }

        public void RegisterUser(UserDTO userDTO)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, User>());

            User user = Mapper.Map<UserDTO, User>(userDTO);

            unitOfWork.Users.Create(user);
            unitOfWork.Commit();

        }

        public void EditUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ValidationException("Требуется пользователь", "");
            }

            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, User>());
            User user = Mapper.Map<UserDTO, User>(userDTO);

            unitOfWork.Users.Update(user);
            unitOfWork.Commit();
        }

        public void DeleteUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ValidationException("Требуется пользователь", "");
            }

            Mapper.Initialize(cfg => cfg.CreateMap<UserDTO, User>());
            User user = Mapper.Map<UserDTO, User>(userDTO);

            unitOfWork.Users.Delete(user);
            unitOfWork.Commit();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }


    }
}
