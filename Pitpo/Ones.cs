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
        
        long start = 1;
        var counter = 1;
        while (start % n != 0)
        {
        
            start = (start % n) * (10 % n) + 1;
            counter++;
        }
        return counter;
    }
}