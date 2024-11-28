using MealPlan_Business;
using MealPlan_Business.Models;
using MealPlan_Business.Services;
using Moq;

namespace MealPlan_UnitTest;

public class TransactionsHistoryUT
{
    private readonly Mock<ITransactionHistoryService> _mockTransactionRepo;
    private readonly TransactionHistoryService _transactionService;

    public TransactionsHistoryUT()
    {
        _mockTransactionRepo = new Mock<ITransactionHistoryService>();
        _transactionService = new TransactionHistoryService(_mockTransactionRepo.Object);
    }

    private static List<MealTransaction> GenerateFakeTransactions() => new()
    {
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
    };

    [Fact]
    public void GetAllTheTransactionForUser1_ShouldReturn4Transactions()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1))
                            .Returns(transactions.Where(t => t.UserId == 1).ToList());

        // Act
        var result = _transactionService.GetTransactionsHistory(1);

        // Assert
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public void GetAllTransactionFromInexistingUser_ShouldReturn0Transaction()
    {
        // Arrange
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(new List<MealTransaction>());

        // Act
        var result = _transactionService.GetTransactionsHistory(5);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAllTransactionFromInexistingUser_ShouldReturnErrorMessage()
    {
        // Arrange
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(new List<MealTransaction>());

        // Act
        var result = _transactionService.GetTransactionsHistory(5);
        var errorMessage = _transactionService.GetErrorMessage();

        // Assert
        Assert.Equal("User doesn't exist", errorMessage);
    }

    [Fact]
    public void GetFilterTransactionForToday_ShouldReturnTransactionOfTheDay()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        var today = new DateTime(2024, 10, 09);
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3))
                            .Returns(transactions.Where(t => t.UserId == 3).ToList());

        // Act
        var result = _transactionService.GetFilteredTransaction(3, today, today);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public void GetFilterTransactionAPeriod_ShouldReturnTransactionOfThePeriod()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        var firstDate = new DateTime(2024, 10, 05);
        var secondDate = new DateTime(2024, 10, 10);
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3))
                            .Returns(transactions.Where(t => t.UserId == 3).ToList());

        // Act
        var result = _transactionService.GetFilteredTransaction(3, firstDate, secondDate);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void VerifyTheFirstDateAndLastDate_ShouldReturnAnErrorMessage()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        var firstDate = new DateTime(2024, 10, 10);
        var secondDate = new DateTime(2024, 10, 05);
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3))
                            .Returns(transactions.Where(t => t.UserId == 3).ToList());

        // Act
        var result = _transactionService.GetFilteredTransaction(3, firstDate, secondDate);
        var errorMessage = _transactionService.GetErrorMessage();

        // Assert
        Assert.Equal("First date must be older than second date", errorMessage);
    }

    [Fact]
    public void GetTheLast2Transactions_ShouldReturn2TransactionsAndTheGoodDate()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1))
                            .Returns(transactions.Where(t => t.UserId == 1).ToList());

        // Act
        var result = _transactionService.GetLatestTransaction(1, 2);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(new DateTime(2024, 10, 10), result.First().Date);
        Assert.Equal(new DateTime(2024, 10, 07), result.Last().Date);
    }

    [Fact]
    public void AskMoreThanTheMaxTransaction_ShouldReturnAllTheTransactions()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1))
                            .Returns(transactions.Where(t => t.UserId == 1).ToList());

        // Act
        var result = _transactionService.GetLatestTransaction(1, 10);

        // Assert
        Assert.Equal(4, result.Count());
    }

    [Fact]
    public void GetAllTransactionsWithNegativeNumber_ShouldReturnEmptyList()
    {
        // Arrange
        var transactions = GenerateFakeTransactions();
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1))
                            .Returns(transactions.Where(t => t.UserId == 1).ToList());

        // Act
        var result = _transactionService.GetLatestTransaction(1, -1);

        // Assert
        Assert.Empty(result);
    }
}
