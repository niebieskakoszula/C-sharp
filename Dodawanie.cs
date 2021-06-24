//https://pl.spoj.com/problems/JSUMDUZE/

using System;
using System.Numerics;

namespace JSUMDUZE___Dodawanie
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger first;
            BigInteger second;
            BigInteger result;
            string[] input;
            int tests = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < tests; i++)
            {
                input = Console.ReadLine().Split(' ');
                first = BigInteger.Parse(input[0]);
                second = BigInteger.Parse(input[1]);
                result = BigInteger.Add(first, second);
                Console.WriteLine(result);
            }
        }
    }
}

