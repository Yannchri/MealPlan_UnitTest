using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Services
{
    public class UserService (IUserRepository userRepository) : IUserService
    {
        public User GetUserById(int userId)
        {
            var user = userRepository.GetUserById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");
            return user;
        }
    }
}
