using MealPlan_Business;
using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using Moq;

namespace MealPlan_UnitTest;
public class TransactionsHistoryUT
{
    // Mock repository for simulating data access without a real database.
    private readonly Mock<ITransactionHistoryRepository> _mockTransactionRepo;

    // The service under test, initialized with the mocked repository.
    private readonly TransactionHistoryService _transactionService;

    public TransactionsHistoryUT()
    {
        // Initialize the mock repository.
        _mockTransactionRepo = new Mock<ITransactionHistoryRepository>();

        // Inject the mocked repository into the service.
        _transactionService = new TransactionHistoryService(_mockTransactionRepo.Object);

        // Predefined fake data to be used in the tests.
        var fakeTransactions = GenerateFakeTransactions();
        var startDate = new DateTime(2024, 10, 01);
        var endDate = new DateTime(2024, 10, 02);

        // A specific transaction used for certain test cases.
        var fakeTransaction = new MealTransaction
        {
            Id = 2,
            Amount = 30.0m,
            Date = new DateTime(2024, 10, 02),
            UserId = 1
        };

        // Configure mock behaviors for the repository methods.

        // Returns all transactions for user ID 1.
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsByUserId(1)).Returns(fakeTransactions);

        // Returns an empty list for invalid user ID 5.
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsByUserId(5)).Returns(new List<MealTransaction>());

        // Returns transactions within the specified date range for user ID 1.
        _mockTransactionRepo.Setup(repo => repo.GetTransactionsByPeriodOfTime(1, startDate, endDate))
                     .Returns(fakeTransactions);

        // Returns the latest transaction for user ID 1 with a limit of 1.
        _mockTransactionRepo.Setup(repo => repo.GetLastXTransactions(1, 1))
                            .Returns(new List<MealTransaction> { fakeTransaction });
    }

    /// Generates a predefined list of fake transactions for use in tests.
    private static List<MealTransaction> GenerateFakeTransactions() => new()
    {
        new MealTransaction { Id = 1, Amount = 50.0m, Date = new DateTime(2024, 10, 01), UserId = 1 },
        new MealTransaction { Id = 2, Amount = 30.0m, Date = new DateTime(2024, 10, 02), UserId = 1 },
    };


    [Fact]
    public void GetTransactionsHistory_ValidUser_ShouldReturnAllTransactions()
    {
        // Act
        var result = _transactionService.GetTransactionsHistory(1);

        // Assert: Check that the result is not empty and contains the expected number of transactions.
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetTransactionsHistory_InvalidUser_ShouldThrowException()
    {
        // Act & Assert: Ensure that calling the method with an invalid user ID throws an exception.
        Assert.Throws<InvalidOperationException>(() => _transactionService.GetTransactionsHistory(5));
    }

    [Fact]
    public void GetFilteredTransaction_ValidDateRange_ShouldReturnTransactions()
    {
        // Arrange: Define a date range for the test.
        var startDate = new DateTime(2024, 10, 01);
        var endDate = new DateTime(2024, 10, 02);

        // Act
        var result = _transactionService.GetFilteredTransaction(1, startDate, endDate);

        // Assert: Check that the result contains the expected number of transactions.
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetFilteredTransaction_InvalidDateRange_ShouldThrowException()
    {
        // Arrange: Define an invalid date range.
        var startDate = new DateTime(2024, 10, 02);
        var endDate = new DateTime(2024, 10, 01);

        // Act & Assert: Ensure that calling the method with an invalid range throws an exception.
        Assert.Throws<ArgumentException>(() => _transactionService.GetFilteredTransaction(1, startDate, endDate));
    }

 
    [Fact]
    public void GetFilteredTransaction_NoTransactionsInDateRange_ShouldReturnEmptyList()
    {
        // Arrange: Define a date range with no transactions.
        var startDate = new DateTime(2024, 11, 01);
        var endDate = new DateTime(2024, 11, 02);

        // Act
        var result = _transactionService.GetFilteredTransaction(1, startDate, endDate);

        // Assert: Check that the result is an empty list.
        Assert.Empty(result);
    }

 
    [Fact]
    public void GetLatestTransaction_ValidNumber_ShouldReturnSpecifiedTransactions()
    {
        // Act
        var result = _transactionService.GetLatestTransaction(1, 1);

        // Assert: Check that the result contains a single transaction with the expected date.
        Assert.Single(result);
        Assert.Equal(new DateTime(2024, 10, 02), result.First().Date);
    }

    [Fact]
    public void GetLatestTransaction_NumberExceedsTotal_ShouldReturnAllTransactions()
    {
        // Act
        var result = _transactionService.GetLatestTransaction(1, 10);

        // Assert: Check that all transactions are returned.
        Assert.Equal(2, result.Count());
    }


    [Fact]
    public void GetLatestTransaction_NegativeNumber_ShouldReturnEmptyList()
    {
        // Act
        var result = _transactionService.GetLatestTransaction(1, -1);

        // Assert: Check that the result is an empty list.
        Assert.Empty(result);
    }

    [Fact]
    public void GetUserID_NegativeNumber_ShouldReturnError()
    {
        // Act
        var userid = - 1;

        // Assert: Check that the result is an empty list.
        Assert.Throws<ArgumentException>(() => _transactionService.GetTransactionsHistory(userid));
    }
}
