using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public static class Factorial
    {
        public static int Factorial_(int n)
        {
            int i;
            int fact =1;
            for (i = 0; i < n; i++)
            {

                fact *=(n - i);

            }
            Console.WriteLine($"la factorielle de {n} est : {fact}");
            return fact;

        }

        public static int FactorialRecursive(int n)
        {
            int factoreil = 0;
            if (n == 0)
            {
                factoreil = 1;

            }
            else
            {
                factoreil = n * Factorial_(n - 1);
            }
            Console.WriteLine($"la factorielle de {n} est : {factoreil}");
            return factoreil;
        }
    }
}
