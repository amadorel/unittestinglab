using Blazor_Lab_Starter_Code.Models;
using System.Reflection;

namespace Blazor_Lab_Starter_Code
{
    public class Program {

		public static List<Book> books = new List<Book>();
		public static List<User> users = new List<User>();
		public static Dictionary<User, List<Book>> borrowedBooks = new Dictionary<User, List<Book>>();

		static void Main() {
			ReadBooks();
			ReadUsers();

			string option;

			do {
				Console.WriteLine("Library Management System");
				Console.WriteLine("1. Manage Books");
				Console.WriteLine("2. Manage Users");
				Console.WriteLine("3. Borrow Book");
				Console.WriteLine("4. Return Book");
				Console.WriteLine("5. List Borrowed Books");
				Console.WriteLine("6. Exit");

				Console.Write("Choose an option: ");
				option = Console.ReadLine();

				switch (option) {
					case "1":
						ManageBooks();
						break;
					case "2":
						ManageUsers();
						break;
					case "3":
						BorrowBook();
						break;
					case "4":
						ReturnBook();
						break;
					case "5":
						ListBorrowedBooks();
						break;
				}
			} while (option != "6");
		}

		public static void ReadBooks() {
            try {
                foreach (var line in File.ReadLines("./Data/Books.csv")) {
                    var fields = line.Split(',');

                    if (fields.Length >= 4) 
                    {
                        var book = new Book {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        };

                        books.Add(book);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

		public static void ReadUsers() {
            try {
                foreach (var line in File.ReadLines("./Data/Users.csv")) {
                    var fields = line.Split(',');

                    if (fields.Length >= 3) // Ensure there are enough fields
                    {
                        var user = new User {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
							Email = fields[2].Trim()
                        };

                        users.Add(user);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

		public static void ManageBooks()
		{

			string option = "";
			string title = "";
			string author = "";
			string isbn = ""; 

			do {
				Console.WriteLine("\nManage Books");
				Console.WriteLine("1. Add Book");
				Console.WriteLine("2. Edit Book");
				Console.WriteLine("3. Delete Book");
				Console.WriteLine("4. List Books");
				Console.WriteLine("5. Back");
				Console.Write("Choose an option: ");
				option = Console.ReadLine();

				switch (option) {
					case "1":
						AddBook(title, author, isbn);
						break;
					case "2":
						EditBook();
						break;
					case "3":
						DeleteBook();
						break;
					case "4":
						ListBooks();
						break;
				}
			} while (option != "5");
		}

		//Refactored for testing - removing console interaction, testing assumed input purely
		public static void AddBook(string title, string author, string isbn)
		{
			/*Console.Write("\nEnter Book Title: ");
			title = Console.ReadLine();

			Console.Write("Enter Author: ");
			author = Console.ReadLine();

			Console.Write("Enter ISBN: ");
			isbn = Console.ReadLine();*/

			int id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
			books.Add(new Book { Id = id, Title = title, Author = author, ISBN = isbn });
			Console.WriteLine("Book added successfully!\n");
		}

		public static void EditBook() {

			ListBooks();
			Console.Write("\nEnter Book ID to Edit: ");

			if (int.TryParse(Console.ReadLine(), out int bookId)) {

				Book book = books.FirstOrDefault(b => b.Id == bookId);

				if (book != null) {
					Console.Write("Enter new Title (leave blank to keep current): ");
					string title = Console.ReadLine();
					if (!string.IsNullOrEmpty(title)) book.Title = title;

					Console.Write("Enter new Author (leave blank to keep current): ");
					string author = Console.ReadLine();
					if (!string.IsNullOrEmpty(author)) book.Author = author;

					Console.Write("Enter new ISBN (leave blank to keep current): ");
					string isbn = Console.ReadLine();
					if (!string.IsNullOrEmpty(isbn)) book.ISBN = isbn;

					Console.WriteLine("Book updated successfully!\n");
				}
				else {
					Console.WriteLine("Book not found!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}

		public static void DeleteBook() {

			ListBooks();

			Console.Write("\nEnter Book ID to Delete: ");

			if (int.TryParse(Console.ReadLine(), out int bookId)) {

				Book book = books.FirstOrDefault(b => b.Id == bookId);

				if (book != null) {
					books.Remove(book);
					Console.WriteLine("Book deleted successfully!\n");
				}
				else {
					Console.WriteLine("Book not found!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}

		public static void ListBooks() {

			Console.WriteLine("\nAvailable Books:");

			var bookGroups = books.GroupBy(b => b.Id).Select(bookGroup => new { Book = bookGroup.First(), Count = bookGroup.Count() });
			
			foreach (var group in bookGroups) {
				Console.WriteLine($"{group.Book.Id}. {group.Book.Title} by {group.Book.Author} (ISBN: {group.Book.ISBN}) - Available Copies: {group.Count}");
			}

			Console.WriteLine();
		}

		public static void ManageUsers() {

			string option = "";
			string name = "";
			string email = "";
			int userId = 0; 


			do {
				Console.WriteLine("\nManage Users");
				Console.WriteLine("1. Add User");
				Console.WriteLine("2. Edit User");
				Console.WriteLine("3. Delete User");
				Console.WriteLine("4. List Users");
				Console.WriteLine("5. Back");

				Console.Write("Choose an option: ");
				option = Console.ReadLine();

				switch (option) {
					case "1":
						AddUser(name, email);
						break;
					case "2":
						EditUser();
						break;
					case "3":
						DeleteUser();
						break;
					case "4":
						ListUsers();
						break;
				}
			} while (option != "5");
		}

		//Refactored in a similar way to add book
		public static void AddUser(string name, string email) {
			
			/*Console.Write("\nEnter User Name: ");
			name = Console.ReadLine();

			Console.Write("Enter Email: ");
			email = Console.ReadLine();*/

			int id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
			users.Add(new User { Id = id, Name = name, Email = email });
			Console.WriteLine("User added successfully!\n");
		}

		public static void EditUser() {

			ListUsers();
			Console.Write("\nEnter User ID to Edit: ");

			if (int.TryParse(Console.ReadLine(), out int userId)) {

				User user = users.FirstOrDefault(u => u.Id == userId);

				if (user != null) {
					Console.Write("Enter new Name (leave blank to keep current): ");
					string name = Console.ReadLine();
					if (!string.IsNullOrEmpty(name)) user.Name = name;

					Console.Write("Enter new Email (leave blank to keep current): ");
					string email = Console.ReadLine();
					if (!string.IsNullOrEmpty(email)) user.Email = email;

					Console.WriteLine("User updated successfully!\n");
				}
				else {
					Console.WriteLine("User not found!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}


		public static void DeleteUser() {

			ListUsers();
			Console.Write("\nEnter User ID to Delete: ");

			if (int.TryParse(Console.ReadLine(), out int userId)) {

				User user = users.FirstOrDefault(u => u.Id == userId);

				if (user != null) {
					users.Remove(user);
					Console.WriteLine("User deleted successfully!\n");
				}
				else {
					Console.WriteLine("User not found!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}

		public static void ListUsers() {

			Console.WriteLine("\nUsers:");

			foreach (var user in users) {
				Console.WriteLine($"{user.Id}. {user.Name} (Email: {user.Email})");
			}

			Console.WriteLine();
		}

		public static void BorrowBook() {

			ListBooks();
			Console.Write("\nEnter Book ID to Borrow: ");

			if (int.TryParse(Console.ReadLine(), out int bookId)) {

				Book book = books.FirstOrDefault(b => b.Id == bookId);

				if (book != null && books.Count(b => b.Id == bookId) > 0) {

					ListUsers();
					Console.Write("\nEnter User ID who is borrowing the book: ");

					if (int.TryParse(Console.ReadLine(), out int userId)) {

						User user = users.FirstOrDefault(u => u.Id == userId);

						if (user != null) {
							if (!borrowedBooks.ContainsKey(user)) {
								borrowedBooks[user] = new List<Book>();
							}
							borrowedBooks[user].Add(book);
							books.Remove(book);
							Console.WriteLine("Book borrowed successfully!\n");
						}
						else {
							Console.WriteLine("User not found!\n");
						}
					}
					else {
						Console.WriteLine("Invalid input!\n");
					}
				}
				else {
					Console.WriteLine("Book not found or no available copies!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}

		public static void ReturnBook() {

			ListBorrowedBooks();
			Console.Write("\nEnter User ID to return a book for: ");

			if (int.TryParse(Console.ReadLine(), out int userId)) {

				User user = users.FirstOrDefault(u => u.Id == userId);

				if (user != null && borrowedBooks.ContainsKey(user) && borrowedBooks[user].Count > 0) {

					Console.WriteLine("Borrowed Books:");

					for (int i = 0; i < borrowedBooks[user].Count; i++) {
						Console.WriteLine($"{i + 1}. {borrowedBooks[user][i].Title} by {borrowedBooks[user][i].Author} (ISBN: {borrowedBooks[user][i].ISBN})");
					}

					Console.Write("\nEnter the number of the book to return: ");

					if (int.TryParse(Console.ReadLine(), out int bookNumber) && bookNumber >= 1 && bookNumber <= borrowedBooks[user].Count) {
						
						Book bookToReturn = borrowedBooks[user][bookNumber - 1];

						borrowedBooks[user].RemoveAt(bookNumber - 1);
						books.Add(bookToReturn);

						Console.WriteLine("Book returned successfully!\n");
					}
					else {
						Console.WriteLine("Invalid input!\n");
					}
				}
				else {
					Console.WriteLine("User not found or no borrowed books!\n");
				}
			}
			else {
				Console.WriteLine("Invalid input!\n");
			}
		}

		public static void ListBorrowedBooks() {

			Console.WriteLine("\nBorrowed Books:");

			foreach (var entry in borrowedBooks) {
				Console.WriteLine($"User: {entry.Key.Name}");

				foreach (var book in entry.Value) {
					Console.WriteLine($"{book.Title} by {book.Author} (ISBN: {book.ISBN})");
				}

				Console.WriteLine();
			}
		}
	}	
}