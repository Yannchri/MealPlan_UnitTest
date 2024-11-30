using MealPlan_Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Repositories
{
    public  interface ITransactionHistoryRepository
    {
        IEnumerable<MealTransaction> GetTransactionsByUserId(int userId);

        IEnumerable<MealTransaction> GetTransactionsByPeriodOfTime(int userId, DateTime startDate, DateTime endDate);

        IEnumerable<MealTransaction> GetLastXTransactions(int userId, int numberOfTransactions);

    }
}
