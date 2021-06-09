using System;
using System.Collections.Generic;

namespace Jipp_4_Projekt_Uproszczony_v2_2
{
    abstract class Gracz
    {
        protected int n;
        protected PunktAbstrakcyjny[,] punkty_moje;
        protected PunktAbstrakcyjny[,] punkty_wrog;
        protected Blokada blokada = new Blokada();
        public Gracz(int n, ref PunktAbstrakcyjny[,] m, ref PunktAbstrakcyjny[,] w) { this.n = n; punkty_moje = m; punkty_wrog = w; }
        public abstract int[] Ruch();
        protected void Polacz(int[] vs)
        {
            if (vs[0] > vs[2] || vs[1] > vs[3])
            {
                int x = vs[0];
                vs[0] = vs[2];
                vs[2] = x;
                x = vs[1];
                vs[1] = vs[3];
                vs[3] = x;
            }

            if (vs[0] != vs[2])
            {
                ((Punkt)punkty_moje[vs[0], vs[1]]).Polacz(2);
                ((Punkt)punkty_wrog[vs[3] - 1, vs[2]]).Zablokuj(blokada, 2);
                ((Punkt)punkty_moje[vs[2], vs[3]]).Polacz(4);
                ((Punkt)punkty_wrog[vs[3], vs[2]]).Zablokuj(blokada, 4);
            }
            else
            {
                ((Punkt)punkty_moje[vs[0], vs[1]]).Polacz(3);
                ((Punkt)punkty_wrog[vs[1], vs[0]]).Zablokuj(blokada, 3);
                ((Punkt)punkty_moje[vs[2], vs[3]]).Polacz(1);
                ((Punkt)punkty_wrog[vs[1], vs[0] + 1]).Zablokuj(blokada, 1);
            }
        }
    }
    class Czlowiek : Gracz
    {
        public Czlowiek(int n, ref PunktAbstrakcyjny[,] p, ref PunktAbstrakcyjny[,] w) : base(n, ref p, ref w) { }
        public override int[] Ruch()
        {
            int[] vs = new int[4];
            while (true)
            {
                Console.WriteLine("Podaj wiersz/kolumnę pierwszej koordynaty: ");
                vs[0] = SprawdzWejscie();
                if (vs[0] == -2147483648) break;
                Console.WriteLine("Podaj numer pierwszej koordynaty: ");
                vs[1] = SprawdzWejscie();
                if (vs[1] == -2147483648) break;
                Console.WriteLine("Podaj wiersz/kolumnę drugiej koordynaty: ");
                vs[2] = SprawdzWejscie();
                if (vs[2] == -2147483648) break;
                Console.WriteLine("Podaj numer drugiej koordynaty: ");
                vs[3] = SprawdzWejscie();
                if (vs[3] == -2147483648) break;
                if (SprawdzRuch(vs)) break;
            }
            Polacz(vs);
            return vs;
        }
        bool SprawdzRuch(int[] vs)
        {
            for (int i = 0; i < 4; i++)
                if (vs[i] > n || vs[i] < 1)
                {
                    Console.WriteLine("Dane spoza zakresu");
                    return false;
                }

            if ((vs[1] == 1 && vs[3] == 1) || (vs[1] == n && vs[3] == n))
            {
                Console.WriteLine("Nie możesz łączyć punktów na ścianach");
                return false;
            }

            int a = Math.Abs(vs[0] - vs[2]), b = Math.Abs(vs[1] - vs[3]);
            if (a > 1 || b > 1) { Console.WriteLine("Punkty zbyt daleko od siebie"); return false; }
            if (a == 0 && b == 0) { Console.WriteLine("To jest ten sam punkt"); return false; }
            if (a == 1 && b == 1) { Console.WriteLine("Nie możesz łączyć na skos"); return false; }

            if (vs[0] > vs[2] || vs[1] > vs[3])
            {
                int x = vs[0];
                vs[0] = vs[2];
                vs[2] = x;
                x = vs[1];
                vs[1] = vs[3];
                vs[3] = x;
            }


            if (a == 1 && punkty_moje[vs[0], vs[1]].PokazPolaczenie(2) == 1)
                return true;
            else if (b == 1 && punkty_moje[vs[0], vs[1]].PokazPolaczenie(3) == 1)
                return true;
            
            Console.WriteLine("Połączenie już zajęte");
            return false;
        }
        int SprawdzWejscie()
        {
            int liczba;
            string wejscie;
            while (true)
            {
                wejscie = Console.ReadLine();
                if(wejscie == "poddaję się") return -2147483648;
                if (int.TryParse(wejscie, out liczba)) break;
                Console.WriteLine("Niepoprawne wejście, spróbuj ponownie.");
            }
            return liczba;
        }
    }
    abstract class Bot : Gracz
    {
        protected bool[,] droga_moja;
        protected bool[,] droga_wroga;
        protected Tuple<int, int> start_moj;
        protected Tuple<int, int> start_wrog;
        protected int[] reakcja;
        public Bot(int n, ref PunktAbstrakcyjny[,] p, ref PunktAbstrakcyjny[,] w) : base(n, ref p, ref w) { }
        public override abstract int[] Ruch();
        protected void PoprawTablice(ref PunktAbstrakcyjny[,] punkty)
        {
            Queue<int> priorytet = new Queue<int>();
            Queue<int> kolejka = new Queue<int>();

            for (int i = 1; i < n; i++)
            {
                priorytet.Enqueue(i);
                priorytet.Enqueue(1);
                punkty[i, 1].Odleglosc = 0;
                for (int j = 2; j <= n; j++)
                {
                    punkty[i, j].Odleglosc = 2147483647;
                }
            }

            int x, y;
            int[] roznice;
            while (priorytet.Count != 0 || kolejka.Count != 0)
            {
                if (priorytet.Count != 0)
                {
                    x = priorytet.Peek(); priorytet.Dequeue();
                    y = priorytet.Peek(); priorytet.Dequeue();
                }
                else
                {
                    x = kolejka.Peek(); kolejka.Dequeue();
                    y = kolejka.Peek(); kolejka.Dequeue();
                }
                roznice = punkty[x, y].PokazRoznice();
                if (roznice[0] == 3)
                {
                    if (punkty[x, y].PokazPolaczenie(1) == 0) { punkty[x, y - 1].Odleglosc = punkty[x, y].Odleglosc; priorytet.Enqueue(x); priorytet.Enqueue(y - 1); }
                    else { punkty[x, y - 1].Odleglosc = punkty[x, y].Odleglosc + 1; kolejka.Enqueue(x); kolejka.Enqueue(y - 1); }
                }
                if (roznice[1] == 3)
                {
                    if (punkty[x, y].PokazPolaczenie(2) == 0) { punkty[x + 1, y].Odleglosc = punkty[x, y].Odleglosc; priorytet.Enqueue(x + 1); priorytet.Enqueue(y); }
                    else { punkty[x + 1, y].Odleglosc = punkty[x, y].Odleglosc + 1; kolejka.Enqueue(x + 1); kolejka.Enqueue(y); }
                }
                if (roznice[2] == 3)
                {
                    if (punkty[x, y].PokazPolaczenie(3) == 0) { punkty[x, y + 1].Odleglosc = punkty[x, y].Odleglosc; priorytet.Enqueue(x); priorytet.Enqueue(y + 1); }
                    else { punkty[x, y + 1].Odleglosc = punkty[x, y].Odleglosc + 1; kolejka.Enqueue(x); kolejka.Enqueue(y + 1); }
                }
                if (roznice[3] == 3)
                {
                    if (punkty[x, y].PokazPolaczenie(4) == 0) { punkty[x - 1, y].Odleglosc = punkty[x, y].Odleglosc; priorytet.Enqueue(x - 1); priorytet.Enqueue(y); }
                    else { punkty[x - 1, y].Odleglosc = punkty[x, y].Odleglosc + 1; kolejka.Enqueue(x - 1); kolejka.Enqueue(y); }
                }
            }
        }
        protected void SzukajDrogi(ref bool[,] droga, ref PunktAbstrakcyjny[,] punkty, ref Tuple<int, int> kontynuuj)
        {
            int min = 2147483647, x = 0, y = n;
            Random r = new Random();

            for (int i = 1; i < n; i++)
            {
                if (punkty[i, n].Odleglosc < min || (punkty[i, n].Odleglosc == min && r.NextDouble() < 0.5f))
                {
                    min = punkty[i, n].Odleglosc;
                    x = i;
                }
            }

            int dlugosc = punkty[x, y].Odleglosc;
            droga = new bool[n + 1, n + 2];
            droga[x, y] = true;

            PunktAbstrakcyjny aktualny;
            List<int> droga_polaczonych;
            int[] roznice;
            while (dlugosc > 0)
            {
                aktualny = punkty[x, y];
                roznice = aktualny.PokazRoznice();
                if (roznice[4] == 1)
                {
                    if (roznice[0] == -1) y--;
                    else if (roznice[1] == -1) x++;
                    else if (roznice[2] == -1) y++;
                    else if (roznice[3] == -1) x--;

                    droga[x, y] = true;
                    dlugosc--;
                }
                else
                {
                    droga_polaczonych = SzukajDrogiPolaczonych(x, y, ref punkty, ref droga);
                    for (int i = 0; i < droga_polaczonych.Count; i += 2)
                        droga[droga_polaczonych[i], droga_polaczonych[i + 1]] = true;
                    x = droga_polaczonych[droga_polaczonych.Count - 2];
                    y = droga_polaczonych[droga_polaczonych.Count - 1];
                }
            }

            kontynuuj = new Tuple<int, int>(x, y);
        }
        protected List<int> SzukajDrogiPolaczonych(int x, int y, ref PunktAbstrakcyjny[,] punkty, ref bool[,] drogaaaa)
        {
            List<int> droga = new List<int>();
            bool[,] odwiedzone = new bool[n + 1, n + 2];
            droga.Add(x); droga.Add(y);
            PunktAbstrakcyjny aktualny;
            int[] roznice;
            int poprzednik = 0;
            while (true)
            {
                aktualny = punkty[x, y];
                roznice = aktualny.PokazRoznice();
                if (roznice[4] == 1) return droga;

                if (roznice[0] == 0 && !odwiedzone[x, y - 1] && poprzednik != 1) { y--; droga.Add(x); droga.Add(y); poprzednik = 3; }
                else if (roznice[1] == 0 && !odwiedzone[x + 1, y] && poprzednik != 2) { x++; droga.Add(x); droga.Add(y); poprzednik = 4; }
                else if (roznice[2] == 0 && !odwiedzone[x, y + 1] && poprzednik != 3) { y++; droga.Add(x); droga.Add(y); poprzednik = 1; }
                else if (roznice[3] == 0 && !odwiedzone[x - 1, y] && poprzednik != 4) { x--; droga.Add(x); droga.Add(y); poprzednik = 2; }
                else
                {
                    odwiedzone[x, y] = true;
                    droga.RemoveAt(droga.Count - 1);
                    droga.RemoveAt(droga.Count - 1);
                    x = droga[droga.Count - 2];
                    y = droga[droga.Count - 1];
                    poprzednik = 0;
                }
            }
        }
    }
    class BotBloker : Bot
    {
        public BotBloker(int n, ref PunktAbstrakcyjny[,] p, ref  PunktAbstrakcyjny[,] w) : base(n, ref p, ref w) { }
        public override int[] Ruch()
        {
            SzukajZagrozen();
            Polacz(reakcja);
            return reakcja;
        }
        protected void SzukajZagrozen()
        {
            PoprawTablice(ref punkty_moje);
            PoprawTablice(ref punkty_wrog);
            SzukajDrogi(ref droga_moja, ref punkty_moje, ref start_moj);
            SzukajDrogi(ref droga_wroga, ref punkty_wrog, ref start_wrog);

            int x = start_moj.Item1, y = start_moj.Item2;
            bool[,] odwiedzone = new bool[n + 1, n + 2];
            reakcja = null;
            List<int[]> propozycje = new List<int[]>();
            while (true)
            {
                if (!odwiedzone[x, y + 1] && droga_moja[x, y + 1] && punkty_moje[x, y].PokazPolaczenie(3) != -1)
                {
                    if (droga_wroga[y, x] && droga_wroga[y, x + 1] && punkty_moje[x, y].PokazPolaczenie(3) == 1)
                        propozycje.Add(new int[4] { x, y, x, y + 1 });

                    odwiedzone[x, y] = true;
                    y++;
                }
                else if (!odwiedzone[x + 1, y] && droga_moja[x + 1, y] && punkty_moje[x, y].PokazPolaczenie(2) != -1)
                {
                    if (droga_wroga[y - 1, x + 1] && droga_wroga[y, x + 1] && punkty_moje[x, y].PokazPolaczenie(2) == 1)
                        propozycje.Add(new int[4] { x, y, x + 1, y });

                    odwiedzone[x, y] = true;
                    x++;
                }
                else if (!odwiedzone[x - 1, y] && droga_moja[x - 1, y] && punkty_moje[x, y].PokazPolaczenie(4) != -1)
                {
                    if (droga_wroga[y - 1, x] && droga_wroga[y, x] && punkty_moje[x, y].PokazPolaczenie(4) == 1)
                        propozycje.Add(new int[4] { x, y, x - 1, y });

                    odwiedzone[x, y] = true;
                    x--;
                }
                else if (!odwiedzone[x, y - 1] && droga_moja[x, y - 1] && punkty_moje[x, y].PokazPolaczenie(1) != -1)
                {
                    if (droga_wroga[y - 1, x] && droga_wroga[y - 1, x + 1] && punkty_moje[x, y].PokazPolaczenie(1) == 1)
                        propozycje.Add(new int[4] { x, y, x, y - 1 });

                    odwiedzone[x, y] = true;
                    y--;
                }
                else break;
            }
            //reakcja = propozycje[(new Random()).Next(0, propozycje.Count - 1)];

            SzukajNajlepszegoPolaczenia(propozycje);
        }
        void SzukajNajlepszegoPolaczenia(List<int[]> propozycje)
        {
            int minn, najlepszy = 0;
            Random r = new Random();
            foreach(int[] opcja in propozycje)
            {
                minn = int.MaxValue;
                if (opcja[0] > opcja[2] || opcja[1] > opcja[3])
                {
                    int x = opcja[0];
                    opcja[0] = opcja[2];
                    opcja[2] = x;
                    x = opcja[1];
                    opcja[1] = opcja[3];
                    opcja[3] = x;
                }

                if (opcja[0] != opcja[2])
                {
                    ((Punkt)punkty_wrog[opcja[3] - 1, opcja[2]]).ZablokujTymczasowo(2);
                    ((Punkt)punkty_wrog[opcja[3], opcja[2]]).ZablokujTymczasowo(4);
                }
                else
                {
                    ((Punkt)punkty_wrog[opcja[1], opcja[0]]).ZablokujTymczasowo(3);
                    ((Punkt)punkty_wrog[opcja[1], opcja[0] + 1]).ZablokujTymczasowo(1);
                }
                PoprawTablice(ref punkty_wrog);
                for (int i = 1; i < n; i++)
                {
                    minn = Math.Min(minn, punkty_wrog[i, n].Odleglosc);
                }
                if (opcja[0] != opcja[2])
                {
                    ((Punkt)punkty_wrog[opcja[3] - 1, opcja[2]]).Odblokuj(2);
                    ((Punkt)punkty_wrog[opcja[3], opcja[2]]).Odblokuj(4);
                }
                else
                {
                    ((Punkt)punkty_wrog[opcja[1], opcja[0]]).Odblokuj(3);
                    ((Punkt)punkty_wrog[opcja[1], opcja[0] + 1]).Odblokuj(1);
                }

                if (minn > najlepszy || (minn == najlepszy && r.NextDouble() < 0.5f) )
                {
                    reakcja = opcja;
                    najlepszy = minn;
                }
            }
        }
    }
}

