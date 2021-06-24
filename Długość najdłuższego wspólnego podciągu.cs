//https://pl.spoj.com/problems/LENLCS/

using System;

namespace LENLCS___Długość_najdłuższego_wspólnego_podciągu
{
    class Substring_Calculator
    {
        static string word;
        static int word_lenght;

        static string sub_word;
        static int sub_word_lenght;

        static int[,] result;

        public static int Find_Longest_Substring_Lenght()
        {
            Read_Input();
            Initialize();
            Calculate();
            return result[word_lenght,sub_word_lenght];
        }

        static void Read_Input()
        {
            word_lenght = Convert.ToInt32(Console.ReadLine());
            word = Console.ReadLine();
            sub_word_lenght = Convert.ToInt32(Console.ReadLine());
            sub_word = Console.ReadLine();
        }
        static void Initialize()
        {
            result = new int[word_lenght + 1, sub_word_lenght + 1];
            for (int i = 0; i < word_lenght; i++)
                result[i, 0] = 0;
            for (int i = 0; i < sub_word_lenght; i++)
                result[0, i] = 0;
        }
        static void Calculate()
        {
            for (int i = 1; i <= word_lenght; i++)
            {
                for (int j = 1; j <= sub_word_lenght; j++)
                {
                    if (word[i - 1] == sub_word[j - 1])
                        result[i, j] = result[i - 1, j - 1] + 1;
                    else
                        result[i, j] = Math.Max(result[i - 1, j], result[i, j - 1]);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int tests = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < tests; i++)
            {
                Console.WriteLine(Substring_Calculator.Find_Longest_Substring_Lenght());
            }
        }
    }
}
