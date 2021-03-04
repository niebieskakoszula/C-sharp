//https://pl.spoj.com/problems/ETI07E1/

using System;

namespace ETI07E1___Bajtlandzka_wojenka
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[] karty = new bool[53];
            string temp = Console.ReadLine();
            string[] wejscie = temp.Split(' ');
            int zwyciestwa = 0, maxx = 53, j;

            for (int i = 0; i < 26; i++)
            {
                karty[Convert.ToInt32(wejscie[i])] = true;
            }
            karty[0] = true;

            for (int i = 51; i > 0; i--)
            {
                if (!karty[i])
                {
                    for (j = i; j < maxx; j++)
                    {
                        if (karty[j])
                        {
                            karty[j] = false;
                            zwyciestwa++;
                            break;
                        }
                    }
                    if (j == 52)
                    {
                        for (j = 0; j < i; j++)
                        {
                            if (karty[j])
                            {
                                karty[j] = false;
                                break;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(zwyciestwa);
        }
    }
}
