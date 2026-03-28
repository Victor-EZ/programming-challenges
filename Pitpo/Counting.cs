using System.Text;
namespace Pitpo;

public class Counting
{
    public static void Start()
    {
        //Предварительно вычисляем все значения F(n) для n от 1 до 1000
        var counting = InitializeCounting();
        string line;
        while ((line = Console.ReadLine()) != null)
        {
            var parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (int.TryParse(part, out var n) && n is >= 1 and <= 1000)
                {
                    var result = counting[n - 1]; //Выводим F(n) в правильном порядке
                    var chars = result.ToCharArray();
                    Array.Reverse(chars); //Переворачиваем строку, так как она хранится в обратном порядке
                    Console.WriteLine(new string(chars));
                }
            }
        }
    }
    
    //Значения хранятся в виде строк в обратном порядке (младший разряд в начале строки)
    //для удобства выполнения сложений с переносом.
    private static List<string> InitializeCounting()
    {
        const int maxN = 1000;
        var counting = new List<string>(maxN)
        {
            //Базовые значения
            "2",
            "5",
            "31" //F3 = 13, тк перевёрнуто
        };
        for (var i = 3; i < maxN; i++)
        {
            //следующее значение F(i+1) = 2*F(i) + F(i-1) + F(i-2)
            var nextValue = NextValue(counting[i - 1], counting[i - 2], counting[i - 3]);
            counting.Add(nextValue);
        }
        return counting;
    }
    
    //Вычисляет следующее число по формуле: result = 2*a + b + c.
    //Все числа представлены в виде строк в обратном порядке (младший разряд слева).
    private static string NextValue(string a, string b, string c)
    {
        // Сначала вычисляем 2*a (удвоение числа a)
        var doubledA = new StringBuilder();
        var carry = 0;
        for (var i = 0; i < a.Length; i++)
        {
            var digit = a[i] - '0';
            var sum = carry + digit * 2;
            carry = sum / 10;
            doubledA.Append((char)('0' + sum % 10));
        }
        if (carry > 0)
        {
            doubledA.Append((char)('0' + carry));
        }

        // Теперь складываем (2*a) + b + c
        // Дополняем b и c нулями до длины doubledA, если необходимо
        var strB = PadRight(b, doubledA.Length);
        var strC = PadRight(c, doubledA.Length);
        var strA = doubledA.ToString();

        var result = new StringBuilder();
        carry = 0;
        for (var i = 0; i < strA.Length; i++)
        {
            var digitA = strA[i] - '0';
            var digitB = strB[i] - '0';
            var digitC = strC[i] - '0';
            var sum = carry + digitA + digitB + digitC;
            carry = sum / 10;
            result.Append((char)('0' + sum % 10));
        }
        if (carry > 0)
        {
            result.Append((char)('0' + carry));
        }
        return result.ToString();
    }
    
    // Дополняет строку справа нулями до указанной длины.
    private static string PadRight(string s, int length)
    {
        if (s.Length >= length) return s;
        return s + new string('0', length - s.Length);
    }
}