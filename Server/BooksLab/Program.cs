using BooksLab.Functions;
using BooksLab.Books;

namespace BooksLab;

class Program
{
    public static void Main(string[] args)
    {
        
        /*try
        {
            Console.WriteLine("Введите ваш идентификатор пользователя: ");
            int userId;

            while (!Validator.ValidateUserId(Console.ReadLine(), out userId))
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите числовой идентификатор.");
            }

            int choice;
            do
            {
                MenusManager.MainMenu(userId);
                choice = Inp.Input(1, 6);

                
                    switch (choice)
                    {
                        case 1:
                            // Добавление книги
                            AddBookAsync(userId);
                            break;

                        case 2:
                            Console.Write("Введите название книги: ");
                            string title = Console.ReadLine()!;
                            IBookSearch titleSearch = new BookSearch();
                            var booksByTitle = titleSearch.SearchAsync(userId, title, (Book book) => book.Title);
                            Display.ShowBooks(booksByTitle);
                            break;

                        case 3:
                            Console.Write("Введите имя автора: ");
                            string author = Console.ReadLine()!;
                            IBookSearch authorSearch = new BookSearch();
                            var booksByAuthor = authorSearch.SearchAsync(userId, author, (Book book) => book.Author);
                            Display.ShowBooks(booksByAuthor);
                            break;

                        case 4:
                            Console.Write("Введите ISBN: ");
                            string isbn = Console.ReadLine()!;
                            IBookSearch isbnSearch = new BookSearch();
                            var booksByIsbn = isbnSearch.SearchAsync(userId, isbn, (Book book) => book.ISBN);
                            Display.ShowBooks(booksByIsbn);
                            break;
                        case 5:
                            Console.Write("Введите ключевые слова (через запятую): ");
                            string keywordQuery = Console.ReadLine()!;
                            IBookSearch keywordSearch = new KeywordSearch();
                            var booksByKeywords = keywordSearch.SearchAsync(userId, keywordQuery, (Book book) => "");
                            Display.ShowBooks(booksByKeywords);
                            break;
                    }

                    if (choice != 6)
                    {
                        Inp.Pause();
                    }
                
               
            } while (choice != 6);
            
            Console.WriteLine("До свидания!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Произошло исключение.");
            Console.WriteLine(e.Message);
        }*/
    }
    /*
    private static async void AddBookAsync(int userId)
    {

        Console.Write("Введите название книги: ");
        string title;
        while (true)
        {
            title = Console.ReadLine()!;
            if (Validator.ValidateBookTitle(title))
                break;

            Console.WriteLine("Некорректное название. Пожалуйста, введите ещё раз:");
        }

        Console.Write("Введите имя автора: ");
        string author;
        while (true)
        {
            author = Console.ReadLine()!;
            if (Validator.ValidateAuthorName(author))
                break;

            Console.WriteLine("Некорректное имя автора. Пожалуйста, введите ещё раз:");
        }

        Console.Write("Введите жанры (через запятую): ");
        List<string> genres;
        while (!Validator.ValidateGenres(Console.ReadLine()!, out genres))
        {
            Console.WriteLine("Некорректные жанры. Пожалуйста, введите ещё раз:");
        }

        Console.Write("Введите дату публикации (ГГГГ): ");
        int publicationYear = Validator.ValidatePublicationYear();

        Console.Write("Введите аннотацию: ");
        string annotation = Console.ReadLine()!;
        while (!Validator.ValidateAnnotation(annotation))
        {
            Console.WriteLine("Некорректная аннотация. Пожалуйста, введите ещё раз:");
            annotation = Console.ReadLine()!;
        }

        Console.Write("Введите ISBN: ");
        string isbn = Console.ReadLine()!;
        while (!Validator.ValidateISBN(isbn))
        {
            Console.WriteLine("Некорректный ISBN. Пожалуйста, введите ещё раз:");
            isbn = Console.ReadLine()!;
        }

        Book book = new(title, author, genres, publicationYear, annotation, isbn, userId);
        await using(BookContext context = new(userId))
        {
            await context.AddBookAsync(book);
        }
    }*/
}
