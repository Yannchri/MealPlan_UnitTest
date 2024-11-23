using MealPlan_Business.Models;

namespace MealPlan_Business.Services;

public interface IMealPaymentService
{
    bool ProcessMealPayment(int userId, decimal amount);
    List<MealTransaction> GetUserMealTransactions(int userId);
}