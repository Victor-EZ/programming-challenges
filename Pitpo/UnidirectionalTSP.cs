namespace Pitpo;

public class UnidirectionalTSP
{
    public static void Start()
    {
        string line;
        // Читаем входные данные до конца файла
        while ((line = Console.ReadLine()) != null)
        {
            // Пропускаем пустые строки
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Разбираем размеры матрицы: m строк, n столбцов
            var parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                continue;

            if (!int.TryParse(parts[0], out var m) || !int.TryParse(parts[1], out var n))
                continue;

            // Читаем матрицу размером m x n
            var matrix = new int[m, n];
            for (var i = 0; i < m; i++)
            {
                // Читаем строку с числами (может быть разбита на несколько строк, но в задаче обычно одна строка на строку матрицы)
                var rowValues = Console.ReadLine()?.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (rowValues == null || rowValues.Length < n)
                {
                    // Если строка не содержит всех n чисел, пытаемся дочитать недостающие
                    // Но для простоты считаем, что данные корректны
                    i--; // повторить попытку
                    continue;
                }
                for (var j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(rowValues[j]);
                }
            }

            // Таблица DP: dp[i, j] – минимальная сумма пути от (i, j) до последнего столбца
            var dp = new int[m, n];
            // Таблица nextRow[i, j] – номер строки следующей клетки на оптимальном пути
            var nextRow = new int[m, n];

            // Инициализируем dp большими значениями
            for (var i = 0; i < m; i++)
                for (var j = 0; j < n; j++)
                    dp[i, j] = int.MaxValue;

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
                    // Сортируем для лексикографически минимального пути при равной стоимости
                    Array.Sort(candidates);

                    foreach (var r in candidates)
                    {
                        var cost = dp[r, j + 1] + matrix[i, j];
                        if (cost < dp[i, j])
                        {
                            dp[i, j] = cost;
                            nextRow[i, j] = r;
                        }
                    }
                }
            }

            // Находим наилучший старт в первом столбце (минимальная стоимость)
            var minCost = int.MaxValue;
            var startRow = 0;
            for (var i = 0; i < m; i++)
            {
                if (dp[i, 0] < minCost)
                {
                    minCost = dp[i, 0];
                    startRow = i;
                }
            }

            // Восстанавливаем путь
            var path = new List<int>();
            var curRow = startRow;
            for (var j = 0; j < n; j++)
            {
                path.Add(curRow + 1); // переводим в 1-индексацию для вывода
                curRow = nextRow[curRow, j];
            }

            // Выводим путь
            Console.WriteLine(string.Join(" ", path));
            // Выводим минимальную стоимость
            Console.WriteLine(minCost);
        }
    }
}