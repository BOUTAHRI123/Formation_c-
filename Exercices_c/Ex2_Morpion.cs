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

        public static int CheckMorpion(char[,] tab)
        {
            int i;
            bool valide = true;
            int joueur=0;
            for( i=0; i<tab.GetLength(0); i++)
            {
                if(tab[i,0]!= ' ' && tab[i,0]==tab[i,1] && tab[i,1]== tab[i, 2])
                {
                    joueur = (tab[i, 0] == 'X' ?  1: 2);
                   
                }
                else if(tab[0, i] != ' ' && tab[0, i] == tab[1, i] && tab[1, i] == tab[2, i]){
                    joueur = (tab[0, i] == 'X' ? 1 : 2);
                    
                }
                else if (tab[0, 0] != ' ' && tab[0, 0] == tab[1, 1] && tab[1, 1] == tab[2, 2]){
                    joueur = (tab[0, 0] == 'X' ? 1 : 2);
                    
                }
                else if (tab[0, 2] != ' ' && tab[0, 2] == tab[1, 1] && tab[1, 1] == tab[2, 0]){
                    joueur = (tab[0, 2] == 'X' ? 1 : 2);
                }

            }
            Console.WriteLine($"La partie est  terminée , le joueur qui a gagné est {joueur}");
            foreach (char c in tab)
            {
                if(c == ' ')
                {
                    valide = false;
                    Console.WriteLine("La partie n'est pas terminée");
                }
            }


            return joueur;
        }
    }
}
