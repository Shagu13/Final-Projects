using System.Text.Json;

namespace ManageBooks
{
    public class BookManager
    {
        private const string FileName = "Books.json";
        private const string FilePath = $"Your File Path Here {FileName}";

        private readonly List<Book> books;
        private readonly BookValidator bookValidator;

        public BookManager()
        {
            bookValidator = new BookValidator(); 
            books = LoadBooksFromFile(); 
        }

        public void AddBook(string title, string author, int publicationYear)
        {
            var newBook = new Book(title, author, publicationYear);
            var validationResult = bookValidator.Validate(newBook);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return;
            }

            books.Add(newBook);
            SaveBooksToFile();
            Console.WriteLine("Book added and saved successfully!");
        }

        public void ShowAllBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("No books in the list.");
                return;
            }

            Console.WriteLine("\nBooks List:");
            foreach (var book in books)
            {
                Console.WriteLine(book.ToString());
            }
        }

        public void SearchBookByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Search query cannot be empty or whitespace.");
                return;
            }

            var foundBooks = books
                .Where(b => b.Title.StartsWith(title, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundBooks.Count == 0)
            {
                Console.WriteLine("No books found with the given title.");
                return;
            }

            Console.WriteLine("\nSearch Results:");
            foreach (var book in foundBooks)
            {
                Console.WriteLine(book.ToString());
            }
        }

        private List<Book> LoadBooksFromFile()
        {
            if (!File.Exists(FilePath))
                return new List<Book>();

            try
            {
                var jsonData = File.ReadAllText(FilePath);
                var loadedBooks = JsonSerializer.Deserialize<List<Book>>(jsonData) ?? new List<Book>();

                return loadedBooks.Where(book =>
                {
                    if (book == null)
                    {
                        Console.WriteLine("Null book object found in file.");
                        return false;
                    }

                    var validationResult = bookValidator.Validate(book);
                    if (!validationResult.IsValid)
                    {
                        Console.WriteLine("Invalid book found in file:");
                        foreach (var error in validationResult.Errors)
                        {
                            Console.WriteLine($" - {error.ErrorMessage}");
                        }
                        return false;
                    }
                    return true;
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading books from file: {ex.Message}");
                return new List<Book>();
            }
        }

        private void SaveBooksToFile()
        {
            var jsonData = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }
    }
}
