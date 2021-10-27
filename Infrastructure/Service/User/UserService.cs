using Domain;
using Domain.DTO;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserService : IUserService
    {
        private readonly ICosmosReadRepository<User> _userReadRepository;
        private readonly ICosmosWriteRepository<User> _userWriteRepository;

        public UserService(ICosmosReadRepository<User> userReadRepository, ICosmosWriteRepository<User> userWriteRepository)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
        }


        public async Task<User> AddUserAsync(UserDTO userDTO)
        {
            User user = new User();
            user.UserId = Guid.NewGuid();
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Address = userDTO.Address;
            user.Email = userDTO.Email;
            user.PartitionKey = userDTO.FirstName;

            return await _userWriteRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            User User = await GetUserByIdAsync(userId);
            await _userWriteRepository.Delete(User);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userReadRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string UserId)
        {
            Guid id = Guid.Parse(UserId);
            return await _userReadRepository.GetAll().FirstOrDefaultAsync(u=>u.UserId == id);
        }

        public async Task<User> UpdateUserAsync(UserDTO userDTO)
        {
            User updateUser = new User();
            return await _userWriteRepository.Update(updateUser);
        }
    }
}
