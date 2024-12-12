using MealPlan_Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_Business.Repositories
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        public IEnumerable<MealTransaction> GetLastXTransactions(int userId, int numberOfTransactions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MealTransaction> GetTransactionsByPeriodOfTime(int userId, DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MealTransaction> GetTransactionsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public MealTransaction GetTransactionbyId(int id ) { 
            throw new NotImplementedException(); 
        }
    }
}
