namespace Gestion_Systéme_bancaire
{
    public class CompteBancaire
    {
        public int Identifiant { get; set; }
        // Bonne idée de ne pas autoriser la modification de Solde
        public decimal Solde { get; private set; }
        public string TypeCompte { get; set; }
        public long NumeroCarte { get; set; } // lié à la carte
        
        public CompteBancaire(int idt, decimal solde, string type, long numero)
        {
            Identifiant = idt;
            Solde = solde;
            TypeCompte = type;
            NumeroCarte = numero;
        }
        
        // Méthodes OK
        public bool DepotArgent(decimal Montant)
        {
            bool Statut = true;
            if(Montant > 0)
            {
                Solde += Montant;
            }
            else
            {
                Statut = false;
            }
            return Statut;
        }
        public bool RetireArgent(decimal Montant)
        {
            bool Statut = true;
            if (Montant > 0 && Solde >= Montant)
            {
                Solde -= Montant;
            }
            else
            {
                Statut = false;
            }
            return Statut;
        }
    }
}
