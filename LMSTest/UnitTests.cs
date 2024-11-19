using Microsoft.VisualStudio.TestPlatform.TestHost;
using Blazor_Lab_Starter_Code;
using Blazor_Lab_Starter_Code.Models;
using System.Collections.Immutable;


/* 
https://www.codeproject.com/Articles/17652/How-to-Test-Console-Applications - 
Used to learn about TextWriter, StringWriter, and StringBuilder use cases for testing console applications 
https://learn.microsoft.com/en-us/dotnet/api/system.io.stringwriter?view=net-9.0 - StringWriter Class

Student: Eli Amador, Submission Date: 11-19-2024, Instructor: Dillon Buchanan, CSCI 2910-001 
*/
namespace LMSTest
{
    [TestClass]
    public class LMSUnitTests
    {
        [TestMethod]
        public void AddBookSuccess()
        {
            //Arrange 
            Blazor_Lab_Starter_Code.Program.books = new List<Book>();

            string title = "New Title";
            string author = "Latest Author";
            string isbn = "324-3-14-43432-0";

            //Act
            Blazor_Lab_Starter_Code.Program.AddBook(title, author, isbn);

            //Assert 
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.books.Count); 
            
        }

        [TestMethod]
        //I expect that the book will get added to the books list regardless of field values
        public void AddBook_AddBookWithEmptyField()
        {
            //Arrange 
            Blazor_Lab_Starter_Code.Program.books = new List<Book>();

            string title = "Test title";
            string author = "";
            string isbn = "979-3-15-43432-0";

            //Act 
            Blazor_Lab_Starter_Code.Program.AddBook(title, author, isbn);

            //Assert - The book will be added regardless of empty field
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.books.Count);
        }

        [TestMethod]
        public void AddUserSucess()
        {
            //Arrange 
            Blazor_Lab_Starter_Code.Program.users = new List<User>();

            string name = "Name of User";
            string email = "user@email.com";

            //Act 
            Blazor_Lab_Starter_Code.Program.AddUser(name, email);

            //Assert
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.users.Count);
        }

        [TestMethod]  
        public void AddUser_TestEmptyUserValues()
        {
            Blazor_Lab_Starter_Code.Program.users = new List<User>();

            string name = "Randy Roo";
            string email = "";

            Blazor_Lab_Starter_Code.Program.AddUser(name, email);

            //From the way this method is set up, I expect a majority of entries to be accepted
            //Even though one of the values are empty, it will still be added to the user count
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.users.Count);
            
        }

        [TestMethod]
        public void DeleteUserSuccess()
        {
           // Arrange
           Blazor_Lab_Starter_Code.Program.users = new List<User> //Modify static list from main program
           {
              new User { Id = 1, Name = "Jane Doe", Email = "newemail@test.com" }
           };

           var input = new StringReader("1\n"); //Simulate a user entering 1 to delete the user at ID 1
           Console.SetIn(input);

           // Act
           Blazor_Lab_Starter_Code.Program.DeleteUser();

           //Assert -- ensure that there are 0 users in the static list
           Assert.AreEqual(0, Blazor_Lab_Starter_Code.Program.users.Count);
        }

        [TestMethod]
        public void DeleteUser_InvalidUser()
        {
            //Arrange
            Blazor_Lab_Starter_Code.Program.users = new List<User>
            {
               new User {Id = 1, Name = "Jane Dorothy Roe", Email= "newFakeMail@gmail.com"}
            };

            var input = new StringReader("2\n");
            Console.SetIn(input); 

            //Act
            Blazor_Lab_Starter_Code.Program.DeleteUser();

            //Assert -- There will still be one user in our base of users
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.users.Count); 

        }

        [TestMethod] 
        public void DeleteBookSuccess()
        {
            Blazor_Lab_Starter_Code.Program.books = new List<Book>
            {
                new Book {Id = 1, Author = "Lois Lowry", Title = "The Giver", ISBN = "938-09-34342-09-0"}
            };

            var input = new StringReader("1\n"); 
            Console.SetIn(input);

            //Act
            Blazor_Lab_Starter_Code.Program.DeleteBook(); 

            //Assert -- There will still be one user in our base of users 
            Assert.AreEqual(0, Blazor_Lab_Starter_Code.Program.books.Count);
        }

        [TestMethod]
        public void DeleteBook_InvalidBook()
        {
            Blazor_Lab_Starter_Code.Program.books = new List<Book>
            {
                new Book {Id = 1, Author = "Angela Brewer", Title = "", ISBN = "938-09-39803-4"}
            };

            var input = new StringReader("3\n"); 
            Console.SetIn(input);

            Blazor_Lab_Starter_Code.Program.DeleteBook();

            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.books.Count);
        }

        [TestMethod]
        public void TestBorrowBooksSuccess()
        {
            //Arrange - populate users and books with test dummies
            Blazor_Lab_Starter_Code.Program.users = new List<User> 
            {
                new User { Id = 1, Name = "Riley Roo", Email = "rocksR4winners@gmail.com"},
                new User { Id = 2, Name = "John Doe", Email = "winnerchickendinner@gmail.com" }
            };

            Blazor_Lab_Starter_Code.Program.books = new List<Book>
            {
                new Book {Id = 1, Author = "Test Author", Title = "Test Title", ISBN = "Test ISBN (I know this format is technically incorrect...)"},
                new Book {Id = 2, Author = "Test Author 2", Title = "Test Title 2", ISBN = "Test ISBN 2" }

            };

            Blazor_Lab_Starter_Code.Program.borrowedBooks = new Dictionary<User, List<Book>>(); //Create dictionary to store users and their borrowed books
            Blazor_Lab_Starter_Code.Program.borrowedBooks[Blazor_Lab_Starter_Code.Program.users[0]] = new List<Book>(); //Add the user to the borrowedBooks dictionary

            var userInput = new StringReader("1\n1\n"); //Response to Book ID & User ID
            Console.SetIn(userInput); 

            //Act 
            Blazor_Lab_Starter_Code.Program.BorrowBook();

            //Assert 
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.borrowedBooks[Blazor_Lab_Starter_Code.Program.users[0]].Count); //There should be ONE book in user 1's collection
            Assert.AreEqual(1, Blazor_Lab_Starter_Code.Program.books.Count); //there should only be one book in the books list at this point

        }

        [TestMethod]
        public void TestBorrowBooksFail_ImproperInput()
        {
            //Arrange - populate users and books with test dummies
            Blazor_Lab_Starter_Code.Program.users = new List<User>
            {
                new User { Id = 1, Name = "Riley Roo", Email = "rocksR4winners@gmail.com"},
                new User { Id = 2, Name = "John Doe", Email = "winnerchickendinner@gmail.com" }
            };

            Blazor_Lab_Starter_Code.Program.books = new List<Book>
            {
                new Book {Id = 1, Author = "Test Author", Title = "Test Title", ISBN = "Test ISBN (I know this format is technically incorrect...)"},
                new Book {Id = 2, Author = "Test Author 2", Title = "Test Title 2", ISBN = "Test ISBN 2" }

            };

            var userSelection = new StringReader("200\n"); //Invalid book entry
            Console.SetIn(userSelection);

            //Act 
            Blazor_Lab_Starter_Code.Program.BorrowBook();

            //Assert 
            Assert.AreEqual(2, Blazor_Lab_Starter_Code.Program.books.Count); //There will still be two books in the "books" list

        }

    }
}