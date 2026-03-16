using Pitpo;

while (true)
{
    var input = Console.ReadLine().Split(' ');
    var size = Convert.ToInt32(input[0]);
    var number = Convert.ToInt32(input[1]);
    if (size != 0 || number != 0)
    {
        LCDdisplay.display(size, number, number.ToString().Length);
        Console.WriteLine("");
    }
    else break;
}