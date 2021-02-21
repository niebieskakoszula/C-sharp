//https://pl.spoj.com/problems/ETI06F2/

using System;

namespace ETI06F2___Ciekawa_wyliczanka
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 1, k;
            double p = 0;
            k = Convert.ToInt32(Console.ReadLine());
            while (k > p)
            {
                p += Math.Pow(2, i);
                i++;
            }
            i--;
            int dlugosc = Convert.ToInt32(Math.Pow(2, i) - (p - k));

            for (int j = i; j > 0; j--)
            {
                if (Math.Ceiling(dlugosc / Math.Pow(2, j - 1)) % 2 == 0)
                    Console.Write("6");
                else
                    Console.Write("5");
            }
        }
    }
}
