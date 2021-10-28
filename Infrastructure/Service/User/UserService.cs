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
            if (!string.IsNullOrEmpty(userId))
            {
                User User = await GetUserByIdAsync(userId);
                await _userWriteRepository.Delete(User);
            }
            else
            {
                throw new Exception("Review Id provided does not exist");
            }            
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userReadRepository.GetAll().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string UserId)
        {
            try
            {
                Guid id = Guid.Parse(UserId);
                var User =  await _userReadRepository.GetAll().FirstOrDefaultAsync(u => u.UserId == id);
                
                if(User == null)
                {
                    throw new Exception("The user you are looking for does not exist");
                }

                return User;
            }
            catch
            {
                throw new Exception("Please provide a proper GUID for users");
            }
        }

        public async Task<User> UpdateUserAsync(UserDTO userDTO, string userId)
        {
            User userInfo = await GetUserByIdAsync(userId);
            //update user info.
            userInfo.FirstName = userDTO.FirstName;
            userInfo.LastName = userDTO.LastName;
            userInfo.Address = userDTO.Address;
            userInfo.Email = userDTO.Email;

            return await _userWriteRepository.Update(userInfo);
        }
    }
}
