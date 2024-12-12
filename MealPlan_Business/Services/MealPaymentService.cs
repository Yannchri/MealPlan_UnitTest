using MealPlan_Business.Models;
using MealPlan_Business.Repositories;

namespace MealPlan_Business.Services;

public class MealPaymentService(
    IMealPlanRepository mealPlanRepository,
    IUserRepository userRepository,
    List<MealTransaction> _transactions) : IMealPaymentService
{
    public bool ProcessMealPayment(int userId, decimal amount, int mealId)
    {
        var user = userRepository.GetUserById(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        decimal price = mealPlanRepository.GetMealPlanPrice(mealId);

        if (price == 0)
            throw new InvalidOperationException("Meal plan not found");

        if (user.Credits < price)
            return false;

        user.Credits -= price;

        AddUserMealTransactions(userId, mealId);

        return true;
    }

    public void AddUserMealTransactions(int userId, int mealId)
    {
        // Enregistrer la transaction
        var transaction = new MealTransaction
        {
            Id = _transactions.Count + 1, // Générer un nouvel ID
            UserId = userId,
            Amount = mealPlanRepository.GetMealPlanPrice(mealId),
            Date = DateTime.Now
        };

        _transactions.Add(transaction);
    }
}