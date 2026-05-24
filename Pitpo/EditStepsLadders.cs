namespace Pitpo;

public static class EditStepsLadders
{
    public static void Start()
    {
        var dp = new Dictionary<string, int>();
        var maxLadder = 0;
        string line;
        while ((line = Console.ReadLine()!) != "stop")
        {
            var words = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                var currentBest = 1; // Минимальная длина – само слово
                var len = word.Length;

                // Вариант 1: предыдущее слово короче на 1
                for (var i = 0; i < len; i++)
                {
                    var prev = string.Concat(word.AsSpan(0, i), word.AsSpan(i + 1));
                    if (string.CompareOrdinal(prev, word) < 0 && dp.TryGetValue(prev, out var value))
                    {
                        currentBest = Math.Max(currentBest, value + 1);
                    }
                }

                // Вариант 2: замена одного символа
                for (var i = 0; i < len; i++)
                {
                    // Для каждой позиции перебираем все возможные символы
                    for (var c = 'a'; c <= 'z'; c++)
                    {
                        if (c == word[i]) continue; // Тот же символ – не меняем
                        var chars = word.ToCharArray();
                        chars[i] = c;
                        var prev = new string(chars);
                        if (string.CompareOrdinal(prev, word) < 0 && dp.TryGetValue(prev, out var value))
                        {
                            currentBest = Math.Max(currentBest, value + 1);
                        }
                    }
                }

                // Вариант 3: предыдущее слово длиннее на 1
                for (var i = 0; i <= len; i++)
                {
                    for (var c = 'a'; c <= 'z'; c++)
                    {
                        // Создаём слово prev = word с вставленным символом c на позицию i
                        var prev = word[..i] + c + word[i..];
                        if (string.CompareOrdinal(prev, word) < 0 && dp.TryGetValue(prev, out var value))
                        {
                            currentBest = Math.Max(currentBest, value + 1);
                        }
                    }
                }
                dp[word] = currentBest;
                if (currentBest > maxLadder)
                    maxLadder = currentBest;
            }
        }
        Console.WriteLine(maxLadder);
    }
}