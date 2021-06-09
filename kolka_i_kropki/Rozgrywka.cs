using System;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{

    class Rozgrywka
    {
        Gracz pierwszy;
        Gracz drugi;
        int n;
        PunktAbstrakcyjny[,] kropki;
        PunktAbstrakcyjny[,] kolka;
        Blokada blokada = new Blokada();
        Wyswietlanie wyswietl;
        public void PrzygotujGre()
        {
            do
            {
                Console.WriteLine("Podaj rozmiar planszy (nie mniejszy niż 3): ");
                n = Convert.ToInt32(Console.ReadLine());
            } while (n < 3);
            PrzygotujTablice();
            Console.WriteLine("wybierz pierwszego gracza:");
            Console.WriteLine("1 Człowiek");
            Console.WriteLine("2 Bot bloker");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1: pierwszy = new Czlowiek(n, ref kropki, ref kolka); break;
                case 2: pierwszy = new BotBloker(n, ref kropki, ref kolka); break;
                default: pierwszy = new Czlowiek(n, ref kropki, ref kolka); break;
            }
            ConsoleColor p = WybierzKolor();

            Console.WriteLine("wybierz drugiego gracza:");
            Console.WriteLine("1 Człowiek");
            Console.WriteLine("2 Bot bloker");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1: drugi = new Czlowiek(n, ref kolka, ref kropki); break;
                case 2: drugi = new BotBloker(n, ref kolka, ref kropki); break;
                default: drugi = new Czlowiek(n, ref kolka, ref kropki); break;
            }
            ConsoleColor d;
            while (true)
            {
                d = WybierzKolor();
                if (d == p) Console.WriteLine("Wybrany kolor jest już zajęty");
                else break;
            }
            wyswietl = new Wyswietlanie(n, p, d);
            if (pierwszy is Bot && drugi is Bot) WalkaBotow();
            else Graj();
        }
        void PrzygotujTablice()
        {
            kropki = new PunktAbstrakcyjny[n + 1, n + 2];
            kolka = new PunktAbstrakcyjny[n + 1, n + 2];

            for (int i = 0; i < n + 1; i++)
            {
                kropki[i, 0] = blokada;
                kolka[i, 0] = blokada;

                kropki[0, i] = blokada;
                kolka[0, i] = blokada;

                kropki[n, i] = blokada;
                kolka[n, i] = blokada;

                kropki[i, n + 1] = blokada;
                kolka[i, n + 1] = blokada;
            }

            for (int i = 1; i < n; i++)
            {
                kropki[i, 1] = new Punkt(0, 1);
                kolka[i, 1] = new Punkt(0, 1);
                kropki[i, n] = new Punkt(n - 1, 2);
                kolka[i, n] = new Punkt(n - 1, 2);
                for (int j = 2; j < n; j++)
                {
                    kropki[i, j] = new Punkt(j - 1);
                    kolka[i, j] = new Punkt(j - 1);
                }
            }
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    kropki[i, j].PrzypiszSasiadow(kropki[i, j - 1], kropki[i + 1, j], kropki[i, j + 1], kropki[i - 1, j]);
                    kolka[i, j].PrzypiszSasiadow(kolka[i, j - 1], kolka[i + 1, j], kolka[i, j + 1], kolka[i - 1, j]);
                }
            }
        }


        public void Graj()
        {
            if(pierwszy == null)
            {
                Console.WriteLine("Nie przygotowano rozgrywki!");
                Console.WriteLine("Aby to zrobić użyj metody PrzygotujGre");
                Console.ReadKey();
                return;
            }
            bool ruch_gracza1 = true;
            bool kontynuuj = true;
            int[] ruch;
            wyswietl.Wyswietl();
            while (kontynuuj)
            {
                if (ruch_gracza1)
                {
                    Console.ForegroundColor = wyswietl.Pierwszy;
                    Console.WriteLine("Ruch gracza 1");
                    ruch = pierwszy.Ruch();
                    wyswietl.PoprawWyswietlanie(true, ruch);
                    wyswietl.Wyswietl();
                    if (SprawdzWynik(ruch, true)) break;
                }
                else
                {
                    Console.ForegroundColor = wyswietl.Drugi;
                    Console.WriteLine("Ruch gracza 2");
                    ruch = drugi.Ruch();
                    wyswietl.PoprawWyswietlanie(false, ruch);
                    wyswietl.Wyswietl();
                    if (SprawdzWynik(ruch, false)) break;
                }
                ruch_gracza1 = !ruch_gracza1;
            }
            Console.WriteLine("(Wciśnij dowolny przycisk aby zakończyć)");
            Console.ReadKey();
        }
        bool SprawdzWynik(int[] vs, bool gracz)
        {
            for (int i = 0; i < 4; i++)
            {
                if(vs[i] == -2147483648)
                {
                    if (gracz) Console.WriteLine("Gracz pierwszy się poddaje - wygrywa Gacz Drugi!");
                    else Console.WriteLine("Gracz Drugi się poddaje - wygrywa Gacz Pierwszy!");
                    return true;
                }
            }

            if (gracz)
            {
                if(kropki[vs[0], vs[1]].Koniec == 3 || kropki[vs[2], vs[3]].Koniec == 3)
                {
                    wyswietl.ZaznaczWygrana(gracz, vs);
                    wyswietl.Wyswietl();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Wygrywa Gracz Pierwszy!");
                    return true;
                }
            }
            else
            {
                if (kolka[vs[0], vs[1]].Koniec == 3 || kolka[vs[2], vs[3]].Koniec == 3)
                {
                    wyswietl.ZaznaczWygrana(gracz, vs);
                    wyswietl.Wyswietl();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Wygrywa Gracz Drugi!");
                    return true;
                }
            }
            return false;
        }
        public void WalkaBotow()
        {
            if (pierwszy == null)
            {
                Console.WriteLine("Nie przygotowano rozgrywki!");
                Console.WriteLine("Aby to zrobić użyj metody PrzygotujGre");
                Console.ReadKey();
                return;
            }
            bool ruch_gracza1 = true;
            bool kontynuuj = true;
            int[] ruch = new int[4];
            var licznik = System.Diagnostics.Stopwatch.StartNew();
            while (kontynuuj)
            {
                if (ruch_gracza1)
                {
                    ruch = pierwszy.Ruch();
                    wyswietl.PoprawWyswietlanie(true, ruch);
                    if (SprawdzWynik(ruch, true)) break;
                }
                else
                {
                    ruch = drugi.Ruch();
                    wyswietl.PoprawWyswietlanie(false, ruch);
                    if (SprawdzWynik(ruch, false)) break;
                }
                ruch_gracza1 = !ruch_gracza1;
            }
            licznik.Stop();
            if (ruch_gracza1) SprawdzWynik(ruch, true);
            else SprawdzWynik(ruch, false);
            var elapsedMs = licznik.ElapsedMilliseconds;
            Console.WriteLine("upłynęło: " + elapsedMs/1000 + "s");
            Console.WriteLine("(Wciśnij dowolny przycisk aby zakończyć)");
            Console.ReadKey();
        }


        public void Reset()
        {
            PrzygotujTablice();
            wyswietl = new Wyswietlanie(n, wyswietl.Pierwszy, wyswietl.Drugi);
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
    }
}
