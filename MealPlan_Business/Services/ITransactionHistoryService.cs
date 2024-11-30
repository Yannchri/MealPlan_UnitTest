using MealPlan_Business.Models;

namespace MealPlan_Business.Services;

public interface ITransactionHistoryService
{
    public IEnumerable<MealTransaction> GetTransactionsHistory(int userId);

    public IEnumerable<MealTransaction> GetFilteredTransaction(int userId, DateTime startDate, DateTime endDate);
    public IEnumerable<MealTransaction> GetLatestTransaction(int userId, int limit);
}