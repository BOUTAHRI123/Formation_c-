using System.Collections.Generic;
using System.Linq;

namespace Bataille_Navale
{
    internal class Bateau
    {
        public string Nom { get; private set ; }
        public int Taille { get; private set; }
        public List<Position> Positions { get; private set; }

        public Bateau(string nom, int taille, List<Position> position)
        {
            Nom = nom;
            Taille = taille;
            Positions = position;
        }

        /// <summary>
        /// Case à l'état touché si elle appartient au bateau
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        // Implémentation de la méthode Touché 
        public void Touché(int x, int y)
        {
            // Pourquoi tu crées une position P ?!? 
            /*Position P = new Position(x, y);
            if (P.Statut == Position.Etat.Coulé)
            {
                P.Touché();
            }*/

            for (int i = 0; i < Positions.Count; i++)
            {
                if (Positions[i].X == x && Positions[i].Y == y)
                {
                    Positions[i].Touché();
                }
            }
        }

        /// <summary>
        /// Le bateau est-il coulé ? 
        /// </summary>
        // Implémentation de la methode EstCoulé() (Question 2/b)
        public bool EstCoulé()
        {

            foreach (Position p in Positions)
            {
                if (p.Statut != Position.Etat.Touché && p.Statut != Position.Etat.Coulé)
                {
                    return false;
                }

                /*if (p.Statut == Position.Etat.Touché)
                {
                    return true;
                }*/
            }
            return true;
        }

    }
}