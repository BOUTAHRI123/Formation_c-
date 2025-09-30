using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie2
{
    public static class Morpion
    {
        /*typeGrille grille : je vais utiliser un tableau de type char et de taille 3*3 */
        public static void DisplayMorpion(char[,] tab)
        {
            int i;
            int j;
            Console.WriteLine("Affichage grille de Morpion :");
            for (i=0;i < tab.GetLength(0);i++)
            {
                for (j = 0; j < tab.GetLength(1); j++)
                {
                    Console.Write($"{tab[i,j]} ");
                }
                Console.WriteLine();
            }
            return;
        }

        public static int CheckMorpion(/*typeGrille grille */)
        {
            //TODO
            return -1;
        }
    }
}
