using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Gestion_Systéme_bancaire
{
    public class CarteBancaire
    {
        public string NumeroCarte { get; set; }
        public List<CompteBancaire> ComptesAssocies { get; set; }
        public List<Transaction> Historique { get; set; }
        public decimal Plafond { get; set; } = 500;

        public CarteBancaire(string num, decimal plaf)
        {
            NumeroCarte = num;
            Plafond = plaf;
        }
       
        public decimal TotalTran(DateTime Date)
        {
            decimal Totale = 0;
            foreach(Transaction t in Historique)
            {
                TimeSpan diff = Date - t.Date ;
                //if (t.CompteDestination == 0 && Date.AddDays(-10) >= t.Date )
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
            if ((total + montant) <= Plafond)
                return true;
            else
                return false;
        }



    }
}
