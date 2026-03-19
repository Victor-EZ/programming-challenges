namespace Pitpo;

public static class LcdDisplay
{
    //private const int MaxLength = 8;

    private static readonly string[,] Result = new [,]
    {
        { " - ", "   ", " - ", " - ", "   ", " - ", " - ", " - ", " - ", " - " },
        { "| |", "  |", "  |", "  |", "| |", "|  ", "|  ", "  |", "| |", "| |" },
        { " - ", "   ", " - ", " - ", " - ", " - ", " - ", "   ", " - ", " - " },
        { "| |", "  |", "|  ", "  |", "  |", "  |", "| |", "  |", "| |", "  |" },
        { " - ", "   ", " - ", " - ", "   ", " - ", " - ", "   ", " - ", " - " }
    };

    public static void Display(int size, int number, int maxLength)
    {
        var numbers = new int[maxLength];
        if (number == 0) numbers[maxLength - 1] = 0;
        else
        {
            for (var i = maxLength - 1; number > 0; i--)
            {
                numbers[i] = number % 10;
                number /= 10;
            }
        }

        for (var row = 1; row <= (2 * size + 3); row++)
        {
            for (var i = 0; i < maxLength; i++)
            {
                if (numbers[i] == -1) continue;
                var line = "";
                if (row == 1) line = Result[0, numbers[i]];
                if (2 <= row && row < (size + 2)) line = Result[1, numbers[i]];
                if (row == (size+2)) line = Result[2, numbers[i]];
                if ((size + 3) <= row && row <= (2*size + 2)) line = Result[3, numbers[i]];
                if (row == (2 * size + 3)) line = Result[4, numbers[i]];
                Console.Write(line[0]);
                for (var j = 0; j < size; j++) Console.Write(line[1]);
                Console.Write(line[2]);
                if (i < (maxLength - 1)) Console.Write(" ");
            }
            Console.Write('\n');
        }
    }
}