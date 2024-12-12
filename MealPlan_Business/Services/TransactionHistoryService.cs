using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using System;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MealPlan_Business
{
 
    public class TransactionHistoryService : ITransactionHistoryService
    {
        // Repository to handle data access for transaction history.
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        public TransactionHistoryService(ITransactionHistoryRepository transactionHistoryRepository)
        {
            // Ensure the repository is not null to prevent runtime errors.
            _transactionHistoryRepository = transactionHistoryRepository ?? throw new ArgumentNullException(nameof(transactionHistoryRepository));
        }

        /// Retrieves all transactions for a given user.
        public IEnumerable<MealTransaction> GetTransactionsHistory(int userId)
        {
            // Validate the provided user ID.
            ValidateUserId(userId);

            // Fetch transactions from the repository.
            var transactions = _transactionHistoryRepository.GetTransactionsByUserId(userId);

            // Explicit check to ensure transactions exist for the given user.
            if (transactions == null || !transactions.Any())
            {
                throw new InvalidOperationException($"No transactions found for user with ID {userId}.");
            }

            return transactions;
        }

        /// Retrieves transactions for a user within a specific date range.
        public IEnumerable<MealTransaction> GetFilteredTransaction(int userId, DateTime startDate, DateTime endDate)
        {
            // Validate the user ID and the date range.
            ValidateUserId(userId);
            ValidateDateRange(startDate, endDate);

            // Fetch transactions within the specified range.
            var transactions = _transactionHistoryRepository.GetTransactionsByPeriodOfTime(userId, startDate, endDate)
                ?? throw new InvalidOperationException("No transactions found for the given date range.");

            return transactions;
        }

        /// Retrieves the latest transactions for a user, limited to a specific number.
        public IEnumerable<MealTransaction> GetLatestTransaction(int userId, int number)
        {
            // Validate the user ID.
            ValidateUserId(userId);

            // If the requested number is less than or equal to 0, return an empty list.
            if (number <= 0) return Enumerable.Empty<MealTransaction>();

            // Retrieve all transactions for the user.
            var transactions = _transactionHistoryRepository.GetTransactionsByUserId(userId)
                ?? throw new InvalidOperationException("Unable to retrieve transactions for this user.");

            // If the requested number exceeds the total transactions, return all transactions.
            return number >= transactions.Count()
                ? transactions
                : _transactionHistoryRepository.GetLastXTransactions(userId, number)
                  ?? throw new InvalidOperationException($"Unable to retrieve the last {number} transactions.");
        }

        public MealTransaction getTransactionbyId(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"Transaction ID must be positive");
            }

            return _transactionHistoryRepository.GetTransactionbyId(id);
        }


        /// Validates that a user ID is a positive integer.
        private static void ValidateUserId(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }

        /// Validates that a date range is valid and logical.
        private static void ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            // Ensure the start date is earlier than or equal to the end date.
            if (startDate > endDate)
                throw new ArgumentException("Start date must be earlier than or equal to the end date.");
        }
    }
}
