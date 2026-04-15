namespace Pitpo;

public static class EncryptionII
{
    public static void Start()
    {
        var blocks = int.Parse(Console.ReadLine()!); //Количество блоков
        Console.ReadLine();
        for (var i = 0; i < blocks; i++)
        {
            var encrypted = new List<List<string>>();
            while (Console.ReadLine() is { Length: > 0 } line)
            {
                var words = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                encrypted.Add([..words]);
            }
            
            Decipher(encrypted);
            if (i < blocks - 1) Console.WriteLine(); //Пустая строка между блоками
        }
    }

    private static void Decipher(List<List<string>> encrypted)
    {
        var plainWords = "the quick brown fox jumps over the lazy dog".Split(' '); //Кодовая строка
        foreach (var line in encrypted)
        {
            if (line.Count != plainWords.Length) continue;
            var lengthsOk = !plainWords.Where((t, i) => line[i].Length != t.Length).Any();
            if (!lengthsOk) continue;
            var forward = new char[26];
            var backward = new char[26];
            var mappingOk = true;
            for (var i = 0; i < plainWords.Length; i++)
            {
                var cipherWord = line[i];
                var plainWord = plainWords[i];
                for (var j = 0; j < cipherWord.Length; j++)
                {
                    var cipherChar = cipherWord[j];
                    var plainChar = plainWord[j];
                    var ci = cipherChar - 'a';
                    var pi = plainChar - 'a';
                    if (forward[ci] == '\0' && backward[pi] == '\0')
                    {
                        forward[ci] = plainChar;
                        backward[pi] = cipherChar;
                    }
                    else if (forward[ci] != plainChar || backward[pi] != cipherChar)
                    {
                        mappingOk = false;
                        break;
                    }
                }
                if (!mappingOk) break;
            }
            if (!mappingOk) continue;
            foreach (var lineToPrint in encrypted)
            {
                for (var i = 0; i < lineToPrint.Count; i++)
                {
                    var w = lineToPrint[i];
                    var decoded = new char[w.Length];
                    for (var j = 0; j < w.Length; j++) decoded[j] = forward[w[j] - 'a'];
                    Console.Write(new string(decoded));
                    if (i < lineToPrint.Count - 1) Console.Write(" ");
                }
                Console.WriteLine();
            }
            return;
        }
        Console.WriteLine("No solution."); //Если подходящая строка не найдена
    }
}