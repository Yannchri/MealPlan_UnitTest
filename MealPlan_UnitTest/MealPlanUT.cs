using Castle.Core.Configuration;
using MealPlan_Business;
using MealPlan_Business.Models;
using MealPlan_Business.Repositories;
using Moq;

namespace MealPlan_UnitTest
{
    public class MealPlanUT
    {

        private List<Transaction> fakeTransactions;

        //G�n�rer une liste qui va �tre r�utilis�e, je suis s�r que y'a un autre moyen mais �a fonctionnait pas
        // j'attends de voir le code des coll�gues
        private List<Transaction> createFakeTransactions()
        {
            fakeTransactions = new List<Transaction>
        {
            new Transaction { Id = 1, Amount = 50.0, Date = new DateOnly(2024, 10, 01), User = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" } },
            new Transaction { Id = 2, Amount = 30.0, Date = new DateOnly(2024, 10, 02), User = new User { Id = 2, Name = "Bob Johnson", Email = "bob@example.com" } },
            new Transaction { Id = 3, Amount = 75.0, Date = new DateOnly(2024, 10, 03), User = new User { Id = 3, Name = "Charlie Brown", Email = "charlie@example.com" } },
            new Transaction { Id = 4, Amount = 100.0, Date = new DateOnly(2024, 10, 04), User = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" } },
            new Transaction { Id = 5, Amount = 40.0, Date = new DateOnly(2024, 10, 05), User = new User { Id = 2, Name = "Bob Johnson", Email = "bob@example.com" } },
            new Transaction { Id = 6, Amount = 60.0, Date = new DateOnly(2024, 10, 06), User = new User { Id = 3, Name = "Charlie Brown", Email = "charlie@example.com" } },
            new Transaction { Id = 7, Amount = 20.0, Date = new DateOnly(2024, 10, 07), User = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" } },
            new Transaction { Id = 8, Amount = 90.0, Date = new DateOnly(2024, 10, 08), User = new User { Id = 2, Name = "Bob Johnson", Email = "bob@example.com" } },
            new Transaction { Id = 9, Amount = 10.0, Date = new DateOnly(2024, 10, 09), User = new User { Id = 3, Name = "Charlie Brown", Email = "charlie@example.com" } },
            new Transaction { Id = 10, Amount = 25.0, Date = new DateOnly(2024, 10, 10), User = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" } }
        };
            return fakeTransactions;
          
        }


        [Fact]
        public void GetAllTheTransactionForUser1_ShouldReturn4Transactions()
        {
            // Arrange : set up data with 10 transactions
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetTransactionsHistory(1); // Utilisateur avec ID = 1

            // Assert = v�rifier le r�sultat
            Assert.Equal(4, result.Count()); 
        }

        [Fact]
        public void GetAllTransactionFromInexistingUser_ShouldReturn0Transaction()
        {
            // Arrange : set up data with 10 transactions
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();
            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);


            // Act = appeler la m�thode sous test
            var result = transactionService.GetTransactionsHistory(5); // Utilisateur avec ID = 5

            // Assert = v�rifier le r�sultat
            Assert.Equal(0, result.Count()); 
        }

        [Fact]

        public void GetAllTransactionFromInexistingUser_ShouldReturnErrorMessage()
        {
            // Arrange : set up data with 10 transactions
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 5
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(5)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);


            // Act = appeler la m�thode sous test
            var result = transactionService.GetTransactionsHistory(5); // Utilisateur avec ID = 5

            var errormessage = transactionService.GetErrorMessage();

            // Assert = v�rifier le r�sultat
            Assert.Equal("User doesn't exist",errormessage); // V�rifie que le message d'erreur est le bon
        }

        [Fact]
        public void GetFilterTransactionForToday_ShouldReturnTransactionOfTheDay()
        {
            // Arrange : set up data with several transactions, including one for today
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            //Charlie Brown user ID 3
            var today = new DateOnly(2024, 10, 09);

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
             mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetFilteredTransaction(3, today, today); // Filtrer pour aujourd'hui

            // Assert = v�rifier le r�sultat
            Assert.Single(result);

        }

        [Fact]
        public void GetFilterTransactionAPeriod_ShouldReturnTransactionOfThePeriod()
        {
            // Arrange : set up data with several transactions, including one for today
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            var firstDate = new DateOnly(2024, 10, 05);
            var secondDate = new DateOnly(2024, 10, 10);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetFilteredTransaction(3, firstDate, secondDate); // Filtrer dans une plage de 2 entr�e sur 3

            // Assert = v�rifier le r�sultat
            Assert.Equal(2,result.Count());

        }

        [Fact]
        public void VerifyTheFirstDateAndLastDate_ShouldReturnAnErrorMessage()
        {
            // Arrange : set up data with several transactions, including one for today
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 3
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(3)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            var firstDate = new DateOnly(2024, 10, 05);
            var secondDate = new DateOnly(2024, 10, 10);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetFilteredTransaction(3, secondDate, firstDate); // Filtrer dans une plage de 2 entr�e sur 3

            // Assert = v�rifier le r�sultat
            Assert.Equal("First date must be older than second date",transactionService.GetErrorMessage());

        }

        [Fact]
        public void GetTheLast2Transactions_ShouldReturn2TransactionsAndTheGoodDate()
        {
            // Arrange : set up data with several transactions, including one for today
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetLatestTransaction(1, 2); //retourne les 2 derni�res transactions sur 4 

            // Assert = v�rifier le r�sultat
            Assert.Equal(2, result.Count());

            var lastTransactions = result.ToList();
            Assert.Equal(new DateOnly(2024, 10, 10), lastTransactions[0].Date); // V�rifie la premi�re date
            Assert.Equal(new DateOnly(2024, 10, 07), lastTransactions[1].Date); // V�rifie la deuxi�me date

        }

        [Fact]
        public void AskMoreThanTheMaxTransaction_ShouldReturnAllTheTransactions()
        {
            // Arrange : set up data with several transactions
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetLatestTransaction(1, 10); //Demande 10 transactions mais il y en 4

            // Assert = v�rifier le r�sultat
            Assert.Equal(4, result.Count());

        }

        [Fact]
        public void GetAllTransactionsWithNegativeNumber_ShouldReturnEmptyList()
        {
            // Arrange : set up data with several transactions
            var mockTransactionRepo = new Mock<ITransactionHistoryService>();
            List<Transaction> fakeTransaction = createFakeTransactions();

            // Configuration du mock pour retourner les transactions fictives pour l'utilisateur avec ID = 1
            mockTransactionRepo.Setup(repo => repo.GetTransactionsHistory(1)).Returns(fakeTransactions);

            // Instanciation du service avec le mock inject�
            var transactionService = new TransactionHistoryService(mockTransactionRepo.Object);

            // Act = appeler la m�thode sous test
            var result = transactionService.GetLatestTransaction(1, -1); //negative number

            // Assert = v�rifier le r�sultat
            Assert.Empty(result);
        }


        [Fact]
        public void Should_Process_Valid_Payment_And_Add_Transaction()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionHistoryService>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Simuler un utilisateur valide
            var user = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" };
            mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);

            var paymentService = new PaymentService(mockTransactionRepository.Object, mockUserRepository.Object);
            double validAmount = 100.0;
            int validUserId = 1;

            // Act
            var result = paymentService.ProcessMealPayment(validUserId, validAmount);

            // Assert
            Assert.True(result); // V�rifie que le paiement est accept�
            mockTransactionRepository.Verify(r => r.AddTransaction(It.IsAny<Transaction>()), Times.Once); // V�rifie que la transaction a �t� ajout�e
        }



