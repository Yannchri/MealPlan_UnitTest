using MealPlan_Business.Models;

namespace MealPlan_Business.Repositories;

public class UserRepository : IUserRepository
{
    public User GetUserById(int userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsersByMealPlanId(int mealPlanId)
    {
        throw new NotImplementedException();
    }
}