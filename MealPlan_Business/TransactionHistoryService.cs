using MealPlan_Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealPlan_Business.Models;

namespace MealPlan_Business
{
    public class TransactionHistoryService
    {

        private readonly ITransactionHistoryService _transactionHistoryService;
        private string _errorMessage;

        public TransactionHistoryService (ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;

        }


        public IEnumerable<Transaction> GetTransactionsHistory(int userId)
        {
            var transactions = _transactionHistoryService.GetTransactionsHistory(userId).Where(t => t.User.Id == userId);

            // Vérifier si aucune transaction n'a été trouvée
            if (!transactions.Any())
            {
                _errorMessage = "User doesn't exist";
                return new List<Transaction>(); // Retourne une liste vide si l'utilisateur n'existe pas
            }

            return transactions;

        }

        public IEnumerable<Transaction> GetFilteredTransaction(int userId, DateOnly startDate, DateOnly endDate)
        {
           
            var transactions = _transactionHistoryService.GetTransactionsHistory(userId)
                .Where(t => t.User.Id == userId); // Accéder à l'ID via l'objet User

            // Filtrer les transactions selon la plage de dates
            var filteredTransactions = transactions.Where(t => t.Date >= startDate && t.Date <= endDate);

            if(startDate>endDate)
            {
                _errorMessage = "First date must be older than second date";
                return new List<Transaction>();
            }

            return filteredTransactions;
        }



        public IEnumerable<Transaction> GetLatestTransaction(int userId, int limit)
        {
            
            var transactions = _transactionHistoryService.GetTransactionsHistory(userId)
                .Where(t => t.User.Id == userId) // Accéder à l'ID via l'objet User
                .OrderByDescending(t => t.Date) // Trier les transactions par date décroissante
                .Take(limit); // Prendre seulement les X dernières transactions

            return transactions;
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

    }


}

