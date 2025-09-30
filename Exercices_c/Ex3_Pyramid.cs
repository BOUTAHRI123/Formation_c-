using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public static class Pyramid
    {
        public static void PyramidConstruction(int n, bool isSmooth)
        {
            int i;
            int j;
            int width = 2 * n - 1;
            for (i = 0; i < n; i++)
            {
                for(j=0; j< width; j++)
                {
                    int gauche = n - 1 - i;
                    int droite = n - 1 + i;
                    if(j<=droite && j >= gauche)
                    {
                        char c = isSmooth ? '+' :( (i % 2 == 0) ? '+' : '-');
                        Console.Write(c);

                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
