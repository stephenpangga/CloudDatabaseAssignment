using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;

namespace Infrastructure
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(string UserId);

        Task<User> AddUserAsync(UserDTO userDTO);

        Task<User> UpdateUserAsync(UserDTO userDTO);

        Task DeleteUserAsync(string UserId);
    }
}
