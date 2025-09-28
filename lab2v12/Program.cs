using System;
using System.Collections.Generic;

namespace lab2v12
{
    public class PolynomialSet
    {
        // приватне поле – список багаточленів
        private List<int[]> polynomials = new List<int[]>();

        // властивість для кількості
        public int Count => polynomials.Count;

        // індексатор по індексу
        public int[] this[int index]
        {
            get
            {
                if (index < 0 || index >= polynomials.Count)
                    throw new IndexOutOfRangeException();
                return polynomials[index];
            }
        }

        // оператор + для додавання нового полінома
        public static PolynomialSet operator +(PolynomialSet set, int[] coeffs)
        {
            set.polynomials.Add(coeffs);
            return set;
        }

        // метод для виводу усіх поліномів
        public void PrintAll()
        {
            for (int k = 0; k < polynomials.Count; k++)
            {
                var coeffs = polynomials[k];
                Console.Write($"P{k + 1}(x) = ");

                for (int i = 0; i < coeffs.Length; i++)
                {
                    int power = coeffs.Length - i - 1;
                    int c = coeffs[i];

                    if (c == 0) continue;

                    if (i > 0 && c > 0) Console.Write("+");

                    if (power == 0)
                        Console.Write($"{c}");
                    else if (power == 1)
                        Console.Write($"{c}x");
                    else
                        Console.Write($"{c}x^{power}");
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PolynomialSet set = new PolynomialSet();

            // додаємо поліноми через оператор +
            set = set + new int[] { 1, -3, 2 }; // x^2 - 3x + 2
            set = set + new int[] { 2, 0, 5 };  // 2x^2 + 5
            set = set + new int[] { 1, 1 };     // x + 1

            Console.WriteLine("=== Усі поліноми ===");
            set.PrintAll();

            Console.WriteLine("\nДоступ через індекс [1]:");
            var p = set[1];
            foreach (var c in p) Console.Write(c + " ");
        }
    }
}