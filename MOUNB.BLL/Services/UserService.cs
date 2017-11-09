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
using PagedList;
using System.Linq.Expressions;

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

        public StaticPagedList<UserDTO> GetPagedUsers(string sortOrder, string searchString, string searchSelection, int? page, int? pageSize)
        {
            // filter: q => q.Name ==""
            Func<IQueryable<User>, IOrderedQueryable<User>> sortQuery;
            Expression<Func<User, bool>> searchQuery = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchSelection)
                {
                    case "Login":
                        searchQuery = q => q.Login.ToLower().Contains(searchString.ToLower());
                        break;
                    case "Position":
                        searchQuery = q => q.Position.ToLower().Contains(searchString.ToLower());
                        break;
                    case "Role":
                        searchQuery = q => q.Role.ToString().ToLower().Contains(searchString.ToLower());
                        break;
                    default:
                        searchQuery = q => q.Name.ToLower().Contains(searchString.ToLower());
                        break;
                }
            } // Конец if (!String.IsNullOrEmpty(searchString))

            switch (sortOrder)
            {
                case "name_desc":
                    sortQuery = q => q.OrderByDescending(s => s.Name);
                    break;
                case "Login":
                    sortQuery = q => q.OrderBy(s => s.Login);
                    break;
                case "Login_desc":
                    sortQuery = q => q.OrderByDescending(s => s.Login);
                    break;
                case "Position":
                    sortQuery = q => q.OrderBy(s => s.Position);
                    break;
                case "Position_desc":
                    sortQuery = q => q.OrderByDescending(s => s.Position);
                    break;
                case "Role":
                    sortQuery = q => q.OrderBy(s => s.Role);
                    break;
                case "Role_desc":
                    sortQuery = q => q.OrderByDescending(s => s.Role);
                    break;
                default:  
                    sortQuery = q => q.OrderBy(t => t.Name);
                    break;
            }


            var pagedUsers = unitOfWork.Users.GetWithPaging(orderBy: sortQuery, filter: searchQuery, size: pageSize.Value, page: page.Value);

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();

            });

            var usersDTO = Mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(pagedUsers.ToArray());
            var result = new StaticPagedList<UserDTO>(usersDTO, pagedUsers.GetMetaData());
            return result;
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
        // Проверка логина на уникальность 
        private void CheckLogin(UserDTO user)
        {
            // поиск других пользователей с введённым логином
            var loginIsExist = unitOfWork.Users.IsExist(where: q => q.Login == user.Login && q.Id != user.Id);
            if (loginIsExist)
            {
                throw new ValidationException("Логин должен быть уникальным", "Login");
            }
        } 

        public void RegisterUser(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                throw new ValidationException("Требуется пользователь", "");
            }

            CheckLogin(userDTO);

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

            CheckLogin(userDTO);

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

            User user = unitOfWork.Users.GetById(userDTO.Id);

            unitOfWork.Users.Delete(user);
            unitOfWork.Commit();
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }
    }
}
