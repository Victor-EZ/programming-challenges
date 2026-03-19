namespace Pitpo;

public static class ShoemakersProblem
{
    private struct Order
    {
        public int Days;
        public int Fine;
        public int Index;
    }

    public static void Start()
    {
        var blocks = int.Parse(Console.ReadLine()!);
        for (var t = 0; t < blocks; t++)
        {
            var tasksCount = int.Parse(Console.ReadLine()!);
            var orders = new Order[tasksCount];
            for (var i = 0; i < tasksCount; i++)
            {
                var parts = Console.ReadLine()!.Split();
                orders[i].Days = int.Parse(parts[0]);
                orders[i].Fine = int.Parse(parts[1]);
                orders[i].Index = i + 1;
            }

            //Сравниваем штраф за количество дней
            Array.Sort(orders, (x, y) =>
            {
                //long, чтобы избежать переполнения
                var lhs = (long)x.Days * y.Fine;
                var rhs = (long)y.Days * x.Fine;
                if (lhs < rhs) return -1;
                if (lhs > rhs) return 1;
                return 0; //При равенстве порядок не важен
            });
            Console.WriteLine(string.Join(" ", orders.Select(o => o.Index)));
            if (t < blocks - 1) Console.WriteLine(); //Пустая строка между блоками
        }
    }
}