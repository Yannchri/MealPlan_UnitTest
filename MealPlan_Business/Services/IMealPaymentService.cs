namespace MealPlan_Business.Services;

public interface IMealPaymentService
{
    bool ProcessMealPayment(int userId, decimal amount, int mealId);
    void AddUserMealTransactions(int userId, int mealId);
}