using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBooks
{
    public class BookUIHandler
    {
        private readonly BookManager bookManager;

        public BookUIHandler(BookManager bookManager)
        {
            this.bookManager = bookManager;
        }

        public void AddBook()
        {
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter book author: ");
            string author = Console.ReadLine();

            Console.Write("Enter publication year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Invalid year. Please enter a valid positive number.");
                return;
            }

            bookManager.AddBook(title, author, year);
        }

        public void ShowAllBooks()
        {
            bookManager.ShowAllBooks();
        }

        public void SearchBook()
        {
            Console.Write("Enter the title to search: ");
            string title = Console.ReadLine();
            bookManager.SearchBookByTitle(title);
        }
    }
}
