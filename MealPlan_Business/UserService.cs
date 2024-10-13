using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business
{
    public class UserService : IUserRepository
    {
        private readonly IUserRepository _userRepository;
        private string _errorMessage;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUserById(int userId)
        {
            User user = (User)_userRepository.GetUserById(userId);

            if (user == null)
            {
                _errorMessage = "User doesn't exist";
                return null;
            }

            return user;
        }
    }
}
