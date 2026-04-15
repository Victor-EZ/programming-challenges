namespace Pitpo;

public class UnidirectionalTSP
{
    public static void Start()
    {
        while (Console.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue; // Пропускаем пустые строки

            // Разбираем размеры матрицы: m строк, n столбцов
            var parts = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;
            if (!int.TryParse(parts[0], out var m) || !int.TryParse(parts[1], out var n)) continue;
            var matrix = new int[m, n];
            for (var i = 0; i < m; i++)
            {
                // Читаем строку с числами
                var rowValues = 
                    Console.ReadLine()?.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
                if (rowValues == null || rowValues.Length < n)
                {
                    i--; // повторить попытку
                    continue;
                }
                for (var j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                }
            }
            
            var dp = new int[m, n]; // Минимальная сумма пути от (m, n) до последнего столбца
            var nextRow = new int[m, n]; // Номер строки следующей клетки на оптимальном пути

            // Инициализируем dp большими значениями
            for (var i = 0; i < m; i++)
                for (var j = 0; j < n; j++) dp[i, j] = int.MaxValue;

            // Последний столбец: стоимость пути равна значению клетки
            for (var i = 0; i < m; i++)
            {
                dp[i, n - 1] = matrix[i, n - 1];
                nextRow[i, n - 1] = -1; // у последнего столбца следующей строки нет
            }

            // Заполняем таблицу справа налево, начиная с предпоследнего столбца
            for (var j = n - 2; j >= 0; j--)
            {
                for (var i = 0; i < m; i++)
                {
                    // Три возможных направления с учётом цикличности (первая и последняя строки соседствуют)
                    var up = (i - 1 + m) % m;
                    var down = (i + 1) % m;
                    int[] candidates = { up, i, down };
                    Array.Sort(candidates); // Сортируем для лексикографически минимального пути при равной стоимости
                    foreach (var r in candidates)
                    {
                        var cost = dp[r, j + 1] + matrix[i, j];
                        if (cost >= dp[i, j]) continue;
                        dp[i, j] = cost;
                        nextRow[i, j] = r;
                    }
                }
            }

            // Находим наилучший старт в первом столбце (минимальная стоимость)
            var minCost = int.MaxValue;
            var startRow = 0;
            for (var i = 0; i < m; i++)
            {
                if (dp[i, 0] >= minCost) continue;
                minCost = dp[i, 0];
                startRow = i;
            }

            // Восстанавливаем путь
            var path = new List<int>();
            var curRow = startRow;
            for (var j = 0; j < n; j++)
            {
                path.Add(curRow + 1); // переводим в 1-индексацию для вывода
                curRow = nextRow[curRow, j];
            }
            
            Console.WriteLine(string.Join(" ", path));
            Console.WriteLine(minCost);
        }
    }
}