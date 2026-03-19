namespace Pitpo;

public static class ErdosNum
{
    private class ErdosNumber
    {
        public required string Name { get; init; }
        public int Number { get; init; }
    }


    private static readonly List<ErdosNumber> Erdos = [];
    private static readonly List<string> Authors = []; // строки авторов из статей
    private static readonly Dictionary<string, int> Exist = new Dictionary<string, int>();
    private static readonly List<bool> Processed = [];

    // Разбирает строку с авторами (часть до двоеточия) и возвращает список имён
    private static List<string> ExtractAuthors(string authorLine)
    {
        // Пример: "Erdos, P., Smith, J." -> после разбиения по ',' получаем:
        // ["Erdos", " P.", " Smith", " J."]
        var parts = authorLine.Split(',');
        var result = new List<string>();

        for (var i = 0; i < parts.Length; i += 2)
        {
            var lastName = parts[i].Trim(); // "Erdos"
            var initials = parts[i + 1].Trim(); // "P." (с пробелом в начале, который убирает Trim)
            var fullName = lastName + ", " + initials; // "Erdos, P."
            result.Add(fullName);
        }

        return result;
    }

    // Добавляет всех авторов статьи в множество erdos с числом (number + 1),
    // если они ещё не были добавлены ранее.
    private static void SetErdosNumber(string authorLine, int number)
    {
        var names = ExtractAuthors(authorLine);
        foreach (var name in names.Where(name => !Exist.ContainsKey(name)))
        {
            Erdos.Add(new ErdosNumber { Name = name, Number = number + 1 });
            Exist[name] = Erdos.Count - 1;
        }
    }

    // Вычисляет числа Эрдёша для всех авторов, встречающихся в статьях.
    private static void CalculateErdosNumbers()
    {
        Erdos.Clear();
        Exist.Clear();

        // Начальное значение: сам Эрдёш
        Erdos.Add(new ErdosNumber { Name = "Erdos, P.", Number = 0 });
        Exist["Erdos, P."] = 0;

        Processed.Clear();
        Processed.AddRange(Enumerable.Repeat(false, Authors.Count));

        int begin = 0, end = 0; // границы текущего слоя в массиве erdos

        while (true)
        {
            // Просматриваем все ещё не обработанные статьи
            for (var i = 0; i < Authors.Count; i++)
            {
                if (Processed[i]) continue;

                // Ищем, есть ли в статье автор из текущего слоя
                for (var j = begin; j <= end; j++)
                {
                    if (!Authors[i].Contains(Erdos[j].Name)) continue;
                    // Если есть, то все остальные авторы этой статьи получают число на 1 больше
                    SetErdosNumber(Authors[i], Erdos[j].Number);
                    Processed[i] = true;
                    break;
                }
            }

            // Если за проход не добавилось ни одного нового автора – выходим
            if (end == Erdos.Count - 1)
                break;

            // Иначе переходим к следующему слою
            begin = end + 1;
            end = Erdos.Count - 1;
        }
    }

    public static void Start()
    {
        var scenario = int.Parse(Console.ReadLine()!);

        for (var s = 1; s <= scenario; s++)
        {
            var firstLine = Console.ReadLine()!.Split();
            var p = int.Parse(firstLine[0]);
            var n = int.Parse(firstLine[1]);

            Authors.Clear();
            for (var i = 0; i < p; i++)
            {
                var line = Console.ReadLine();
                var colonPos = line!.IndexOf(':');
                var authorPart = line.Substring(0, colonPos);
                Authors.Add(authorPart);
            }

            CalculateErdosNumbers();

            Console.WriteLine($"Scenario {s}");
            for (var i = 0; i < n; i++)
            {
                var name = Console.ReadLine();
                Console.WriteLine(Exist.TryGetValue(name!, out var index)
                    ? $"{name} {Erdos[index].Number}"
                    : $"{name} infinity");
            }
        }
    }
}