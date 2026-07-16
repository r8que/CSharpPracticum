void PrintDiamond(int n)
{
    int mid = n / 2;

    // Верх + центр
    for (int i = 0; i <= mid; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (j == mid - i || j == mid + i)
                Console.Write("X");
            else
                Console.Write(" ");
        }
        Console.WriteLine();
    }

    // Низ
    for (int i = mid - 1; i >= 0; i--)
    {
        for (int j = 0; j < n; j++)
        {
            if (j == mid - i || j == mid + i)
                Console.Write("X");
            else
                Console.Write(" ");
        }
        Console.WriteLine();
    }
}

Console.WriteLine("Введите N");
int n = Convert.ToInt32(Console.ReadLine());                    
PrintDiamond(n);