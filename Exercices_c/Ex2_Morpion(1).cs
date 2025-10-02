using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie4
{
    public static class Morpion2
    {
        
        public static void MorpionGame()
        {
            char[,] tab;
            int Count = 0;
            int i;
            int j;
            string[] S = { "A1", "A2", "A3", "B1", "B2", "B3", "C1", "C2", "C3" };
            int[] N = { 1, 2 };
            Console.WriteLine("Début de partie de Morpion :");
            while (Count < 9)
            {
                Console.WriteLine($"Coup du joueur {N[0]} : ");
                string NUM =Console.ReadLine();
                if (S.Contains(NUM))                 
                {
                    Console.Write(NUM);
                    int indice = int.Parse(NUM.Substring(1, 1));
                    string C = NUM.Substring(0, 1);
                    switch(C)
                    {
                        case "A":
                            i = 0;
                            if (indice > 1 && indice < 3)
                            {

                                j = indice;
                                tab[i, j] = 'X';
                                Count++;
                            }
                            break;
                        case "B":
                            i = 1;
                            if (indice > 1 && indice < 3)
                            {

                                j = indice;
                                tab[i, j] = 'X';
                                Count++;
                            }
                            break;
                        case "C":
                            i = 2;
                            if (indice > 1 && indice < 3)
                            {

                                j = indice;
                                tab[i, j] = 'X';
                                Count++;
                            }
                            break;
                        default :
                            Console.WriteLine("Coup incorrect, Veuillez réssayer");
                            break;
                    }

            }

        }

        public static void DisplayMorpion(/*typeGrille grille */)
        {
            //TODO
            return;
        }

        public static int CheckMorpion(/*typeGrille grille */)
        {
            //TODO
            return -1;
        }
    }
}
