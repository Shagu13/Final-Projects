using ManageBooks;

namespace BookManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bookManager = new BookManager();
            var uiHandler = new BookUIHandler(bookManager);

            while (true)
            {
                Console.WriteLine("\nBook Management System:");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Show all books");
                Console.WriteLine("3. Search for a book by title");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        uiHandler.AddBook();
                        break;
                    case "2":
                        uiHandler.ShowAllBooks();
                        break;
                    case "3":
                        uiHandler.SearchBook();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }       
}











