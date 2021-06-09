using System;
using System.Collections.Generic;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    class Wyswietlanie
    {
        int n;
        string[,] znaki;
        ConsoleColor[,] kolory;
        ConsoleColor pierwszy;
        ConsoleColor drugi;
        public ConsoleColor Pierwszy
        {
            get => pierwszy;
            set
            {
                if (value != drugi) pierwszy = value;
                else Console.WriteLine("kolor już zajęty");
            }
        }
        public ConsoleColor Drugi
        {
            get => drugi;
            set
            {
                if (value != pierwszy) drugi = value;
                else Console.WriteLine("kolor już zajęty");
            }
        }
        public Wyswietlanie(int nn, ConsoleColor p, ConsoleColor d)
        {
            pierwszy = p;
            drugi = d;
            n = nn;
            znaki = new string[(n * 2) - 1, (n * 2) - 1];
            kolory = new ConsoleColor[(n * 2) - 1, (n * 2) - 1];

            for (int i = 0; i < (n * 2) - 2; i += 2)
            {
                for (int j = 0; j < (n * 2) - 2; j += 2)
                {
                    znaki[i, j + 1] = Convert.ToString((i / 2) + 1); kolory[i, j + 1] = pierwszy;
                    znaki[i, j] = " "; kolory[i, j] = ConsoleColor.Black;
                    znaki[i + 1, j] = Convert.ToString((j / 2) + 1); kolory[i + 1, j] = drugi;
                    znaki[i + 1, j + 1] = " "; kolory[i + 1, j + 1] = ConsoleColor.Black;
                }
                znaki[i, (n * 2) - 2] = " "; kolory[i, (n * 2) - 2] = ConsoleColor.Black;
                znaki[i + 1, (n * 2) - 2] = Convert.ToString(n); kolory[i + 1, (n * 2) - 2] = drugi;
            }
            for (int j = 0; j < (n * 2) - 2; j += 2)
            {
                znaki[(n * 2) - 2, j + 1] = Convert.ToString(n); kolory[(n * 2) - 2, j + 1] = pierwszy;
                znaki[(n * 2) - 2, j] = " "; kolory[(n * 2) - 2, j] = ConsoleColor.Black;
            }
            znaki[(n * 2) - 2, (n * 2) - 2] = " "; kolory[(n * 2) - 2, (n * 2) - 2] = ConsoleColor.Black;
        }
        ConsoleColor WybierzKolor()
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

        public void ZaznaczWygrana(bool gracz1, int[] ruch)
        {
            int x, y;
            if (gracz1) { x = (ruch[1] - 1) * 2; y = ((ruch[0] - 1) * 2) + 1; }
            else { x = ((ruch[0] - 1) * 2) + 1; y = ((ruch[1] - 1) * 2); }

            Queue<int> koordynaty = new Queue<int>();
            koordynaty.Enqueue(x); koordynaty.Enqueue(y);
            kolory[x, y] = ConsoleColor.White;

            while (koordynaty.Count != 0)
            {
                x = koordynaty.Peek(); koordynaty.Dequeue();
                y = koordynaty.Peek(); koordynaty.Dequeue();

                if (y - 1 >= 0 && znaki[x, y - 1] == "-" && kolory[x, y - 1] != kolory[x, y])
                {
                    kolory[x, y - 1] = ConsoleColor.White;
                    kolory[x, y - 2] = ConsoleColor.White;
                    koordynaty.Enqueue(x); koordynaty.Enqueue(y - 2);
                }
                if (x + 1 <= (n * 2) - 2 && znaki[x + 1, y] == "|" && kolory[x + 1, y] != kolory[x, y])
                {
                    kolory[x + 1, y] = ConsoleColor.White;
                    kolory[x + 2, y] = ConsoleColor.White;
                    koordynaty.Enqueue(x + 2); koordynaty.Enqueue(y);
                }
                if (y + 1 <= (n * 2) - 2 && znaki[x, y + 1] == "-" && kolory[x, y + 1] != kolory[x, y])
                {
                    kolory[x, y + 1] = ConsoleColor.White;
                    kolory[x, y + 2] = ConsoleColor.White;
                    koordynaty.Enqueue(x); koordynaty.Enqueue(y + 2);
                }
                if (x - 1 >= 0 && znaki[x - 1, y] == "|" && kolory[x - 1, y] != kolory[x, y])
                {
                    kolory[x - 1, y] = ConsoleColor.White;
                    kolory[x - 2, y] = ConsoleColor.White;
                    koordynaty.Enqueue(x - 2); koordynaty.Enqueue(y);
                }
            }
        }
        public void Wyswietl()
        {
            Console.Clear();
            Console.Write("    ");
            Console.ForegroundColor = pierwszy;
            for (int j = 0; j < n - 1; j++) Console.Write(j + 1 + " ");
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < (n * 2) - 1; i++)
            {
                if (i % 2 != 0)
                {
                    Console.ForegroundColor = drugi;
                    Console.Write((i / 2) + 1);
                }
                else Console.Write(" ");

                Console.Write("  ");
                for (int j = 0; j < (n * 2) - 1; j++)
                {
                    Console.ForegroundColor = kolory[i, j];
                    Console.Write(znaki[i, j]);
                }
                Console.WriteLine();
            }
        }
        public void PoprawWyswietlanie(bool gracz1, int[] ruch)
        {
            if (gracz1)
            {
                if (ruch[0] == ruch[2]) { znaki[((ruch[1] - 1) * 2) + 1, ((ruch[0] - 1) * 2) + 1] = "|"; kolory[((ruch[1] - 1) * 2) + 1, ((ruch[0] - 1) * 2) + 1] = pierwszy; }  
                else { znaki[(ruch[1] - 1) * 2, ruch[0] * 2] = "-"; kolory[(ruch[1] - 1) * 2, ruch[0] * 2] = pierwszy; }
            }
            else
            {
                if (ruch[0] == ruch[2]) { znaki[((ruch[0] - 1) * 2) + 1, ((ruch[1] - 1) * 2) + 1] = "-"; kolory[((ruch[0] - 1) * 2) + 1, ((ruch[1] - 1) * 2) + 1] = drugi; }
                else { znaki[ruch[0] * 2, ((ruch[1] - 1) * 2)] = "|"; kolory[ruch[0] * 2, ((ruch[1] - 1) * 2)] = drugi; }
            }
        }
    }
}
