using MealPlan_Business.Models;
using MealPlan_Business.Repositories;

namespace MealPlan_Business.Services;

public class MealPlanService(IMealPlanRepository mealPlanRepository, IUserRepository userRepository) : IMealPlanService
{
    public void SubscribeToPlan(int userId, int mealPlanId)
    {
        throw new NotImplementedException();

    }
}