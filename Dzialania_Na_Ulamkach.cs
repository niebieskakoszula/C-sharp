using System;

namespace Operations_On_Fractions
{
    class Fraction
    {
        public int counter, denominator;
        public Fraction(string input)
        {
            counter = Convert.ToInt32(input.Split('/')[0]);
            denominator = Convert.ToInt32(input.Split('/')[1]);
            Shortening();
        }
        public Fraction(int c, int d)
        {
            counter = c;
            denominator = d;
            Shortening();
        }
        void Shortening()
        {
            int factor = OperationsOnFractions.Greatest_Common_Factor(counter, denominator);
            counter /= factor;
            denominator /= factor;
        }
    }
    static class OperationsOnFractions
    {
        static public Fraction Addition(Fraction first, Fraction second)
        {
            if (first.denominator == second.denominator) return new Fraction(first.counter + second.counter, first.denominator);
            else return new Fraction(first.counter * second.denominator + second.counter * first.denominator, first.denominator * second.denominator);
        }
        static public Fraction Substraction(Fraction first, Fraction second)
        {
            if (first.denominator == second.denominator) return new Fraction(first.counter - second.counter, first.denominator);
            else return new Fraction(first.counter * second.denominator - second.counter * first.denominator, first.denominator * second.denominator);
        }
        static public Fraction Multiplication(Fraction first, Fraction second)
        {
            return new Fraction(first.counter * second.counter, first.denominator * second.denominator);
        }
        static public Fraction Division(Fraction first, Fraction second)
        {
            return new Fraction(first.counter * second.denominator, first.denominator * second.counter);
        }
        static public int Greatest_Common_Factor(int x, int y)
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
            Fraction fraction = Calculate_Fraction(Console.ReadLine());
            Console.WriteLine(fraction.counter);
            Console.WriteLine(fraction.denominator);
            Console.ReadKey();
        }
        public static Fraction Calculate_Fraction(string input)
        {
            string first_sub = "", second_sub = "";
            int level = 0, current = 0, start = 1;
            char operation = ' ';

            for (int i = 1; i < input.Length - 1; i++)
            {
                if (input[i] == '(') { level++; current++; }
                else if (input[i] == ')') current--;

                if (current == 0)
                {
                    if (first_sub == "") { first_sub = input.Substring(start, i - start + 1); operation = input[i + 1]; i += 2; start = i; }
                    else second_sub = input.Substring(start, i - start + 2);
                }
            }


            if (level != 0)
            {
                switch (operation)
                {
                    case '+': return OperationsOnFractions.Addition(Calculate_Fraction(first_sub), Calculate_Fraction(second_sub));
                    case '-': return OperationsOnFractions.Substraction(Calculate_Fraction(first_sub), Calculate_Fraction(second_sub));
                    case '*': return OperationsOnFractions.Multiplication(Calculate_Fraction(first_sub), Calculate_Fraction(second_sub));
                    default: return OperationsOnFractions.Division(Calculate_Fraction(first_sub), Calculate_Fraction(second_sub));
                }
            }
            else
            {
                return new Fraction(input.Substring(1, input.Length - 2));
            }
        }

    }
}
