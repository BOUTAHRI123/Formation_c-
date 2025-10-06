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
        public List<string> ComptesAssocies { get; set; }
        public List<Transaction> Historique { get; set; }
        public decimal Plafond { get; set; } = 500;

       
       
        public decimal TotalTran(DateTime Date)
        {
            decimal Totale = 0;
            foreach(Transaction t in Historique)
            {
              
                if(t.CompteDestination == 0 && Date.AddDays(-10) >= t.Date )
                {
                    Totale += t.Montant;
                }
            }


            return Totale;
        }
            
       

    }
}
