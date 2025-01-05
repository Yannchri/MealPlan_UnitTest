# MealPlan Payment Service

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)
![.NET](https://img.shields.io/badge/.NET-6.0-blue.svg)
![GitHub Issues](https://img.shields.io/github/issues/your-repository/MealPlanPaymentService.svg)
![GitHub Pull Requests](https://img.shields.io/github/issues-pr/your-repository/MealPlanPaymentService.svg)

## Table of Contents

1. [Introduction](#introduction)
2. [Project Structure](#project-structure)
3. [Main Components](#main-components)
   - [Models](#models)
   - [Repositories](#repositories)
   - [Services](#services)
   - [Interfaces](#interfaces)
4. [Unit Tests](#unit-tests)
   - [Test Documentation](#test-documentation)
     - [UserServiceUT](#1-userserviceut)
     - [TransactionsHistoryUT](#2-transactionshistoryut)
     - [MealPlanManagementUt](#3-mealplanmanagementut)
     - [MealPaymentProcessingUt](#4-mealpaymentprocessingut)

## Introduction

The **MealPlan Payment Service** is a backend application designed to manage meal plan payments, user subscriptions to these plans, and transaction histories. This project is organized into several modular services, ensuring optimal extensibility and maintainability, with comprehensive unit test coverage to guarantee system reliability.

## Project Structure

The structure of the main files and folders in the project is as follows:

```
MealPlan_Business/
├── Models/
│   ├── User.cs
│   ├── MealPlan.cs
│   ├── MealTransaction.cs
├── Repositories/
│   ├── IUserRepository.cs
│   ├── IMealPlanRepository.cs
│   ├── ITransactionHistoryRepository.cs
│   ├── UserRepository.cs
│   ├── MealPlanRepository.cs
│   ├── TransactionHistoryRepository.cs
├── Services/
│   ├── IUserService.cs
│   ├── IMealPlanService.cs
│   ├── IMealPaymentService.cs
│   ├── ITransactionHistoryService.cs
│   ├── UserService.cs
│   ├── MealPlanService.cs
│   ├── MealPaymentService.cs
│   ├── TransactionHistoryService.cs
MealPlan_UnitTest/
├── UnitTests/
│   ├── MealPaymentProcessingUT.cs
│   ├── MealPlanManagementUT.cs
│   ├── TransactionsHistoryUT.cs
│   ├── UserServiceUT.cs
```


## Main Components

### Models

- **User** ()
  - Represents a user with information such as ID, name, email, credit balance, and associated meal plan ID.

- **MealPlan** ()
  - Represents a meal plan with details like ID, name, price, start date, and end date.

- **MealTransaction** ()
  - Represents a payment transaction with details such as ID, user ID, amount, and date.

### Repositories

- **UserRepository** () : Implements `IUserRepository` to access user data.
- **MealPlanRepository** () : Implements `IMealPlanRepository` to manage meal plans.
- **TransactionHistoryRepository** () : Implements `ITransactionHistoryRepository` to manage transaction histories.

### Services

- **UserService** () : Handles user-related operations, such as retrieving user details.
- **MealPlanService** () : Manages meal plan subscriptions and creation of new plans.
- **MealPaymentService** () : Handles payment processing for meal plans.
- **TransactionHistoryService** () : Manages operations related to transaction histories.

### Interfaces

- **IUserRepository**, **IMealPlanRepository**, **ITransactionHistoryRepository** : Define the necessary methods for data access.
- **IUserService**, **IMealPlanService**, **IMealPaymentService**, **ITransactionHistoryService** : Define the methods for the corresponding services.

## Unit Tests

Unit tests ensure the reliability and maintainability of the project. They cover various scenarios such as validations, exceptions, and critical operations.
We chose to test the service layer mostly to check the business logic and ensure the components interact correctly.

The test are divided into 3 sections related to the corresponding functionality : 

- **Transaction History**  
These tests checks if the service correctly returns all transactions for a valid user. It validates that the service behaves as expected when called with an existing user ID and ensures that all the relevant transactions are retrieved.

- **Mealplan management**  
This suite of unit tests is designed to validate the core functionalities of the MealPlanService class, which is responsible for managing meal plans, user subscriptions, and meal plan-related operations. The tests ensure that the service behaves as expected under a variety of scenarios, including valid and invalid inputs, edge cases, and boundary conditions.

- **Payment processing**  
This suite of unit tests focuses on verifying the functionality of the MealPaymentService class, which is responsible for processing payments related to meal plans. The tests cover various scenarios, ensuring the system behaves as expected under normal and exceptional conditions, and that critical edge cases are handled correctly.

 
### Test Documentation and rationale

#### 1. UserServiceUT

**Objective:** Validate the functionalities of the user service, including retrieving user information and handling exceptions when a user is not found.

**Test File:** `UserServiceUT.cs`

##### Mock Configuration

- **Mocked `IUserRepository`:**
  - **`GetUserById(1)`** returns a valid user:
    ```csharp
    _mockUserRepo.Setup(repo => repo.GetUserById(1))
                 .Returns(new User { Id = 1, Name = "John Doe", Credits = 100.0m });
    ```
  - No other users are configured, allowing simulation of scenarios where the user does not exist.

##### Test Methods

1. **`GetUserById_ShouldReturnUser`**
   - **Purpose:** Verify that the service correctly returns the details of a valid user.
   - **Steps:**
     - **Arrange:** Retrieves the mocked user with `Id = 1`.
     - **Act:** Calls `GetUserById` from the service with the user's ID.
     - **Assert:** Checks that the returned user matches the expected user.
   - **Assertions:**
     ```csharp
     Assert.Equal(user, result);
     Assert.Equal(user.Name, result.Name);
     ```

2. **`GetUserById_ShouldThrowException_WhenUserNotFound`**
   - **Purpose:** Ensure that an exception is thrown when a non-existent user is searched.
   - **Steps:**
     - **Arrange:** Sets an invalid `userId` (`-1`).
     - **Act & Assert:** Verifies that calling `GetUserById` with the invalid ID throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() =>
         _userService.GetUserById(userId));
     ```

3. **`GetUserById_ShouldThrowExceptionWithCorrectMessage_WhenUserNotFound`**
   - **Purpose:** Check that the exception message is correct when a user is not found.
   - **Steps:**
     - **Arrange:** Sets an invalid `userId` (`-1`).
     - **Act:** Captures the exception thrown when calling `GetUserById`.
     - **Assert:** Verifies that the exception message matches "User not found".
   - **Assertions:**
     ```csharp
     Assert.Equal("User not found", exception.Message);
     ```

#### 2. TransactionsHistoryUT

**Objective:** Ensure the reliability of operations related to transaction history, including retrieval and filtering of transactions.

**Test File:** `TransactionsHistoryUT.cs`

##### Mock Configuration

- **Mocked `ITransactionHistoryRepository`:**
  - **`GetTransactionsByUserId(1)`** returns a predefined list of transactions.
  - **`GetTransactionbyId(2)`** returns a specific transaction.
  - **`GetTransactionsByUserId(5)`** returns an empty list to simulate a non-existent user.
  - **`GetTransactionsByPeriodOfTime(1, startDate, endDate)`** returns transactions within a given period.
  - **`GetLastXTransactions(1, 1)`** returns the most recent transactions limited by the specified number.

##### Test Methods

1. **`GetTransactionsHistory_ValidUser_ShouldReturnAllTransactions`**
   - **Purpose:** Verify that all transactions of a valid user are returned.
   - **Steps:**
     - **Act:** Calls `GetTransactionsHistory` with `userId = 1`.
     - **Assert:** Checks that the result is not empty and contains the expected number of transactions.
   - **Assertions:**
     ```csharp
     Assert.NotEmpty(result);
     Assert.Equal(2, result.Count());
     ```

2. **`GetTransactionsHistory_InvalidUser_ShouldThrowException`**
   - **Purpose:** Ensure that an exception is thrown for an invalid user.
   - **Steps:**
     - **Act & Assert:** Verifies that calling with `userId = 5` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _transactionService.GetTransactionsHistory(5));
     ```

3. **`GetFilteredTransaction_ValidDateRange_ShouldReturnTransactions`**
   - **Purpose:** Verify that transactions are correctly filtered by a valid date range.
   - **Steps:**
     - **Arrange:** Defines a date range.
     - **Act:** Calls `GetFilteredTransaction` with the defined dates.
     - **Assert:** Checks that the number of returned transactions matches expectations.
   - **Assertions:**
     ```csharp
     Assert.Equal(2, result.Count());
     ```

4. **`GetFilteredTransaction_InvalidDateRange_ShouldThrowException`**
   - **Purpose:** Ensure that an exception is thrown for an invalid date range (start date after end date).
   - **Steps:**
     - **Arrange:** Defines an invalid date range.
     - **Act & Assert:** Verifies that calling `GetFilteredTransaction` throws an `ArgumentException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<ArgumentException>(() => _transactionService.GetFilteredTransaction(1, startDate, endDate));
     ```

5. **`GetFilteredTransaction_NoTransactionsInDateRange_ShouldReturnEmptyList`**
   - **Purpose:** Verify that no transactions are returned when none exist within the specified date range.
   - **Steps:**
     - **Arrange:** Defines a date range with no transactions.
     - **Act:** Calls `GetFilteredTransaction` with the defined dates.
     - **Assert:** Checks that the result is an empty list.
   - **Assertions:**
     ```csharp
     Assert.Empty(result);
     ```

6. **`GetLatestTransaction_ValidNumber_ShouldReturnSpecifiedTransactions`**
   - **Purpose:** Verify that the specified number of the most recent transactions is returned.
   - **Steps:**
     - **Act:** Calls `GetLatestTransaction` with `limit = 1`.
     - **Assert:** Checks that only the most recent transaction is returned.
   - **Assertions:**
     ```csharp
     Assert.Single(result);
     Assert.Equal(new DateTime(2024, 10, 02), result.First().Date);
     ```

7. **`GetLatestTransaction_NumberExceedsTotal_ShouldReturnAllTransactions`**
   - **Purpose:** Ensure that all transactions are returned when the specified number exceeds the total available transactions.
   - **Steps:**
     - **Act:** Calls `GetLatestTransaction` with `limit = 10`.
     - **Assert:** Checks that all transactions are returned.
   - **Assertions:**
     ```csharp
     Assert.Equal(2, result.Count());
     ```

8. **`GetLatestTransaction_NegativeNumber_ShouldReturnEmptyList`**
   - **Purpose:** Verify that an empty list is returned when a negative number is specified.
   - **Steps:**
     - **Act:** Calls `GetLatestTransaction` with `limit = -1`.
     - **Assert:** Checks that the result is an empty list.
   - **Assertions:**
     ```csharp
     Assert.Empty(result);
     ```

9. **`GetUserID_NegativeNumber_ShouldReturnError`**
   - **Purpose:** Ensure that an exception is thrown when a negative user ID is provided.
   - **Steps:**
     - **Act & Assert:** Verifies that calling with `userId = -1` throws an `ArgumentException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<ArgumentException>(() => _transactionService.GetTransactionsHistory(userid));
     ```

10. **`GetTransactionId_NegativeNumber_ShouldReturnError`**
    - **Purpose:** Verify that an exception is thrown when a negative transaction ID is provided.
    - **Steps:**
      - **Act & Assert:** Verifies that calling with `transactionId = -1` throws an `ArgumentException`.
    - **Assertion:**
      ```csharp
      Assert.Throws<ArgumentException>(() => _transactionService.getTransactionbyId(transactionId));
      ```

11. **`GetTransactionId_valideNumber_ShouldReturnTheTransaction`**
    - **Purpose:** Verify that a valid transaction is correctly returned.
    - **Steps:**
      - **Act:** Calls `getTransactionbyId` with `transactionId = 2`.
      - **Assert:** Checks that the returned transaction is not null and the ID matches.
    - **Assertions:**
      ```csharp
      Assert.NotNull(result);
      Assert.Equal(2, result.Id);
      ```

#### 3. MealPlanManagementUt

**Objective:** Test the management of meal plan subscriptions and the creation of new plans, ensuring that user credits are properly handled and validations are enforced.

**Test File:** `MealPlanManagementUt.cs`

##### Mock Configuration

- **Mocked `IMealPlanRepository`:**
  - **`AddMealPlan`** is set up to verify when a new plan is added.
  - **`GetMealPlanById(1)`** returns an active meal plan.
  - **`GetMealPlanById(2)`** returns an inactive meal plan.

- **Mocked `IUserRepository`:**
  - **`GetUserById(1)`** returns a user with sufficient credits.
  - **`GetUserById(2)`** returns a user with insufficient credits.
  - **`GetUserById(3)`** returns a user already subscribed to a plan.

##### Test Methods

1. **`SubscribeToPlan_ShouldDecreaseUserCredits`**
   - **Purpose:** Verify that the user's credits are correctly deducted when subscribing to a plan.
   - **Steps:**
     - **Arrange:** Retrieves the user with `Id = 1` and the active plan with `Id = 1`.
     - **Act:** Calls `SubscribeToPlan` with the corresponding IDs.
     - **Assert:** Checks that the user's credits have been appropriately reduced.
   - **Assertions:**
     ```csharp
     Assert.Equal(15, user.Credits);
     ```

2. **`SubscribeToPlan_ShouldNotAllowIfNotEnoughCredits`**
   - **Purpose:** Ensure that subscription is denied if the user lacks sufficient credits.
   - **Steps:**
     - **Arrange:** Retrieves the user with `Id = 2` (insufficient credits) and the active plan with `Id = 1`.
     - **Act & Assert:** Verifies that calling `SubscribeToPlan` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
     ```

3. **`SubscribeToPlan_ShouldNotAllowIfUserAlreadySubscribed`**
   - **Purpose:** Verify that a user already subscribed to a plan cannot subscribe to another plan.
   - **Steps:**
     - **Arrange:** Retrieves the user with `Id = 3` (already subscribed) and the active plan with `Id = 1`.
     - **Act & Assert:** Verifies that calling `SubscribeToPlan` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
     ```

4. **`SubscribeToPlan_ShouldNotAllowIfMealPlanDoesNotExist`**
   - **Purpose:** Ensure that subscription is denied if the meal plan does not exist.
   - **Steps:**
     - **Arrange:** Sets an invalid `mealPlanId` (`-1`) and retrieves the user with `Id = 1`.
     - **Act & Assert:** Verifies that calling `SubscribeToPlan` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlanId));
     ```

5. **`SubscribeToPlan_ShouldNotAllowIfUserDoesNotExist`**
   - **Purpose:** Verify that subscription is denied if the user does not exist.
   - **Steps:**
     - **Arrange:** Retrieves the active plan with `Id = 1` and sets an invalid `userId` (`-1`).
     - **Act & Assert:** Verifies that calling `SubscribeToPlan` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(userId, mealPlan.Id));
     ```

6. **`SubscribeToPlan_ShouldNotAllowIfMealPlanIsNotActive`**
   - **Purpose:** Ensure that subscription is denied if the meal plan is not active.
   - **Steps:**
     - **Arrange:** Retrieves the inactive plan with `Id = 2` and the user with `Id = 1`.
     - **Act & Assert:** Verifies that calling `SubscribeToPlan` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() => _mealPlanService.SubscribeToPlan(user.Id, mealPlan.Id));
     ```

7. **`CreateMealPlan_ShouldAddMealPlanToRepository`**
   - **Purpose:** Verify that a new meal plan is correctly added to the repository.
   - **Steps:**
     - **Arrange:** Creates a new valid `MealPlan`.
     - **Act:** Calls `CreateMealPlan` with the new plan.
     - **Assert:** Verifies that `AddMealPlan` in the repository was called once with the correct parameters.
   - **Assertions:**
     ```csharp
     _mockMealPlanRepo.Verify(
         repo => repo.AddMealPlan(It.Is<MealPlan>(mp => mp.Id == newMealPlan.Id && mp.Name == newMealPlan.Name)),
         Times.Once);
     ```

8. **`CreateMealPlan_ShouldNotAllowEmptyName`**
   - **Purpose:** Ensure that a meal plan cannot be created with an empty name.
   - **Steps:**
     - **Arrange:** Creates a `MealPlan` with an empty name.
     - **Act & Assert:** Verifies that calling `CreateMealPlan` throws an `ArgumentException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
     ```

9. **`CreateMealPlan_ShouldNotAllowStartDateAfterEndDate`**
   - **Purpose:** Verify that a meal plan cannot be created if the start date is after the end date.
   - **Steps:**
     - **Arrange:** Creates a `MealPlan` with a start date after the end date.
     - **Act & Assert:** Verifies that calling `CreateMealPlan` throws an `ArgumentException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
     ```

10. **`CreateMealPlan_ShouldNotAllowPriceLessOrEqualToZero`**
    - **Purpose:** Ensure that a meal plan cannot be created with a price of zero or negative.
    - **Steps:**
      - **Arrange:** Creates a `MealPlan` with a price equal to zero.
      - **Act & Assert:** Verifies that calling `CreateMealPlan` throws an `ArgumentException`.
    - **Assertion:**
      ```csharp
      Assert.Throws<ArgumentException>(() => _mealPlanService.CreateMealPlan(newMealPlan));
      ```

11. **`GetSubscribedUsers_ShouldNotAllowIfMealPlanDoesNotExist`**
    - **Purpose:** Verify that an exception is thrown if the requested meal plan does not exist when retrieving subscribed users.
    - **Steps:**
      - **Arrange:** Sets an invalid `mealPlanId` (`-1`).
      - **Act & Assert:** Verifies that calling `GetSubscribedUsers` throws an `InvalidOperationException`.
    - **Assertion:**
      ```csharp
      Assert.Throws<InvalidOperationException>(() => _mealPlanService.GetSubscribedUsers(mealPlanId));
      ```

12. **`GetSubscribedUsers_ShouldReturnEmptyListIfNoUsersSubscribed`**
    - **Purpose:** Verify that an empty list is returned if no users are subscribed to a specific meal plan.
    - **Steps:**
      - **Arrange:** Retrieves the inactive plan with `Id = 2`.
      - **Act:** Calls `GetSubscribedUsers` with the plan's ID.
      - **Assert:** Checks that the result is an empty list.
    - **Assertions:**
      ```csharp
      Assert.Empty(users);
      ```

13. **`GetMealPlanPrice_ShouldReturnPrice_WhenValid`**
    - **Purpose:** Verify that the price of a meal plan is correctly returned when valid.
    - **Steps:**
      - **Arrange:** Retrieves a valid meal plan with `Id = 1`.
      - **Act:** Calls `GetMealPlanPrice` with the plan's ID.
      - **Assert:** Checks that the returned price matches the plan's price.
    - **Assertions:**
      ```csharp
      Assert.Equal(mealPlan.Price, price);
      ```

14. **`GetMealPlanPrice_ShouldNotAllowIfMealPlanDoesNotExist`**
    - **Purpose:** Ensure that an exception is thrown if the requested meal plan does not exist when retrieving the price.
    - **Steps:**
      - **Arrange:** Sets an invalid `mealPlanId` (`-1`).
      - **Act & Assert:** Verifies that calling `GetMealPlanPrice` throws an `InvalidOperationException`.
    - **Assertion:**
      ```csharp
      Assert.Throws<InvalidOperationException>(() => _mealPlanService.GetMealPlanPrice(mealPlanId));
      ```

#### 4. MealPaymentProcessingUt

**Objective:** Test the processing of meal plan payments, verifying various conditions such as user validity, credit availability, and transaction recording.

**Test File:** `MealPaymentProcessingUt.cs`

##### Mock Configuration

- **Mocked `IMealPlanRepository`:**
  - **`GetMealPlanPrice(1)`** returns `50.0m`.
  - **`GetMealPlanPrice(2)`** returns `100.0m`.
  - **`GetMealPlanPrice` for invalid IDs** throws an `InvalidOperationException` with the message "Meal plan not found".

- **Mocked `IUserRepository`:**
  - **`GetUserById(1)`** returns a user with `Credits = 100.0m`.
  - **`GetUserById(2)`** returns a user with `Credits = 50.0m`.
  - **`GetUserById(3)`** returns a user with `Credits = 0.0m`.
  - **`GetUserById` for invalid IDs** throws an `InvalidOperationException` with the message "User not found".

- **Transactions:** A list of transactions is used to simulate transaction recording.

##### Test Methods

1. **`ProcessMealPayment_ShouldThrowException_WhenUserIsInvalid`**
   - **Purpose:** Verify that an exception is thrown when an invalid user attempts to make a payment.
   - **Steps:**
     - **Arrange:** Sets `userId = -1` and `mealId = 1`.
     - **Act & Assert:** Verifies that calling `ProcessMealPayment` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() =>
         _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
     ```

2. **`ProcessMealPayment_ShouldThrowExceptionWithCorrectMessage_WhenUserPlanNotFound`**
   - **Purpose:** Verify that the exception message is correct when the user is not found.
   - **Steps:**
     - **Arrange:** Sets `userId = 4` (invalid user) and `mealId = 1`.
     - **Act:** Captures the exception thrown when calling `ProcessMealPayment`.
     - **Assert:** Checks that the exception message is "User not found".
   - **Assertions:**
     ```csharp
     Assert.Equal("User not found", exception.Message);
     ```

3. **`ProcessMealPayment_ShouldThrowException_WhenMealPlanNotFound`**
   - **Purpose:** Ensure that an exception is thrown when an invalid meal plan is used for payment.
   - **Steps:**
     - **Arrange:** Sets `userId = 1` and `mealId = -1`.
     - **Act & Assert:** Verifies that calling `ProcessMealPayment` throws an `InvalidOperationException`.
   - **Assertion:**
     ```csharp
     Assert.Throws<InvalidOperationException>(() =>
         _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
     ```

4. **`ProcessMealPayment_ShouldThrowExceptionWithCorrectMessage_WhenMealPlanNotFound`**
   - **Purpose:** Verify that the exception message is correct when the meal plan is not found.
   - **Steps:**
     - **Arrange:** Sets `userId = 1` (valid user) and `mealId = 4` (non-existent plan).
     - **Act:** Captures the exception thrown when calling `ProcessMealPayment`.
     - **Assert:** Checks that the exception message is "Meal plan not found".
   - **Assertions:**
     ```csharp
     Assert.Equal("Meal plan not found", exception.Message);
     ```

5. **`ProcessMealPayment_ShouldReturnFalse_WhenInsufficientCredits`**
   - **Purpose:** Verify that the payment fails when the user does not have sufficient credits.
   - **Steps:**
     - **Arrange:** Sets `userId = 2` (insufficient credits) and `mealId = 2`.
     - **Act:** Calls `ProcessMealPayment`.
     - **Assert:** Checks that the result is `false`.
   - **Assertions:**
     ```csharp
     Assert.False(result);
     ```

6. **`ProcessMealPayment_ShouldReturnTrue_WhenValid`**
   - **Purpose:** Ensure that a valid payment succeeds.
   - **Steps:**
     - **Arrange:** Sets `userId = 1` (sufficient credits) and `mealId = 1`.
     - **Act:** Calls `ProcessMealPayment`.
     - **Assert:** Checks that the result is `true`.
   - **Assertions:**
     ```csharp
     Assert.True(result);
     ```

7. **`AddUserMealTransactions_ShouldAddTransaction_WhenValid`**
   - **Purpose:** Verify that a transaction is correctly added when a payment is valid.
   - **Steps:**
     - **Arrange:** Sets `userId = 1` and `mealId = 1`.
     - **Act:** Calls `AddUserMealTransactions`.
     - **Assert:** Checks that the transactions list contains a single entry with the correct amount.
   - **Assertions:**
     ```csharp
     Assert.Single(_transactions);
     Assert.Equal(50.0m, _transactions.First().Amount);
     ```

8. **`Transactions_ShouldRemainEmpty_WhenNoTransactionAdded`**
   - **Purpose:** Ensure that the transactions list remains empty if no transaction is added.
   - **Steps:**
     - **Assert:** Checks that the transactions list is empty.
   - **Assertions:**
     ```csharp
     Assert.Empty(_transactions);
     ```

9. **`Transactions_ShouldContainMultipleEntries_WhenMultipleTransactionsAdded`**
   - **Purpose:** Verify that multiple transactions are correctly recorded.
   - **Steps:**
     - **Arrange:** Calls `AddUserMealTransactions` twice with different users and plans.
     - **Assert:** Checks that the transactions list contains two entries.
   - **Assertions:**
     ```csharp
     Assert.Equal(2, _transactions.Count);
     ```

10. **`ProcessMealPayment_ShouldDeductCredits_WhenPaymentIsSuccessful`**
    - **Purpose:** Verify that the user's credits are correctly deducted after a successful payment.
    - **Steps:**
      - **Arrange:** Sets `userId = 1` and `mealId = 1`.
      - **Act:** Calls `ProcessMealPayment`.
      - **Assert:** Checks that the user's credits have been reduced by `50.0m`.
    - **Assertions:**
      ```csharp
      Assert.Equal(50.0m, user.Credits); // 100 - 50
      ```

11. **`ProcessMealPayment_ShouldThrowException_WhenMealPlanPriceIsZero`**
    - **Purpose:** Ensure that an exception is thrown if the meal plan price is zero.
    - **Steps:**
      - **Arrange:** Sets `userId = 1` and `mealId = 3`, and configures the plan price to `0m`.
      - **Act & Assert:** Verifies that calling `ProcessMealPayment` throws an `InvalidOperationException`.
    - **Assertion:**
      ```csharp
      Assert.Throws<InvalidOperationException>(() =>
          _paymentService.ProcessMealPayment(userId, 50.0m, mealId));
      ```

12. **`ProcessMealPayment_ShouldNotDeductCredits_WhenPaymentFails`**
    - **Purpose:** Verify that the user's credits are not deducted in case of payment failure.
    - **Steps:**
      - **Arrange:** Sets `userId = 2` (insufficient credits) and `mealId = 2`.
      - **Act:** Calls `ProcessMealPayment`.
      - **Assert:** Checks that the user's credits remain unchanged.
    - **Assertions:**
      ```csharp
      Assert.Equal(50.0m, user.Credits); // 50 - 100
      ```

13. **`ProcessMealPayment_ShouldNotAddTransaction_WhenPaymentFails`**
    - **Purpose:** Ensure that no transaction is recorded when the payment fails.
    - **Steps:**
      - **Arrange:** Sets `userId = 2` (insufficient credits) and `mealId = 2`.
      - **Act:** Calls `ProcessMealPayment`.
      - **Assert:** Checks that the transactions list remains empty.
    - **Assertions:**
      ```csharp
      Assert.Empty(_transactions);
      ```

## Code Coverage Results
![image](https://github.com/user-attachments/assets/33dc9630-55c0-4bfa-86c1-833864b2c844)

