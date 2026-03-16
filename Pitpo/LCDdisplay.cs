namespace Pitpo;

public class LCDdisplay
{
    //private const int MaxLength = 8;

    private static readonly string[,] Result = new string[5, 10]
    {
        { " - ", "   ", " - ", " - ", "   ", " - ", " - ", " - ", " - ", " - " },
        { "| |", "  |", "  |", "  |", "| |", "|  ", "|  ", "  |", "| |", "| |" },
        { " - ", "   ", " - ", " - ", " - ", " - ", " - ", "   ", " - ", " - " },
        { "| |", "  |", "|  ", "  |", "  |", "  |", "| |", "  |", "| |", "  |" },
        { " - ", "   ", " - ", " - ", "   ", " - ", " - ", "   ", " - ", " - " }
    };

    public static void display(int size, int number, int MaxLength)
    {
        var numbers = new int[MaxLength];
        if (number == 0) numbers[MaxLength - 1] = 0;
        else
        {
            for (var i = MaxLength - 1; number > 0; i--)
            {
                numbers[i] = number % 10;
                number /= 10;
            }
        }

        for (var row = 1; row <= (2 * size + 3); row++)
        {
            for (var i = 0; i < MaxLength; i++)
            {
                if (numbers[i] != -1)
                {
                    var line = "";
                    if (row == 1) line = Result[0, numbers[i]];
                    if (2 <= row && row < (size + 2)) line = Result[1, numbers[i]];
                    if (row == (size+2)) line = Result[2, numbers[i]];
                    if ((size + 3) <= row && row <= (2*size + 2)) line = Result[3, numbers[i]];
                    if (row == (2 * size + 3)) line = Result[4, numbers[i]];
                    Console.Write(line[0]);
                    for (var j = 0; j < size; j++) Console.Write(line[1]);
                    Console.Write(line[2]);
                    if (i < (MaxLength - 1)) Console.Write(" ");
                }
            }
            Console.Write('\n');
        }
    }
}