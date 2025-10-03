using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bataille_Navale
{
    internal class Plateau
    {
        public Position[,] PlateauJeu { get; set; }

        public List<Bateau> Bateaux { get; set; }

        public Plateau(int taille)
        {
            PlateauJeu = new Position[10, 10];
            Bateaux = new List<Bateau>()
            {
               new Bateau("A", 5, new List<Position>()),
               new Bateau("B", 4, new List<Position>()),
               new Bateau("C", 3, new List<Position>()),
               new Bateau("D", 3, new List<Position>()),
               new Bateau("E", 2, new List<Position>())
            };
        }

        public void CreationPlateau()
        {
            
            Random R = new Random();
            // StatuN prend 0 si le bateau n'a pas deplacer
            int StatuN = 0;
            while (StatuN == 0)
            {
                int x = R.Next();
                int y = R.Next();
                bool Statut = false;
                bool Vertical = (R.Next(2) == 0);
                foreach(Bateau B in Bateaux) {
                    Statut = PlacerBateau(x, y, B.Taille, Vertical) ;      
                }
                // on change le StatuN si on on placer le bateau afin de passer au suivant 
                if (Statut)
                {
                    StatuN = 1;
                }
                
            }
                
            
            
        }
        
        public void LancementPartie()
        {
            int cpt = 0;
            while (!FindePartie())
            {
                Console.Clear();
                AfficherPlateau();

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Quelle case visez-vous : (format: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("ligne");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(",");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("colonne");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(")");
                Console.WriteLine();

                string val = Console.ReadLine();
                string[] position = val.Split(',', '.');

				// Partie à implémenter
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            AfficherPlateau();
            Console.Write($"GG {cpt} coups effectués !");
        }

        /// <summary>
        /// Peut-on placer le navire sur la grille sans qu'il dépasse les bords et qu'il ne touche les autres bateaux ? 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="taille"></param>
        /// <param name="estVertical"></param>
        /// <returns></returns>
        private bool PlacerBateau(int x, int y, int taille, bool estVertical)
        {
            List<Position> list = new List<Position>();
            foreach (Bateau B in Bateaux)
            {
                list.AddRange(B.Positions);
                foreach(Position P in B.Positions)
                {
                    if (list.Contains(P) != true)
                    {
                        return true;

                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// Choix de la case (x , y) 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        // Question 6 
        public void Viser(int x, int y)
        {
            // Vérifier est ce les coordonnées x et y sont dans le Plateau 
            if (x > 1 && x < PlateauJeu.Length && y > 1 && y < PlateauJeu.Length)
            {
                // Parcourir toutes les bateaux 
                foreach (Bateau B in Bateaux)
                {
                     // Parcourir toutes les positions du Bateau
                    foreach( Position P in B.Positions)
                    {
                        // Vérifier l'une  des positions du Bateau correspond au Coordonnées donnés
                        if (P.X == x && P.Y == y)
                        {
                            // Modifier le statut de la position à un statut Touché
                            P.Touché();
                        }
                        else
                        {
                            // met la position dans le plateau qui avait les Coordonnées x,y en Statut pleufé
                            PlateauJeu[x,y].Plouf();
                        }

                    }
                }
               




            }


        }

        /// <summary>
        /// Affichage de l'état de la grille et de la situation de la partie
        /// </summary>
        public void AfficherPlateau()
        {
            List<Position> list = new List<Position>();
            foreach (Bateau b in Bateaux)
            {
                list.AddRange(b.Positions);
                Console.WriteLine($"{b.Nom}: {b.Taille} de long, coulé: {b.EstCoulé()}");
            }

            foreach (Position p in list)
            {
                PlateauJeu.SetValue(p, p.X, p.Y);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
            int cpt = 0, tmp = 0;
            foreach (Position p in PlateauJeu)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                if (p.X != tmp || cpt == 0)
                {
                    if (cpt > 0)
                    {
                        Console.WriteLine();
                    }
                    Console.Write(string.Format("{0,-3}", ++cpt));
                }

                ConsoleColor foreground;
                switch (p.Statut)
                {
                    case Position.Etat.Plouf:
                        foreground = ConsoleColor.Blue;
                        break;
                    case Position.Etat.Touché:
                        foreground = ConsoleColor.Red;
                        break;
                    case Position.Etat.Coulé:
                        foreground = ConsoleColor.Green;
                        break;
                    default:
                        foreground = ConsoleColor.White;
                        break;
                }
                Console.ForegroundColor = foreground;
                Console.Write((char)p.Statut + " ");

                tmp = p.X;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        /// <summary>
        /// La partie est-elle finie ? 
        /// </summary>
        /// <returns></returns>
        internal bool FindePartie() 
		{
            // Parcourir toutes les bateaux 
            foreach (Bateau B in Bateaux)
            {
                // Parcourir toutes les positions du Bateau
                foreach (Position P in B.Positions)
                {
                    if (P.Statut == Position.Etat.Coulé)
                    {

                    }
                    
                }
            }
            return false;
        }
    }
}