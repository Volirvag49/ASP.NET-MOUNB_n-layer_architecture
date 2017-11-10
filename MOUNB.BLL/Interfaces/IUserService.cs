using MOUNB.BLL.DTO;
using System;
using System.Collections.Generic;
using PagedList;


namespace MOUNB.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        IEnumerable<UserDTO> GetAllUsers();
        StaticPagedList<UserDTO> GetPagedUsers(string sortOrder, string searchString, string searchSelection, int? page, int? pageSize);
        UserDTO GetUserById(int? id);
        void RegisterUser(UserDTO userDTO);
        void EditUser(UserDTO item);
        void DeleteUser(UserDTO item);
    }
}
