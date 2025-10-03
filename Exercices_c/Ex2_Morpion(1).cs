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
            // Initialisation de la Morpion avec des '_'
            char[,] Tab = new char[3, 3];
            for (int i = 0; i < Tab.GetLength(0); i++)
            {
                for (int j = 0; j < Tab.GetLength(1); j++)
                {
                    Tab[i, j] = '_';
                }
            }

            Console.WriteLine("Début de partie de Morpion :");

            int joueur = 1;
            int etat = -1;
            string Letter = "ABC";
            string Chiffre = "123";

            while (etat == -1)
            {
                DisplayMorpion(Tab);

                Console.Write($"Coup du joueur {joueur} : ");
                string coup = Console.ReadLine().Trim().ToUpper();

                //if (coup.Length != 2 || coup[0] < 'A' || coup[0] > 'C' || coup[1] < '1' || coup[1] > '3')
                if (coup.Length != 2 || !Letter.Contains(coup[0]) || !Chiffre.Contains(coup[1]))
                {
                    Console.WriteLine("Coup incorrect, veuillez réessayer.");
                    continue;
                }


                int ligne = coup[0] - 'A';  // donne l'indice direct de la ligne
                int colonne = coup[1] - '1'; // donne l'indice de la colonne
                // Vérifier si la case dans la table est deja remplit ou pas
                if (Tab[ligne, colonne] != '_')
                {
                    Console.WriteLine("Cette case est déjà occupée, veuillez réessayer.");
                    continue;
                }


                Tab[ligne, colonne] = (joueur == 1) ? 'X' : 'O';

                etat = CheckMorpion(Tab);
                if (etat == -1)
                {
                    joueur = (joueur == 1) ? 2 : 1;
                }

            }

            DisplayMorpion(Tab);

            if (etat == 0)
            {
                Console.WriteLine("Match nul !");
            }
            else
            {
                Console.WriteLine($"Le joueur {etat} remporte la partie !");
            }

        }

        public static void DisplayMorpion(char[,] Tab)
        {

            if (Tab == null || Tab.GetLength(0) != 3 || Tab.GetLength(1) != 3)
                throw new ArgumentException("La Table doit etre de dimension 3x3 !");


            for (int i = 0; i < 3; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    Console.Write(Tab[i, j] + " ");
                }
                Console.WriteLine();
            }


        }

        public static int CheckMorpion(char[,] Tab)
        {
            if (Tab == null || Tab.GetLength(0) != 3 || Tab.GetLength(1) != 3)
                throw new ArgumentException("La Table doit etre de dimension 3x3 !");

            for (int i = 0; i < 3; i++)
            {
                // les lignes
                if (Tab[i, 0] != '_' && Tab[i, 0] == Tab[i, 1] && Tab[i, 1] == Tab[i, 2])
                    return Tab[i, 0] == 'X' ? 1 : 2;

                //les colonnes
                if (Tab[0, i] != '_' && Tab[0, i] == Tab[1, i] && Tab[1, i] == Tab[2, i])
                    return Tab[0, i] == 'X' ? 1 : 2;
            }

                // diagonale principale
                if (Tab[0, 0] != '_' && Tab[0, 0] == Tab[1, 1] && Tab[1, 1] == Tab[2, 2])
                    return Tab[0, 0] == 'X' ? 1 : 2;

                // diagonale secondaire
                if (Tab[0, 2] != '_' && Tab[0, 2] == Tab[1, 1] && Tab[1, 1] == Tab[2, 0])
                    return Tab[0, 2] == 'X' ? 1 : 2;

                // partie non terminée = existnce de '_' encore dans la table
                foreach (char c in Tab)
                    if (c == '_')
                        return -1;

                //personne n'a gagné 
                return 0;
            
        }
    }
}
