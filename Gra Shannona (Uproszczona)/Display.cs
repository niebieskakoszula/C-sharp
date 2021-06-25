using System;
using System.Collections.Generic;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    class Display
    {
        int size;
        string[,] chars;
        ConsoleColor[,] colors;
        ConsoleColor first_player;
        ConsoleColor second_player;
        bool is_first_last_moving_player;
        public Display(int game_size)
        {
            Choose_Player_Colors();
            this.size = game_size;
            Prepare_Arrays();
        }
        void Prepare_Arrays()
        {
            chars = new string[(size * 2) - 1, (size * 2) - 1];
            colors = new ConsoleColor[(size * 2) - 1, (size * 2) - 1];
            for (int i = 0; i < (size * 2) - 2; i += 2)
            {
                for (int j = 0; j < (size * 2) - 2; j += 2)
                {
                    chars[i, j + 1] = Convert.ToString((i / 2) + 1); colors[i, j + 1] = first_player;
                    chars[i, j] = " "; colors[i, j] = ConsoleColor.Black;
                    chars[i + 1, j] = Convert.ToString((j / 2) + 1); colors[i + 1, j] = second_player;
                    chars[i + 1, j + 1] = " "; colors[i + 1, j + 1] = ConsoleColor.Black;
                }
                chars[i, (size * 2) - 2] = " "; colors[i, (size * 2) - 2] = ConsoleColor.Black;
                chars[i + 1, (size * 2) - 2] = Convert.ToString(size); colors[i + 1, (size * 2) - 2] = second_player;
            }
            for (int j = 0; j < (size * 2) - 2; j += 2)
            {
                chars[(size * 2) - 2, j + 1] = Convert.ToString(size); colors[(size * 2) - 2, j + 1] = first_player;
                chars[(size * 2) - 2, j] = " "; colors[(size * 2) - 2, j] = ConsoleColor.Black;
            }
            chars[(size * 2) - 2, (size * 2) - 2] = " "; colors[(size * 2) - 2, (size * 2) - 2] = ConsoleColor.Black;
        }
        void Choose_Player_Colors()
        {
            Console.WriteLine("Wybierz kolor pierwszego gracza:");
            first_player = Choose_Color();
            Console.WriteLine("Wybierz kolor drugiego gracza:");
            second_player = Choose_Color();
        }
        ConsoleColor Choose_Color()
        {
            Console.WriteLine("Wybierz swój kolor:");
            Console.WriteLine("1. Niebieski");
            Console.WriteLine("2. Ciemny niebieski");
            Console.WriteLine("3. Żółty");
            Console.WriteLine("4. Ciemny żółty");
            Console.WriteLine("5. Czerwony");
            Console.WriteLine("6. Ciemny czerwony");
            Console.WriteLine("7. Zielony");
            Console.WriteLine("8. Ciemny zielony");
            Console.WriteLine("9. Cyjan");
            Console.WriteLine("10. Ciemny cyjan");
            Console.WriteLine("11. Magenta");
            Console.WriteLine("12. Ciemna magenta");
            Console.WriteLine("W innym wypadku wybrany zostanie losowy kolor");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1: return ConsoleColor.Blue;
                case 2: return ConsoleColor.DarkBlue;
                case 3: return ConsoleColor.Yellow;
                case 4: return ConsoleColor.DarkYellow;
                case 5: return ConsoleColor.Red;
                case 6: return ConsoleColor.DarkRed;
                case 7: return ConsoleColor.Green;
                case 8: return ConsoleColor.DarkGreen;
                case 9: return ConsoleColor.Cyan;
                case 10: return ConsoleColor.DarkCyan;
                case 11: return ConsoleColor.Magenta;
                case 12: return ConsoleColor.DarkMagenta;
                default:
                    var kolory = Enum.GetValues(typeof(ConsoleColor));
                    ConsoleColor kolor;
                    do
                    {
                        kolor = (ConsoleColor)kolory.GetValue((new Random()).Next(kolory.Length));
                    } while (kolor == ConsoleColor.White);
                    return kolor;
            }
        }


        public void Mark_Win(bool is_player_one, int[] move)
        {
            int x, y;
            if (is_player_one) { x = (move[1] - 1) * 2; y = ((move[0] - 1) * 2) + 1; }
            else { x = ((move[0] - 1) * 2) + 1; y = ((move[1] - 1) * 2); }

            Queue<int> coordinates = new Queue<int>();
            coordinates.Enqueue(x); coordinates.Enqueue(y);
            colors[x, y] = ConsoleColor.White;

            while (coordinates.Count != 0)
            {
                x = coordinates.Peek(); coordinates.Dequeue();
                y = coordinates.Peek(); coordinates.Dequeue();

                if (y - 1 >= 0 && chars[x, y - 1] == "-" && colors[x, y - 1] != colors[x, y])
                {
                    colors[x, y - 1] = ConsoleColor.White;
                    colors[x, y - 2] = ConsoleColor.White;
                    coordinates.Enqueue(x); coordinates.Enqueue(y - 2);
                }
                if (x + 1 <= (size * 2) - 2 && chars[x + 1, y] == "|" && colors[x + 1, y] != colors[x, y])
                {
                    colors[x + 1, y] = ConsoleColor.White;
                    colors[x + 2, y] = ConsoleColor.White;
                    coordinates.Enqueue(x + 2); coordinates.Enqueue(y);
                }
                if (y + 1 <= (size * 2) - 2 && chars[x, y + 1] == "-" && colors[x, y + 1] != colors[x, y])
                {
                    colors[x, y + 1] = ConsoleColor.White;
                    colors[x, y + 2] = ConsoleColor.White;
                    coordinates.Enqueue(x); coordinates.Enqueue(y + 2);
                }
                if (x - 1 >= 0 && chars[x - 1, y] == "|" && colors[x - 1, y] != colors[x, y])
                {
                    colors[x - 1, y] = ConsoleColor.White;
                    colors[x - 2, y] = ConsoleColor.White;
                    coordinates.Enqueue(x - 2); coordinates.Enqueue(y);
                }
            }
        }
        public void Show()
        {
            Console.Clear();
            Console.Write("    ");
            Console.ForegroundColor = first_player;
            for (int j = 0; j < size - 1; j++) Console.Write(j + 1 + " ");
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < (size * 2) - 1; i++)
            {
                if (i % 2 != 0)
                {
                    Console.ForegroundColor = second_player;
                    Console.Write((i / 2) + 1);
                }
                else Console.Write(" ");

                Console.Write("  ");
                for (int j = 0; j < (size * 2) - 1; j++)
                {
                    Console.ForegroundColor = colors[i, j];
                    Console.Write(chars[i, j]);
                }
                Console.WriteLine();
            }

            Change_Foreground_To_Match_Player();
        }
        void Change_Foreground_To_Match_Player()
        {
            if (is_first_last_moving_player) Console.ForegroundColor = first_player;
            else Console.ForegroundColor = second_player;
        }
        public void Correct_Display_Data(bool is_player_one, int[] move)
        {
            if (is_player_one)
            {
                is_first_last_moving_player = true;
                if (move[0] == move[2]) { chars[((move[1] - 1) * 2) + 1, ((move[0] - 1) * 2) + 1] = "|"; colors[((move[1] - 1) * 2) + 1, ((move[0] - 1) * 2) + 1] = first_player; }  
                else { chars[(move[1] - 1) * 2, move[0] * 2] = "-"; colors[(move[1] - 1) * 2, move[0] * 2] = first_player; }
            }
            else
            {
                is_first_last_moving_player = false;
                if (move[0] == move[2]) { chars[((move[0] - 1) * 2) + 1, ((move[1] - 1) * 2) + 1] = "-"; colors[((move[0] - 1) * 2) + 1, ((move[1] - 1) * 2) + 1] = second_player; }
                else { chars[move[0] * 2, ((move[1] - 1) * 2)] = "|"; colors[move[0] * 2, ((move[1] - 1) * 2)] = second_player; }
            }
        }

        public void Reset()
        {
            Prepare_Arrays();
        }
    }
}
