namespace Pitpo;

// Находит минимальное доминирующее множество в неориентированном графе.
public static class ServicingStations
{
    private const int MaxTown = 35;            // Максимальное количество городов (вершин)
    private static bool _finish;               // Флаг досрочного завершения поиска
    private static int _minimum;               // Найденный минимальный размер доминирующего множества
    private static List<ulong> _edges;         // Битовые маски для каждой вершины (вершина + её соседи)
    private static ulong _targetTag;           // Битовая маска, представляющая все вершины
    private static ulong _originTag;           // Маска уже покрытых вершин (от удалённых вершин)
    
    // Проверяет, является ли выбранное подмножество вершин доминирующим множеством.
    // flag Массив индексов выбранных вершин в списке _edges
    // position Количество выбранных вершин
    private static void Check(int[] flag, int position)
    {
        var idx = 0;
        var newTag = _originTag;
        var oldTag = _originTag;

        for (var i = 0; i < _edges.Count; i++)
        {
            if (idx < position && flag[idx] == i)
            {
                newTag |= _edges[i];
                if (newTag > oldTag)
                    oldTag = newTag;
                else
                    return;    // Нет прогресса – бесполезная комбинация
                idx++;
            }
        }

        if (newTag == _targetTag)
        {
            _minimum = idx;
            _finish = true;
        }
    }
    
    // Генерирует подмножества вершин в лексикографическом порядке (Banker's sequence).
    // flag Массив индексов выбранных вершин
    // position Текущая глубина (количество выбранных вершин)
    // positions Общее количество вершин, которое должно быть выбрано
    private static void Generate(int[] flag, int position, int positions)
    {
        if (_finish) return;

        if (position < positions)
        {
            if (position == 0)
            {
                for (var i = 0; i < _edges.Count; i++)
                {
                    flag[position] = i;
                    Generate(flag, position + 1, positions);
                }
            }
            else
            {
                for (var i = flag[position - 1] + 1; i < _edges.Count; i++)
                {
                    flag[position] = i;
                    Generate(flag, position + 1, positions);
                }
            }
        }
        else
        {
            Check(flag, position);
        }
    }
    
    // Перебирает все подмножества вершин, начиная с наименьшего размера, для поиска минимального доминирующего множества.
    private static void EnumerateSubsets()
    {
        for (var i = 1; i <= _edges.Count; i++)
        {
            var flag = new int[_edges.Count];
            Generate(flag, 0, i);
            if (_finish)
                return;
        }
    }
    
    // Функция сравнения для сортировки битовых масок по убыванию (чем больше единиц, тем выше приоритет).
    private static int CompareEdges(ulong x, ulong y)
    {
        return y.CompareTo(x);
    }
    
    // Вычисляет размер минимального доминирующего множества для связного графа.
    // Использует битовые маски и эвристики для ускорения.
    // graph Граф, представленный списком смежности (0‑индексация)
    // Размер минимального доминирующего множества
    private static int Mdsn(List<List<int>> graph)
    {
        var baseCount = 0;
        _originTag = 0;
        _targetTag = 0;

        var n = graph.Count;
        var dirty = new bool[n];

        // Предварительная обработка: изолированные вершины и висячие вершины
        for (var i = 0; i < n; i++)
        {
            if (graph[i].Count == 0)   // изолированная вершина
            {
                baseCount++;
                _originTag |= (1UL << i);
            }

            if (graph[i].Count == 1 && !dirty[i])
            {
                dirty[i] = true;
                var neighbor = graph[i][0];
                if (!dirty[neighbor])
                {
                    baseCount++;
                    dirty[neighbor] = true;
                }
            }

            _targetTag |= (1UL << i);
        }

        // Построение битовых масок для оставшихся вершин
        _edges = [];
        for (var i = 0; i < n; i++)
        {
            if (dirty[i])
            {
                _originTag |= (1UL << i);
                foreach (int nb in graph[i])
                    _originTag |= (1UL << nb);
            }

            if (!dirty[i] && graph[i].Count > 0)
            {
                var mask = (1UL << i);
                foreach (var nb in graph[i])
                    mask |= (1UL << nb);
                _edges.Add(mask);
            }
        }

        // Сортировка масок: сначала те, у которых больше битов (степень вершины)
        _edges.Sort(CompareEdges);

        _minimum = 0;
        _finish = false;
        EnumerateSubsets();

        return baseCount + _minimum;
    }
    
    // Разбивает граф на компоненты связности и суммирует размеры минимальных доминирующих множеств каждой компоненты.
    // graph Исходный граф (список смежности)
    // Общий размер минимального доминирующего множества
    private static int Servicing(List<List<int>> graph)
    {
        var total = 0;
        // Создаём копию для манипуляций
        List<List<int>> remaining = [];
        foreach (var v in graph)
            remaining.Add(new List<int>(v));

        while (remaining.Count > 0)
        {
            // Поиск компоненты связности с помощью BFS
            List<List<int>> component = [];
            HashSet<int> visited = [];

            component.Add(remaining[0]);
            visited.Add(remaining[0][0]);

            remaining.RemoveAt(0);

            var size = 0;
            while (component.Count > size)
            {
                var oldSize = size;
                var current = component.Count - 1;
                size = component.Count;

                for (var i = oldSize; i <= current; i++)
                {
                    for (var j = 1; j < component[i].Count; j++)
                    {
                        var neighbor = component[i][j];
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            for (var k = 0; k < remaining.Count; k++)
                            {
                                if (remaining[k][0] == neighbor)
                                {
                                    component.Add(remaining[k]);
                                    remaining.RemoveAt(k);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Переиндексация вершин компоненты в диапазон 0..(component.Count-1)
            // для передачи в Mdsn.
            var reindex = new Dictionary<int, int>();
            var idx = 0;
            foreach (var vertex in component)
            {
                reindex[vertex[0]] = idx++;
            }

            List<List<int>> subGraph = [];
            foreach (var t in component)
            {
                List<int> neighbors = [];
                foreach (var nb in t.Skip(1))
                    neighbors.Add(reindex[nb]);
                subGraph.Add(neighbors);
            }

            total += Mdsn(subGraph);
        }

        return total;
    }

    public static void Start()
    {
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            var parts = line.Split();
            var n = int.Parse(parts[0]);
            var m = int.Parse(parts[1]);
            if (n == 0 && m == 0) break;

            // Инициализация графа: список смежности
            List<List<int>> graph = [];
            for (var i = 0; i < n; i++)
                graph.Add([]);

            for (var i = 0; i < m; i++)
            {
                parts = Console.ReadLine().Split();
                var x = int.Parse(parts[0]) - 1; // переводим в 0‑индексацию
                var y = int.Parse(parts[1]) - 1;
                if (x == y) continue; // петли игнорируем
                graph[x].Add(y);
                graph[y].Add(x);
            }

            var result = Servicing(graph);
            Console.WriteLine(result);
        }
    }
}