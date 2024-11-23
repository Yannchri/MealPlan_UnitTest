namespace MealPlan_Business.Services;

public interface IMealPlanService
{
    public void SubscribeToPlan(int userId, int mealPlanId);
}