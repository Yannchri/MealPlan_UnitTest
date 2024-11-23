using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using Moq;

namespace MealPlan_UnitTest;

public class MealPlanManagementUt
{
    private readonly MealPlanService _mealPlanService;

    public MealPlanManagementUt()
    {
        Mock<IMealPlanRepository> mockMealPlanRepo = new();
        Mock<IUserRepository> mockUserRepo = new();
        
        _mealPlanService = new MealPlanService(mockMealPlanRepo.Object, mockUserRepo.Object);
    }
    
    [Fact]
    public void SubscribeToPlan_ShouldDecreaseUserCredits()
    {
        // Arrange
        var mealPlan = new MealPlan { Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2024, 11, 18), endDate = new DateTime(2024, 11, 22), Price = 35};
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch"};
        // Act
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Assert
        Assert.Equal(15, user.Credits);
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfNotEnoughCredits()
    {
        // Arrange
        var mealPlan = new MealPlan { Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2024, 11, 18), endDate = new DateTime(2024, 11, 22), Price = 35};
        var user = new User { Id = 1, Name = "John Doe", Credits = 10, Email = "john.doe@test.ch"};

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }
    
    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfUserAlreadySubscribed()
    {
        // Arrange
        var mealPlan = new MealPlan { Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2024, 11, 18), endDate = new DateTime(2024, 11, 22), Price = 35};
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch", MealPlanId = 1};
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfMealPlanDoesNotExist()
    {
        // Arrange
        var mealPlan = new MealPlan
        {
            Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2024, 11, 18),
            endDate = new DateTime(2024, 11, 22), Price = 35
        };
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch", MealPlanId = 2 };
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfUserDoesNotExist()
    {
        // Arrange
        var mealPlan = new MealPlan
        {
            Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2024, 11, 18),
            endDate = new DateTime(2024, 11, 22), Price = 35
        };
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch", MealPlanId = 1 };
        _mealPlanService.SubscribeToPlan(2, mealPlan.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(2, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfMealPlanIsExpired()
    {
        // Arrange
        var mealPlan = new MealPlan
        {
            Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2022, 11, 18),
            endDate = new DateTime(2022, 11, 22), Price = 35
        };
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch", MealPlanId = 1 };
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }

    [Fact]
    public void SubscribeToPlan_ShouldNotAllowIfMealPlanIsNotStarted()
    {
        // Arrange
        var mealPlan = new MealPlan
        {
            Id = 1, Name = "Student Plan - Weekly", startDate = new DateTime(2025, 11, 18),
            endDate = new DateTime(2025, 11, 22), Price = 35
        };
        var user = new User { Id = 1, Name = "John Doe", Credits = 50, Email = "john.doe@test.ch", MealPlanId = 1 };
        _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
    }
}
