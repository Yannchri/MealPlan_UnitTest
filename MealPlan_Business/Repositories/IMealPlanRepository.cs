using MealPlan_Business.Models;

namespace MealPlan_Business.Repositories;

public interface IMealPlanRepository
{
    MealPlan GetMealPlanById(int mealPlanId);
    void AddMealPlan(MealPlan mealPlan);
}