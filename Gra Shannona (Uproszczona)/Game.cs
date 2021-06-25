using System;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{

    class Game
    {
        Player first;
        Player second;
        int size;
        Abstract_Field[,] first_player_fields;
        Abstract_Field[,] second_player_fields;
        Blockade blockade = new Blockade();
        Display display;
        bool is_prepared;
        public void PrepareGame()
        {
            Choose_Size();
            PrepareArrays();
            Console.WriteLine("wybierz pierwszego gracza:");
            first = Choose_Type_Of_Player();

            Console.WriteLine("wybierz drugiego gracza:");
            second = Choose_Type_Of_Player();

            display = new Display(size);
            is_prepared = true;
        }
        void Choose_Size()
        {
            do
            {
                Console.WriteLine("Podaj rozmiar planszy (nie mniejszy niż 3): ");
                size = Convert.ToInt32(Console.ReadLine());
            } while (size < 3);
        }
        void PrepareArrays()
        {
            first_player_fields = new Abstract_Field[size + 1, size + 2];
            second_player_fields = new Abstract_Field[size + 1, size + 2];

            for (int i = 0; i < size + 1; i++)
            {
                first_player_fields[i, 0] = blockade;
                second_player_fields[i, 0] = blockade;

                first_player_fields[0, i] = blockade;
                second_player_fields[0, i] = blockade;

                first_player_fields[size, i] = blockade;
                second_player_fields[size, i] = blockade;

                first_player_fields[i, size + 1] = blockade;
                second_player_fields[i, size + 1] = blockade;
            }

            for (int i = 1; i < size; i++)
            {
                first_player_fields[i, 1] = new Field(0, 1);
                second_player_fields[i, 1] = new Field(0, 1);
                first_player_fields[i, size] = new Field(size - 1, 2);
                second_player_fields[i, size] = new Field(size - 1, 2);
                for (int j = 2; j < size; j++)
                {
                    first_player_fields[i, j] = new Field(j - 1);
                    second_player_fields[i, j] = new Field(j - 1);
                }
            }
            for (int i = 1; i < size; i++)
            {
                for (int j = 1; j <= size; j++)
                {
                    ((Field)first_player_fields[i, j]).Set_Neighbors(first_player_fields[i, j - 1], first_player_fields[i + 1, j], first_player_fields[i, j + 1], first_player_fields[i - 1, j]);
                    ((Field)second_player_fields[i, j]).Set_Neighbors(second_player_fields[i, j - 1], second_player_fields[i + 1, j], second_player_fields[i, j + 1], second_player_fields[i - 1, j]);
                }
            }
        }
        Player Choose_Type_Of_Player()
        {
            Console.WriteLine("1 Człowiek");
            Console.WriteLine("2 Bot bloker");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1: return new Person(size, ref second_player_fields, ref first_player_fields);
                case 2: return new BotBloker(size, ref second_player_fields, ref first_player_fields);
                default: return new Person(size, ref second_player_fields, ref first_player_fields);
            }
        }

        public void Play()
        {
            if (!is_prepared)
            {
                Console.WriteLine("Nie przygotowano rozgrywki!\n(wciśnij dowolny przycisk aby kontynuować)");
                Console.ReadKey();
                return;
            }
            

            bool is_first_player_move = true, end_of_game = false, active_display;
            int[] move;
            if (first is Bot && second is Bot) active_display = false;
            else active_display = true;

            display.Show();
            do
            {
                if (is_first_player_move)
                {
                    Console.WriteLine("Ruch gracza 1");
                    move = first.Move();
                    display.Correct_Display_Data(true, move);
                    if(active_display) display.Show();
                    if (Check_For_Win(move, true)) end_of_game = true;
                }
                else
                {
                    Console.WriteLine("Ruch gracza 2");
                    move = second.Move();
                    display.Correct_Display_Data(false, move);
                    if (active_display) display.Show();
                    if (Check_For_Win(move, false)) end_of_game = true;
                }
                is_first_player_move = !is_first_player_move;
            } while (!end_of_game);
            Console.WriteLine("(Wciśnij dowolny przycisk aby zakończyć)");
            Console.ReadKey();
        }
        bool Check_For_Win(int[] move, bool is_first)
        {
            for (int i = 0; i < 4; i++)
            {
                if(move[i] == -int.MaxValue)
                {
                    if (is_first) Console.WriteLine("Gracz pierwszy się poddaje - wygrywa Gacz Drugi!");
                    else Console.WriteLine("Gracz Drugi się poddaje - wygrywa Gacz Pierwszy!");
                    return true;
                }
            }

            if (is_first)
            {
                if(first_player_fields[move[0], move[1]].Which_end == 3 || first_player_fields[move[2], move[3]].Which_end == 3)
                {
                    display.Mark_Win(is_first, move);
                    display.Show();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Wygrywa Gracz Pierwszy!");
                    return true;
                }
            }
            else
            {
                if (second_player_fields[move[0], move[1]].Which_end == 3 || second_player_fields[move[2], move[3]].Which_end == 3)
                {
                    display.Mark_Win(is_first, move);
                    display.Show();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Wygrywa Gracz Drugi!");
                    return true;
                }
            }
            return false;
        }


        public void Reset()
        {
            if (!is_prepared) PrepareGame();
            PrepareArrays();
            display.Reset();
        }
    }
}
