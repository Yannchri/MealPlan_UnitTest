using MealPlan_Business.Models;

namespace MealPlan_Business.Services;

public interface IMealPlanService
{
    public void SubscribeToPlan(int userId, int mealPlanId);
    public void CreateMealPlan(MealPlan mealPlan);
    public IEnumerable<User> GetSubscribedUsers(int mealPlanId);
}