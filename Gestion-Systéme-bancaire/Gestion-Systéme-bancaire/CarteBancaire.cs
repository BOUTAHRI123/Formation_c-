using System;
using System.Collections.Generic;

namespace Gestion_Systéme_bancaire
{
    public class CarteBancaire
    {
        public long NumeroCarte { get; set; }
        public List<CompteBancaire> ComptesAssocies { get; set; }
        public List<Transaction> Historique { get; set; }
        public decimal Plafond { get; set; } = 500;

        public CarteBancaire(long num, decimal plaf)
        {
            NumeroCarte = num;
            Plafond = plaf;
        }
       
        // Plutôt privée si tu l'utilises que dans PeutEffectuer
        private decimal TotalTran(DateTime Date)
        {
            decimal Totale = 0;
            foreach(Transaction t in Historique)
            {
                TimeSpan diff = Date - t.Date ;
                // les virements sont aussi à prendre en compte, pas juste les retraits
                if (t.CompteDestination == 0 && diff.TotalDays <= 10)
                {
                    Totale += t.Montant;
                }
            }


            return Totale;
        }

        public bool PeutEffectuer(decimal montant, DateTime date)
        {
            decimal total = TotalTran(date);
            /*if ((total + montant) <= Plafond)
                return true;
            else
                return false;*/
            return (total + montant) <= Plafond;

        }



    }
}
