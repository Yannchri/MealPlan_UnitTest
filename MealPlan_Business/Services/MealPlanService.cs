using MealPlan_Business.Models;
using MealPlan_Business.Repositories;

namespace MealPlan_Business.Services;

public class MealPlanService(IMealPlanRepository mealPlanRepository, IUserRepository userRepository) : IMealPlanService
{
    public void SubscribeToPlan(int userId, int mealPlanId)
    {
        var mealPlan = mealPlanRepository.GetMealPlanById(mealPlanId);
        if (mealPlan == null) throw new InvalidOperationException("Meal plan not found");

        var user = userRepository.GetUserById(userId);
        if (user == null) throw new InvalidOperationException("User not found");

        // Try to subscribe the user to the meal plan
        // Check if user has already subscribed to a meal plan
        if (user.MealPlanId != null) throw new InvalidOperationException("User is already subscribed to a meal plan");

        // Check if the plan is active
        if (DateTime.Now < mealPlan.startDate || DateTime.Now > mealPlan.endDate)
            throw new InvalidOperationException("Meal plan is not active");

        // Check if user has enough credits
        if (user.Credits < mealPlan.Price) throw new InvalidOperationException("User does not have enough credits");

        // Subscribe the user to the meal plan
        user.MealPlanId = mealPlanId;
        user.Credits -= mealPlan.Price;
    }

    public void CreateMealPlan(MealPlan newMealPlan)
    {
        if (newMealPlan == null) throw new ArgumentNullException(nameof(newMealPlan));
        if (string.IsNullOrEmpty(newMealPlan.Name)) throw new ArgumentException("Meal plan name cannot be empty");
        if (newMealPlan.startDate >= newMealPlan.endDate) throw new ArgumentException("Meal plan start date must be before end date");
        if (newMealPlan.Price <= 0) throw new ArgumentException("Meal plan price must be greater than zero");

        mealPlanRepository.AddMealPlan(newMealPlan);
    }
    
    public IEnumerable<User> GetSubscribedUsers(int mealPlanId)
    {
        var mealPlan = mealPlanRepository.GetMealPlanById(mealPlanId);
        if (mealPlan == null) throw new InvalidOperationException("Meal plan not found");

        return userRepository.GetUsersByMealPlanId(mealPlanId);
    }
}