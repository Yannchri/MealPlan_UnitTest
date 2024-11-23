using MealPlan_Business.Models;

namespace MealPlan_Business.Repositories;

public interface IUserRepository
{
    User GetUserById(int userId);
}