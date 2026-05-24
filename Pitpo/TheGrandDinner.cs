namespace Pitpo;

public static class TheGrandDinner
{
    private struct Table
    {
        public int Id;
        public int Capacity;
    }

    public static void Start()
    {
        while (Console.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var mn = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries)
                           .Select(int.Parse).ToArray();
            if (mn.Length < 2) continue;
            var M = mn[0]; // количество команд
            var N = mn[1]; // количество столов
            if (M == 0 && N == 0) break;
            var teams = new List<int>(M);
            // Возможно, размеры команд могут быть на нескольких строках
            while (teams.Count < M)
            {
                var nums = Console.ReadLine()!.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
                foreach (var num in nums)
                {
                    if (teams.Count < M)
                        teams.Add(int.Parse(num));
                }
            }

            // Читаем вместимости столов
            var tables = new List<Table>(N);
            while (tables.Count < N)
            {
                var caps = Console.ReadLine()!.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
                foreach (var cap in caps)
                {
                    if (tables.Count < N)
                    {
                        tables.Add(new Table { Id = tables.Count + 1, Capacity = int.Parse(cap) });
                    }
                }
            }
            
            var result = new List<List<int>>(M);
            var possible = true;
            for (var i = 0; i < M; i++)
            {
                var members = teams[i];
                // Если членов команды больше, чем всего столов, рассадка невозможна
                if (members > N)
                {
                    possible = false;
                    break;
                }

                // Сортируем столы по убыванию вместимости (перед каждой командой)
                tables.Sort((a, b) => b.Capacity.CompareTo(a.Capacity));

                var assignedTables = new List<int>(members);
                // Сажаем членов текущей команды за столы с наибольшей вместимостью
                for (var j = 0; j < members; j++)
                {
                    if (tables[j].Capacity > 0)
                    {
                        assignedTables.Add(tables[j].Id);
                        // Уменьшаем вместимость выбранного стола
                        tables[j] = new Table { Id = tables[j].Id, Capacity = tables[j].Capacity - 1 };
                    }
                    else
                    {
                        possible = false;
                        break;
                    }
                }
                result.Add(assignedTables);
                if (!possible) break;
            }
            
            if (possible)
            {
                Console.WriteLine(1);
                foreach (var teamTables in result)
                {
                    teamTables.Sort();
                    Console.WriteLine(string.Join(" ", teamTables));
                }
            }
            else
            {
                Console.WriteLine(0);
            }
        }
    }
}