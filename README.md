# **MealPlan Project Documentation**

## **1. Introduction**

### **Description**

MealPlan is a meal management system that allows users to:

- Make payments for meal plans using their credits.
- Manage subscriptions to meal plans.
- Record and view transaction history.

### **Objectives**

- Simplify the management of user subscriptions and payments.
- Ensure data integrity and accurate credit management.
- Provide functionalities to analyze transaction history.

---

## **2. Architecture**

### **Models**

1. **User**:

   - Properties: `Id`, `Name`, `Email`, `Credits`, `MealPlanId`.

2. **MealPlan**:

   - Properties: `Id`, `Name`, `Price`, `startDate`, `endDate`.

3. **MealTransaction**:

   - Properties: `Id`, `UserId`, `Amount`, `Date`.

### **Key Components**

1. **Services**

   - **MealPaymentService**:
     - Handles meal-related payments.
     - Methods: `ProcessMealPayment`, `AddUserMealTransactions`.
   - **MealPlanService**:
     - Manages subscriptions and creation of meal plans.
     - Methods: `SubscribeToPlan`, `CreateMealPlan`, `GetSubscribedUsers`, `GetMealPlanPrice`.
   - **TransactionHistoryService**:
     - Provides access to transaction history.
     - Methods: `GetTransactionsHistory`, `GetFilteredTransaction`, `GetLatestTransaction`.
   - **UserService**:
     - Manages user data.
     - Method: `GetUserById`.

2. **Repositories**

   - Interfaces for data persistence.
   - Examples: `IMealPlanRepository`, `ITransactionHistoryRepository`, `IUserRepository`.

---

## **3. Key Features**

### **1. Meal Payments**

- Validates user and meal plan.
- Deducts user credits.
- Records transactions.

**Example Code:**

```csharp
var success = mealPaymentService.ProcessMealPayment(userId: 1, amount: 50.0m, mealId: 1);
if (success)
{
    Console.WriteLine("Payment successful!");
}
```

### **2. Meal Plan Management**

- Create new meal plans.
- Validate subscriptions (sufficient credits, active plan).

**Example Code:**

```csharp
mealPlanService.SubscribeToPlan(userId: 1, mealPlanId: 1);
```

### **3. Transaction History**

- View complete or filtered transaction history.
- Access recent transactions.

**Example Code:**

```csharp
var transactions = transactionHistoryService.GetFilteredTransaction(userId: 1, startDate, endDate);
```

---

## **4. Test Coverage**

### **Unit Tests**

1. **MealPaymentService Tests**:

   - Verify successful and failed payments.
   - Ensure transactions are recorded correctly.

2. **MealPlanService Tests**:

   - Validate subscription rules (active plan, sufficient credits).
   - Ensure valid meal plan creation.

3. **TransactionHistoryService Tests**:

   - Check correct return of transaction history.
   - Handle invalid date ranges.

4. **UserService Tests**:

   - Validate retrieval of existing users.
   - Handle exceptions for non-existent users.

### **Running Tests**

To execute all unit tests, run:

```bash
dotnet test
```

---

## **5. Resources**

- **C# Documentation**: [https://docs.microsoft.com/en-us/dotnet/csharp/](https://docs.microsoft.com/en-us/dotnet/csharp/)
- **Testing Frameworks**:
  - XUnit
  - Moq (for mocking dependencies)

---

## **6. Example Usage Scenarios**

### Scenario: Successful Payment

1. User `John Doe` has 100 credits.
2. Meal plan costs 50 credits.
3. Payment is processed:
   ```csharp
   var result = mealPaymentService.ProcessMealPayment(1, 50.0m, 1);
   Console.WriteLine(result ? "Payment successful" : "Payment failed");
   ```
4. Remaining credits: 50.

### Scenario: Subscription to Meal Plan

1. User subscribes to an active plan with sufficient credits:
   ```csharp
   mealPlanService.SubscribeToPlan(1, 1);
   ```
2. User's credits are deducted, and subscription is confirmed.

---

## **7. Licensing and Credits**

This project is open-source and maintained by the MealPlan development team.

