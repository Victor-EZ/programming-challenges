using Pitpo;

void Task1()
{
    while (true)
    {
        var input = Console.ReadLine().Split(' ');
        var size = Convert.ToInt32(input[0]);
        var number = Convert.ToInt32(input[1]);
        if (size != 0 || number != 0)
        {
            LcdDisplay.Display(size, number, number.ToString().Length);
            Console.WriteLine("");
        }
        else break;
    }
}

void Task2()
{
    ErdosNum.Start();
}

void Task3()
{
    EncryptionII.Start();
    /*
input {
1

vtz ud xnm xugm itr pyy jttk gmv xt otgm xt xnm puk ti xnm fprxq
xnm ceoub lrtzv ita hegfd tsmr xnm ypwq ktj
frtjrpgguvj otvxmdxd prm iev prmvx xnmq
}
    output {
    now is the time for all good men to come to the aid of the party 
    the quick brown fox jumps over the lazy dog 
    programming contests are fun arent they
    }
     */
}

void Task4()
{
    ShoemakersProblem.Start();
    /*
1
4
3 4
1 1000
2 2
5 5
     */
}

void Task5()
{
    Ones.Start();
    /*
enter
3
7
9901

res
3
6
12

    */
}

void Task6()
{
    Counting.Start();
    /*
    enter
    2
    3

    result
    5
    13
    */
}

void Task7()
{
    ServicingStations.Start();
    /*
     enter
8 12
1 2
1 6
1 8
2 3
2 6
3 4
3 5
4 5
4 7
5 6
6 7
6 8
0 0

    result
    2
     */
}

void Task8()
{
    UnidirectionalTSP.Start();
    /*
     enter
5 6
3 4 1 2 8 6
6 1 8 2 7 4
5 9 3 9 9 5
8 4 1 3 2 6
3 7 2 8 6 4

5 6
3 4 1 2 8 6
6 1 8 2 7 4
5 9 3 9 9 5
8 4 1 3 2 6

2 2
9 10 9 10

     result
     1 2 3 4 4 5
     16
     
     1 2 1 5 4 5
     11
     
     1 1
     19
     */
}

Task7();
