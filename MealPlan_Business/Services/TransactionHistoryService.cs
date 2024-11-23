﻿using MealPlan_Business.Models;
using MealPlan_Business.Services;

namespace MealPlan_Business;

public class TransactionHistoryService
{
    private readonly ITransactionHistoryService _transactionHistoryService;
    private string _errorMessage;

    public TransactionHistoryService(ITransactionHistoryService transactionHistoryService)
    {
        _transactionHistoryService = transactionHistoryService;
    }


    public IEnumerable<MealTransaction> GetTransactionsHistory(int userId)
    {
        var transactions = _transactionHistoryService.GetTransactionsHistory(userId).Where(t => t.UserId == userId);

        // Vérifier si aucune transaction n'a été trouvée
        if (!transactions.Any())
        {
            _errorMessage = "User doesn't exist";
            return new List<MealTransaction>(); // Retourne une liste vide si l'utilisateur n'existe pas
        }

        return transactions;
    }

    public IEnumerable<MealTransaction> GetFilteredTransaction(int userId, DateTime startDate, DateTime endDate)
    {
        var transactions = _transactionHistoryService.GetTransactionsHistory(userId)
            .Where(t => t.UserId == userId); // Accéder à l'ID via l'objet User

        // Filtrer les transactions selon la plage de dates
        var filteredTransactions = transactions.Where(t => t.Date >= startDate && t.Date <= endDate);

        if (startDate > endDate)
        {
            _errorMessage = "First date must be older than second date";
            return new List<MealTransaction>();
        }

        return filteredTransactions;
    }


    public IEnumerable<MealTransaction> GetLatestTransaction(int userId, int limit)
    {
        var transactions = _transactionHistoryService.GetTransactionsHistory(userId)
            .Where(t => t.UserId == userId) // Accéder à l'ID via l'objet User
            .OrderByDescending(t => t.Date) // Trier les transactions par date décroissante
            .Take(limit); // Prendre seulement les X dernières transactions

        return transactions;
    }

    public string GetErrorMessage()
    {
        return _errorMessage;
    }
}