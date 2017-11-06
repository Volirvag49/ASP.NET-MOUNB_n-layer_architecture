using MOUNB.BLL.DTO;
using System;
using System.Collections.Generic;

namespace MOUNB.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        IEnumerable<UserDTO> GetAllUsers();
        UserDTO GetUserById(int? id);
        void RegisterUser(UserDTO userDTO);
        void EditUser(UserDTO item);
        void DeleteUser(UserDTO item);
    }
}
