using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using Moq;
using Xunit;

namespace MealPlan_UnitTest;

public class MealPaymentProcessingUt
{
    private readonly Mock<IMealPlanRepository> _mockMealPlanRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly MealPaymentService _paymentService;
    private readonly List<MealTransaction> _transactions = new List<MealTransaction>();

    public MealPaymentProcessingUt()
    {
        //Configuration of the mock objects
        _mockMealPlanRepository = new Mock<IMealPlanRepository>();
        _mockUserRepository = new Mock<IUserRepository>();

        //Price of the meal
        _mockMealPlanRepository.Setup(service => service.GetMealPlanPrice(1)).Returns(50.0m);
        _mockMealPlanRepository.Setup(service => service.GetMealPlanPrice(2)).Returns(100.0m);
        _mockMealPlanRepository.Setup(service => service.GetMealPlanPrice(It.Is<int>(id => id < 0)))
            .Throws(new InvalidOperationException("Meal plan not found"));

        //Users with credits
        _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(new User { Id = 1, Name = "John Doe", Credits = 100.0m });
        _mockUserRepository.Setup(repo => repo.GetUserById(2)).Returns(new User { Id = 2, Name = "Jane Smith", Credits = 50.0m });
        _mockUserRepository.Setup(repo => repo.GetUserById(3)).Returns(new User { Id = 3, Name = "Alice Brown", Credits = 0.0m });
        _mockUserRepository.Setup(repo => repo.GetUserById(It.Is<int>(id => id < 0)))
            .Throws(new InvalidOperationException("User not found"));

        //Instanciate the services
        _paymentService = new MealPaymentService(_mockMealPlanRepository.Object, _mockUserRepository.Object, _transactions);
    }

    [Fact]
    public void ProcessMealPayment_ShouldThrowException_WhenUserIsInvalid()
    {
        // Arrange
        int userId = -1, mealId = 1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
    }

    [Fact]
    public void ProcessMealPayment_ShouldThrowExceptionWithCorrectMessage_WhenUserPlanNotFound()
    {
        // Arrange
        int userId = 4; //Invalid User
        int mealId = 1; 

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
            _paymentService.ProcessMealPayment(userId, 50.0m, mealId));

        // Assert
        Assert.Equal("User not found", exception.Message);
    }


    [Fact]
    public void ProcessMealPayment_ShouldThrowException_WhenMealPlanNotFound()
    {
        // Arrange
        int userId = 1, mealId = -1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
    }

    [Fact]
    public void ProcessMealPayment_ShouldThrowExceptionWithCorrectMessage_WhenMealPlanNotFound()
    {
        // Arrange
        int userId = 1; // Utilisateur valide
        int mealId = 4; // Plan de repas inexistant

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
            _paymentService.ProcessMealPayment(userId, 50.0m, mealId));

        // Assert
        Assert.Equal("Meal plan not found", exception.Message);
    }

    [Fact]
    public void ProcessMealPayment_ShouldReturnFalse_WhenInsufficientCredits()
    {
        // Arrange
        int userId = 2, mealId = 2;

        // Act
        var result = _paymentService.ProcessMealPayment(userId, 100.0m, mealId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ProcessMealPayment_ShouldReturnTrue_WhenValid()
    {
        // Arrange
        int userId = 1, mealId = 1;

        // Act
        var result = _paymentService.ProcessMealPayment(userId, 50.0m, mealId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void AddUserMealTransactions_ShouldAddTransaction_WhenValid()
    {
        // Arrange
        int userId = 1, mealId = 1;

        // Act
        _paymentService.AddUserMealTransactions(userId, mealId);

        // Assert
        Assert.Single(_transactions);
        Assert.Equal(50.0m, _transactions.First().Amount);
    }

    [Fact]
    public void Transactions_ShouldRemainEmpty_WhenNoTransactionAdded()
    {
        // Assert
        Assert.Empty(_transactions);
    }

    [Fact]
    public void Transactions_ShouldContainMultipleEntries_WhenMultipleTransactionsAdded()
    {
        // Arrange
        _paymentService.AddUserMealTransactions(1, 1);
        _paymentService.AddUserMealTransactions(2, 2);

        // Assert
        Assert.Equal(2, _transactions.Count);
    }

    [Fact]
    public void ProcessMealPayment_ShouldDeductCredits_WhenPaymentIsSuccessful()
    {
        // Arrange
        int userId = 1, mealId = 1;

        // Act
        _paymentService.ProcessMealPayment(userId, 50.0m, mealId);

        // Assert
        var user = _mockUserRepository.Object.GetUserById(userId);
        Assert.Equal(50.0m, user.Credits); // 100 - 50
    }

    [Fact]
    public void ProcessMealPayment_ShouldThrowException_WhenMealPlanPriceIsZero()
    {
        // Arrange
        int userId = 1, mealId = 3;
        _mockMealPlanRepository.Setup(repo => repo.GetMealPlanPrice(mealId)).Returns(0m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
    }

    [Fact]
    public void ProcessMealPayment_ShouldNotDeductCredits_WhenPaymentFails()
    {
        // Arrange
        int userId = 2, mealId = 2;

        // Act
        _paymentService.ProcessMealPayment(userId, 100.0m, mealId);

        // Assert
        var user = _mockUserRepository.Object.GetUserById(userId);
        Assert.Equal(50.0m, user.Credits); // 50 - 100
    }

    [Fact]
    public void ProcessMealPayment_ShouldNotAddTransaction_WhenPaymentFails()
    {
        // Arrange
        int userId = 2, mealId = 2;

        // Act
        _paymentService.ProcessMealPayment(userId, 100.0m, mealId);

        // Assert
        Assert.Empty(_transactions);
    }


}

