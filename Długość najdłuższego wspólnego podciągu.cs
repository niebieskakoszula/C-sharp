//https://pl.spoj.com/problems/LENLCS/

using System;

namespace LENLCS___Długość_najdłuższego_wspólnego_podciągu
{
    class Program
    {
        static void Main(string[] args)
        {
            int D, n, m;
            string u, v;
            D = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < D; i++)
            {
                n = Convert.ToInt32(Console.ReadLine());
                u = Console.ReadLine();
                m = Convert.ToInt32(Console.ReadLine());
                v = Console.ReadLine();
                Console.WriteLine(Najdluzszy(n, u, m, v));
            }
        }
        static int Najdluzszy(int n, string u, int m, string v)
        {
            int[,] vs = new int[n+1, m+1];
            for (int i = 0; i < n; i++)
                vs[i, 0] = 0;
            for (int i = 0; i < m; i++)
                vs[0, i] = 0;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    if (u[i - 1] == v[j - 1])
                        vs[i, j] = vs[i - 1, j - 1] + 1;
                    else
                        vs[i, j] = Math.Max(vs[i - 1, j], vs[i, j - 1]);
                }
            }
            return vs[n,m];
        }
    }
}
