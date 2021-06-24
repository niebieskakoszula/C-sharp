//https://pl.spoj.com/problems/ETI07E1/

using System;

namespace ETI07E1___Bajtlandzka_wojenka
{
    static class Fairyland_war
    {
        static bool[] cards;
        const int amount_of_cards = 52;
        public static void Write_Number_Of_Wins()
        {
            Mark_Owned_Cards();
            Console.WriteLine(Count_Wins());
        } 
        static void Mark_Owned_Cards()
        {
            cards = new bool[amount_of_cards + 1];
            int[] input = Load_Input();
            //pleyer owns half of cards in game
            for (int i = 0; i < amount_of_cards / 2; i++)
                cards[input[i]] = true;
        }
        static int[] Load_Input()
        {
            int[] result = new int[amount_of_cards / 2];
            string[] input = Console.ReadLine().Split(' ');
            for (int i = 0; i < amount_of_cards / 2; i++)
                result[i] = Convert.ToInt32(input[i]);

            return result;
        }

        static int Count_Wins()
        {
            int wins = 0, amount_of_enemy_cards = 0;
            for (int i = 1; i <= amount_of_cards; i++)
            {
                if (!cards[i])
                {
                    amount_of_enemy_cards++;
                }
                else if(amount_of_enemy_cards > 0)
                {
                    amount_of_enemy_cards--;
                    wins++;
                }
            }
            return wins;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Fairyland_war.Write_Number_Of_Wins();
        }
    }
}
