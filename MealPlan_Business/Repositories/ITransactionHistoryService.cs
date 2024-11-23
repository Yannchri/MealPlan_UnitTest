using MealPlan_Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Repositories
{
    public  interface ITransactionHistoryService
    {
        public IEnumerable<MealTransaction> GetTransactionsHistory (int userId);

        public IEnumerable<MealTransaction> GetFilteredTransaction (int userId, DateOnly startDate, DateOnly endDate);
        public IEnumerable<MealTransaction> GetLatestTransaction(int userId, int limit);

    }
}
