namespace Bataille_Navale
{
    internal class Position
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public enum Etat { Caché = 'O', Touché = 'T', Coulé = 'X', Plouf = 'P' }
        public Etat Statut { get; private set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
            Statut = Etat.Caché;
        }
        // Réponse au question 1 
        public void Touché() 
		{
            Statut = Etat.Touché;
		}

        public void Coulé() 
		{
            Statut = Etat.Coulé;
		}

        public void Plouf() 
		{
            Statut = Etat.Plouf;
		}
    }
}