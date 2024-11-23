using MealPlan_Business;
using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using Moq;

namespace MealPlan_UnitTest;

public class MealPlanUT
{
    private List<MealTransaction> fakeTransactions;

    //Générer une liste qui va être réutilisée, je suis sûr que y'a un autre moyen mais ça fonctionnait pas
    // j'attends de voir le code des collègues
    private List<MealTransaction> CreateFakeTransactions()
    {
        fakeTransactions =
        [
            new MealTransaction { Id = 1, Amount = 50.0m, Date = new DateTime(2024, 10, 01), UserId = 1 },
            new MealTransaction { Id = 2, Amount = 30.0m, Date = new DateTime(2024, 10, 02), UserId = 2 },
            new MealTransaction { Id = 3, Amount = 75.0m, Date = new DateTime(2024, 10, 03), UserId = 3 },
            new MealTransaction { Id = 4, Amount = 100.0m, Date = new DateTime(2024, 10, 04), UserId = 1 },
            new MealTransaction { Id = 5, Amount = 40.0m, Date = new DateTime(2024, 10, 05), UserId = 2 },
            new MealTransaction { Id = 6, Amount = 60.0m, Date = new DateTime(2024, 10, 06), UserId = 3 },
            new MealTransaction { Id = 7, Amount = 20.0m, Date = new DateTime(2024, 10, 07), UserId = 1 },
            new MealTransaction { Id = 8, Amount = 90.0m, Date = new DateTime(2024, 10, 08), UserId = 2 },
            new MealTransaction { Id = 9, Amount = 10.0m, Date = new DateTime(2024, 10, 09), UserId = 3 },
            new MealTransaction { Id = 10, Amount = 25.0m, Date = new DateTime(2024, 10, 10), UserId = 1 }
        ];
        return fakeTransactions;
    }


    [Fact]
    public void GetAllTheTransactionForUser1_ShouldReturn4Transactions()
    {
        // Arrange : set up data with 10 transactions
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        // Act = appeler la méthode sous test
        var result = transactionService.GetTransactionsHistory(1); // Utilisateur avec ID = 1

        // Assert = vérifier le résultat
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public void GetAllTransactionFromInexistingUser_ShouldReturn0Transaction()
    {
        // Arrange : set up data with 10 transactions
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();
        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);


        // Act = appeler la méthode sous test
        var result = transactionService.GetTransactionsHistory(5); // Utilisateur avec ID = 5

        // Assert = vérifier le résultat
        Assert.Equal(0, result.Count());
    }

    [Fact]
    public void GetAllTransactionFromInexistingUser_ShouldReturnErrorMessage()
    {
        // Arrange : set up data with 10 transactions
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 5
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);


        // Act = appeler la méthode sous test
        var result = transactionService.GetTransactionsHistory(5); // Utilisateur avec ID = 5

        var errormessage = transactionService.GetErrorMessage();

        // Assert = vérifier le résultat
        Assert.Equal("User doesn't exist", errormessage); // Vérifie que le message d'erreur est le bon
    }

    [Fact]
    public void GetFilterTransactionForToday_ShouldReturnTransactionOfTheDay()
    {
        // Arrange : set up data with several transactions, including one for today
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        //Charlie Brown user ID 3
        var today = new DateTime(2024, 10, 09);

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        // Act = appeler la méthode sous test
        var result = transactionService.GetFilteredTransaction(3, today, today); // Filtrer pour aujourd'hui

        // Assert = vérifier le résultat
        Assert.Single(result);
    }

    [Fact]
    public void GetFilterTransactionAPeriod_ShouldReturnTransactionOfThePeriod()
    {
        // Arrange : set up data with several transactions, including one for today
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        var firstDate = new DateTime(2024, 10, 05);
        var secondDate = new DateTime(2024, 10, 10);

        // Act = appeler la méthode sous test
        var result =
            transactionService.GetFilteredTransaction(3, firstDate,
                secondDate); // Filtrer dans une plage de 2 entrée sur 3

        // Assert = vérifier le résultat
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void VerifyTheFirstDateAndLastDate_ShouldReturnAnErrorMessage()
    {
        // Arrange : set up data with several transactions, including one for today
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        var firstDate = new DateTime(2024, 10, 05);
        var secondDate = new DateTime(2024, 10, 10);

        // Act = appeler la méthode sous test
        var result =
            transactionService.GetFilteredTransaction(3, secondDate,
                firstDate); // Filtrer dans une plage de 2 entrée sur 3

        // Assert = vérifier le résultat
        Assert.Equal("First date must be older than second date", transactionService.GetErrorMessage());
    }

    [Fact]
    public void GetTheLast2Transactions_ShouldReturn2TransactionsAndTheGoodDate()
    {
        // Arrange : set up data with several transactions, including one for today
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        // Act = appeler la méthode sous test
        var result = transactionService.GetLatestTransaction(1, 2); //retourne les 2 dernières transactions sur 4 

        // Assert = vérifier le résultat
        Assert.Equal(2, result.Count());

        var lastTransactions = result.ToList();
        Assert.Equal(new DateTime(2024, 10, 10), lastTransactions[0].Date); // Vérifie la première date
        Assert.Equal(new DateTime(2024, 10, 07), lastTransactions[1].Date); // Vérifie la deuxième date
    }

    [Fact]
    public void AskMoreThanTheMaxTransaction_ShouldReturnAllTheTransactions()
    {
        // Arrange : set up data with several transactions
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        // Act = appeler la méthode sous test
        var result = transactionService.GetLatestTransaction(1, 10); //Demande 10 transactions mais il y en 4

        // Assert = vérifier le résultat
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public void GetAllTransactionsWithNegativeNumber_ShouldReturnEmptyList()
    {
        // Arrange : set up data with several transactions
        var mockTransactionRepo = new Mock<ITransactionHistoryService>();
        List<MealTransaction> fakeTransaction = CreateFakeTransactions();

        // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
        mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

        // Instanciation du service avec le mock injecté
        var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

        // Act = appeler la méthode sous test
        var result = transactionService.GetLatestTransaction(1, -1); //negative number

        // Assert = vérifier le résultat
        Assert.Empty(result);
    }
}