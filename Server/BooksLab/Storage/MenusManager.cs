namespace BooksLab.Storage
{
    public class MenusManager
    {
        public static void MainMenu(int userId)
        {
            Console.Clear();
            Console.WriteLine($"Добро пожаловать в каталог книг! Ваш id: {userId}");
            Console.WriteLine("Выберите действие, введя номер соответствующего пункта:");
            Console.WriteLine("1. Добавить книгу в каталог");
            Console.WriteLine("2. Найти книгу по названию");
            Console.WriteLine("3. Найти книгу по имени автора");
            Console.WriteLine("4. Найти книгу по ISBN");
            Console.WriteLine("5. Найти книги по ключевым словам");
            Console.WriteLine("6. Выйти\n");
            Console.Write("Ваш выбор: ");
        }
    }
}
