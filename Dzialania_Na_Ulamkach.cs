using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dzialania_Na_Ulamkach
{
    class Ulamek
    {
        public int licz, mian;
        public Ulamek(string wejscie)
        {
            licz = Convert.ToInt32(wejscie.Split('/')[0]);
            mian = Convert.ToInt32(wejscie.Split('/')[1]);
            Skroc();
        }
        public Ulamek(int l, int m)
        {
            licz = l;
            mian = m;
            Skroc();
        }
        void Skroc()
        {
            int dzielnik = Dzialania.NWD(licz, mian);
            licz /= dzielnik;
            mian /= dzielnik;
        }
    }
    static class Dzialania
    {
        static public Ulamek Dodawanie(Ulamek u1, Ulamek u2)
        {
            if (u1.mian == u2.mian) return new Ulamek(u1.licz + u2.licz, u1.mian);
            else return new Ulamek(u1.licz * u2.mian + u2.licz * u1.mian, u1.mian * u2.mian);
        }
        static public Ulamek Odejmowanie(Ulamek u1, Ulamek u2)
        {
            if (u1.mian == u2.mian) return new Ulamek(u1.licz - u2.licz, u1.mian);
            else return new Ulamek(u1.licz * u2.mian - u2.licz * u1.mian, u1.mian * u2.mian);
        }
        static public Ulamek Mnozenie(Ulamek u1, Ulamek u2)
        {
            return new Ulamek(u1.licz * u2.licz, u1.mian * u2.mian);
        }
        static public Ulamek Dzielenie(Ulamek u1, Ulamek u2)
        {
            return new Ulamek(u1.licz * u2.mian, u1.mian * u2.licz);
        }
        static public int NWD(int x, int y)
        {
            while (x != y)
            {
                if (x > y) x -= y;
                else y -= x;
            }
            return x;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Ulamek u = ObliczUlamek(Console.ReadLine());
            Console.WriteLine(u.licz);
            Console.WriteLine(u.mian);
            Console.ReadKey();
        }
        public static Ulamek ObliczUlamek(string wejscie)
        {
            string pierwszy = "", drugi = "";
            int poziom = 0, aktualny = 0, temp = 1;
            char znak = ' ';

            for (int i = 1; i < wejscie.Length - 1; i++)
            {
                if (wejscie[i] == '(') { poziom++; aktualny++; }
                else if (wejscie[i] == ')') aktualny--;

                if (aktualny == 0)
                {
                    if(pierwszy == "") { pierwszy = wejscie.Substring(temp, i - temp + 1); znak = wejscie[i + 1]; i += 2; temp = i; }
                    else drugi = wejscie.Substring(temp, i - temp + 2);
                }
            }
            

            if(poziom != 0)
            {
                switch (znak)
                {
                    case '+': return Dzialania.Dodawanie(ObliczUlamek(pierwszy), ObliczUlamek(drugi));
                    case '-': return Dzialania.Odejmowanie(ObliczUlamek(pierwszy), ObliczUlamek(drugi));
                    case '*': return Dzialania.Mnozenie(ObliczUlamek(pierwszy), ObliczUlamek(drugi));
                    default: return Dzialania.Dzielenie(ObliczUlamek(pierwszy), ObliczUlamek(drugi));
                }
            }
            else
            {
                return new Ulamek(wejscie.Substring(1, wejscie.Length - 2));
            }
        }

    }
}
