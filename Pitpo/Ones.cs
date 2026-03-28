namespace Pitpo;

public static class Ones
{
    public static void Start()
    {
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            var tokens = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                if (!int.TryParse(token, out var number)) continue;
                var result = MinimalOnes(number);
                Console.WriteLine(result);
            }
        }
    }
    
    static int MinimalOnes(int n)
    {
        
        long start = 1; //остаток от деления текущего числа из единиц на n
        var counter = 1; //количество единиц в текущем числе
        while (start % n != 0)
        {
            // Переход к следующему числу: текущее * 10 + 1.
            // Чтобы избежать переполнения, используем свойства модулярной арифметики:
            // (prev * 10 + 1) % n = ((prev % n) * (10 % n) + 1) % n.
            start = (start % n) * (10 % n) + 1;
            counter++;
        }
        return counter;
    }
}