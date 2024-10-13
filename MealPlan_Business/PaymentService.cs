using MealPlan_Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealPlan_Business.Models;

namespace MealPlan_Business
{
    public class PaymentService : IPaymentRepository
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUserRepository _userRepository;
        private string _errorMessage;

        public PaymentService(ITransactionHistoryService transactionRepository, IUserRepository userRepository)
        {
            _transactionHistoryService = transactionRepository;
            _userRepository = userRepository;
        }

        public bool ProcessMealPayment(int userId, double amount)
        {
            if (!IsPaymentValid(userId, amount))
            {
                return false;
            }

            User user = _userRepository.GetUserById(userId);

            // Create the transaction
            var transaction = new Transaction
            {
                User = user,
                Amount = amount,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            // Add the transaction via the TransactionHistoryService
            _transactionHistoryService.AddTransaction(transaction);

            return true; // The payment succeed
        }

        public bool IsPaymentValid(int userId, double amount)
        {
            bool result = true;
            if (userId == null)
            {
                _errorMessage = "Invalid user";
                result = false;
            }
            else if (amount <= 0)
            {
                result = false;
                _errorMessage = "Invalid amount";
            }

            return result;
        }


        public string GetErrorMessage()
        {
            return _errorMessage;
        }
    }
}
