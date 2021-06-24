//https://pl.spoj.com/problems/JSUMDUZE/

using System;
using System.Text;
using System.Numerics;

namespace JSUMDUZE___Dodawanie
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger pierwsza;
            BigInteger druga;
            BigInteger wynik;
            int t;
            string wczytywanie;
            string[] temp = new string[2];
            t = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < t; i++)
            {
                pierwsza = new BigInteger();
                druga = new BigInteger();
                wynik = new BigInteger();
                wczytywanie = Console.ReadLine();
                temp = wczytywanie.Split(' ');
                pierwsza = BigInteger.Parse(temp[0]);
                druga = BigInteger.Parse(temp[1]);
                wynik = BigInteger.Add(pierwsza, druga);
                Console.WriteLine(wynik);
            }
        }
    }
}
