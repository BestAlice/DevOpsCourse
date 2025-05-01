namespace BooksLab.Books;

public class Book
{
    public int Id { get; set; }          // Первичный ключ
    public  string Title { get; set; }        // название
    public  string Author { get; set; }      // автор
    public  List<string> Genres  { get; set; } // жанры
    public  int PublicationYear  { get; set; } // год публикации
    public  string Annotation { get; set; }   // аннотация
    public  string ISBN  { get; set; }         // ISBN
    public  int UserId  { get; set; }          // Идентификатор пользователя
    
    public Book()
    {
        Genres = new List<string>();
    }
    /* Конструктор класса Book
    Принимает в себя название, автора, жанры, дату публикации, аннотации,
    ISBN и идентификатор пользователя */
    public Book(string title, string author, List<string> genres, int publicationYear, string annotation, string isbn, int userId)
    {
        Title = title;
        Author = author;
        Genres = genres;
        PublicationYear = publicationYear;
        Annotation = annotation;
        ISBN = isbn;
        UserId = userId;
    }

    // Перегрузка функции ToString
    public override string ToString()
    {
        return $"{Title}, {Author}, {string.Join(", ", Genres)}, {PublicationYear}, ISBN: {ISBN}";
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var book = (Book)obj;
        return Title == book.Title &&
               Author == book.Author &&
               Genres == book.Genres &&
               PublicationYear == book.PublicationYear &&
               ISBN == book.ISBN;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Author, Genres, PublicationYear, ISBN);
    }
}