        [Fact]
        public void Should_Reject_Invalid_Payment_With_Negative_Amount()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionHistoryService>();
            var mockUserRepository = new Mock<IUserRepository>();

            // Simuler un utilisateur valide
            var user = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" };
            mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);

            var paymentService = new PaymentService(mockTransactionRepository.Object, mockUserRepository.Object);
            double invalidAmount = -50.0; // Montant invalide
            int validUserId = 1;

            // Act
            var result = paymentService.ProcessMealPayment(validUserId, invalidAmount);

            // Assert
            Assert.False(result); // Le paiement devrait �tre rejet�
            Assert.Equal("Invalid amount", paymentService.GetErrorMessage()); // V�rifie le message d'erreur
            mockTransactionRepository.Verify(r => r.AddTransaction(It.IsAny<Transaction>()), Times.Never); // V�rifie que la m�thode AddTransaction n'a jamais �t� appel�e
        }
    


        [Fact]
        public void Should_Return_ErrorMessage_When_Invalid_User()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionHistoryService>();
            var fakeTransactionList = new List<Transaction>(); // Aucun utilisateur valide
            mockTransactionRepository.Setup(repo => repo.GetTransactionsHistory(It.IsAny<int>())).Returns(fakeTransactionList);

            var paymentService = new TransactionHistoryService(mockTransactionRepository.Object);
            int invalidUserId = 25; // Utilisateur invalide
            double validAmount = 100.0;

            // Act
            var result = paymentService.GetTransactionsHistory(invalidUserId);
            var errorMessage = paymentService.GetErrorMessage();

            // Assert
            Assert.Empty(result); // Aucune transaction pour l'utilisateur invalide
            Assert.Equal("User doesn't exist", errorMessage); // Message d'erreur correct
        }


        [Fact]
        public void Should_Add_Transaction_When_Valid_User_And_Amount()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionHistoryService>();
            var fakeTransactionList = createFakeTransactions();
            mockTransactionRepository.Setup(repo => repo.GetTransactionsHistory(It.IsAny<int>())).Returns(fakeTransactionList);

            var paymentService = new TransactionHistoryService(mockTransactionRepository.Object);
            int userId = 1;
            double validAmount = 100.0;

            // Act
            var result = paymentService.GetTransactionsHistory(userId);

            // Assert
            Assert.NotEmpty(result); // Il devrait y avoir des transactions
            Assert.True(result.Count() > 0); // Assurer que des transactions ont bien �t� ajout�es
        }

        [Fact]
        public void Should_Reject_Payment_With_Zero_Amount()
        {
            // Arrange
            var mockTransactionRepository = new Mock<ITransactionHistoryService>();
            var mockUserRepository = new Mock<IUserRepository>();

            var user = new User { Id = 1, Name = "Alice Smith", Email = "alice@example.com" };
            mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);

            var paymentService = new PaymentService(mockTransactionRepository.Object, mockUserRepository.Object);
            double zeroAmount = 0.0;

            // Act
            var result = paymentService.ProcessMealPayment(1, zeroAmount);

            // Assert
            Assert.False(result); // Le paiement devrait �tre rejet�
            Assert.Equal("Invalid amount", paymentService.GetErrorMessage()); // V�rifie le message d'erreur
        }

        [Fact]
        








    }
}