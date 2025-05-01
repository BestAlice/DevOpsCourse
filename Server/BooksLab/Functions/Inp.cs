namespace BooksLab.Functions;

internal class Inp
{
    public static int Input(int min, int max)
    {
        int value;
        bool isValidInput = false;
        while(true)
        {
            string input = Console.ReadLine()!;
            isValidInput = int.TryParse(input, out value) && value >= min && value <= max;
            if (isValidInput)
            {
                break;
            }
            Console.WriteLine("Повторите ввод");
        }
        return value;
    }

    public static void Pause()
    {
        Console.Write("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}
