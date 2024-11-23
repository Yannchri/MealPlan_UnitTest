using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using Moq;

namespace MealPlan_UnitTest;

public class MealPlanManagementUt
{
    private readonly MealPlanService _mealPlanService;
    private readonly Mock<IMealPlanRepository> _mockMealPlanRepo;
    private readonly Mock<IUserRepository> _mockUserRepo;
    public MealPlanManagementUt()
    {
        _mockMealPlanRepo = new Mock<IMealPlanRepository>();
        _mockUserRepo = new Mock<IUserRepository>();
        
        // Set up the mock objects
        // Setup method to add a meal plan
        _mockMealPlanRepo.Setup(repo => repo.AddMealPlan(It.IsAny<MealPlan>())).Verifiable();
        // Plan with id 1 is active
        _mockMealPlanRepo.Setup(x => x.GetMealPlanById(1)).Returns(new MealPlan { Id = 1, Name = "Student Plan - Weekly - Valid", startDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)), endDate = DateTime.Now.Add(TimeSpan.FromDays(5)), Price = 35});
        // Plan with id 2 is not active
        _mockMealPlanRepo.Setup(x => x.GetMealPlanById(2)).Returns(new MealPlan { Id = 2, Name = "Student Plan - Weekly - Invalid", startDate = DateTime.Now.Add(TimeSpan.FromDays(5)), endDate = DateTime.Now.Add(TimeSpan.FromDays(10)), Price = 35});
        // User with id 1 has enough credits
        _mockUserRepo.Setup(x => x.GetUserById(1)).Returns(new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch"});
        // User with id 2 does not have enough credits
        _mockUserRepo.Setup(x => x.GetUserById(2)).Returns(new User { Id = 2, Name = "Jane Doe", Credits = 10, Email = "jane.doe@test.ch"});
        // User with id 3 is already subscribed to a plan
        _mockUserRepo.Setup(x => x.GetUserById(3)).Returns(new User { Id = 3, Name = "Jack Doe", Credits = 50, Email = "jack.doe@test.ch", MealPlanId = 1});
        _mealPlanService = new MealPlanService(_mockMealPlanRepo.Object, _mockUserRepo.Object);
    }
    
    [Fact]
    public void SubscribeToPlan_ShouldDecreaseUserCredits()
    {
        // Arrange
        var user = _mockUserRepo.Object.GetUserById(1);
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(1);
        
        // Act
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Assert
        Assert.Equal(15, user.Credits);
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfNotEnoughCredits()
    {
        // Arrange
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(1);
        var user = _mockUserRepo.Object.GetUserById(2);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }
    
    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfUserAlreadySubscribed()
    {
        // Arrange
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(1);
        var user = _mockUserRepo.Object.GetUserById(3);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfMealPlanDoesNotExist()
    {
        // Arrange
        var mealPlanId = -1;
        var user = _mockUserRepo.Object.GetUserById(1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlanId));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfUserDoesNotExist()
    {
        // Arrange
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(1);
        var userId = -1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(userId, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfMealPlanIsNotActive()
    {
        // Arrange
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(2);
        var user = _mockUserRepo.Object.GetUserById(1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }
    [Fact]
    public void CreateMealPlan_ShouldAddMealPlanToRepository()
    {
        // Arrange
        var newMealPlan = new MealPlan { Id = 3, Name = "New Plan", startDate = DateTime.Now, endDate = DateTime.Now.AddDays(7), Price = 50 };

        // Act
        _mealPlanService.CreateMealPlan(newMealPlan);

        // Assert
        _mockMealPlanRepo.Verify(repo => repo.AddMealPlan(It.Is<MealPlan>(mp => mp.Id == newMealPlan.Id && mp.Name == newMealPlan.Name)), Times.Once);
    }
    
    [Fact]
    public void CreateMealPlan_ShouldNotAllowEmptyName()
    {
        // Arrange
        var newMealPlan = new MealPlan { Id = 3, Name = "", startDate = DateTime.Now, endDate = DateTime.Now.AddDays(7), Price = 50 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
    }
    
    [Fact]
    public void CreateMealPlan_ShouldNotAllowStartDateAfterEndDate()
    {
        // Arrange
        var newMealPlan = new MealPlan { Id = 3, Name = "New Plan", startDate = DateTime.Now.AddDays(7), endDate = DateTime.Now, Price = 50 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
    }
    
    [Fact]
    public void CreateMealPlan_ShouldNotAllowPriceLessOrEqualToZero()
    {
        // Arrange
        var newMealPlan = new MealPlan { Id = 3, Name = "New Plan", startDate = DateTime.Now, endDate = DateTime.Now.AddDays(7), Price = 0 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
    }

    [Fact]
    public void GetSubscribedUsers_ShouldNotAllowIfMealPlanDoesNotExist()
    {
        // Arrange
        var mealPlanId = -1;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.GetSubscribedUsers(mealPlanId));
    }
    
    [Fact]
    public void GetSubscribedUsers_ShouldReturnEmptyListIfNoUsersSubscribed()
    {
        // Arrange
        var mealPlan = _mockMealPlanRepo.Object.GetMealPlanById(2);

        // Act
        var users = _mealPlanService.GetSubscribedUsers(mealPlan.Id);

        // Assert
        Assert.Empty(users);
    }
}
