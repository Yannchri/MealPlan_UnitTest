using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using MealPlan_Business.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealPlan_UnitTest
{
    public class UserServiceUT
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private object _mockUserRepository;

        public UserServiceUT()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockUserRepo.Setup(repo => repo.GetUserById(1)).Returns(new User { Id = 1, Name = "John Doe", Credits = 100.0m });

            _userService = new UserService(_mockUserRepo.Object);
        }

        [Fact]
        public void GetUserById_ShouldReturnUser()
        {
            // Arrange
            var user = _mockUserRepo.Object.GetUserById(1);

            // Act
            var result = _userService.GetUserById(user.Id);

            // Assert
            Assert.Equal(user, result);
            Assert.Equal(user.Name, result.Name);
        }

        [Fact]
        public void GetUserById_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            int userId = -1;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _userService.GetUserById(userId));
        }

        [Fact]
        public void GetUserById_ShouldThrowExceptionWithCorrectMessage_WhenUserNotFound()
        {
            // Arrange
            int userId = -1;

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _userService.GetUserById(userId));

            // Assert
            Assert.Equal("User not found", exception.Message);
        }

    }
}
