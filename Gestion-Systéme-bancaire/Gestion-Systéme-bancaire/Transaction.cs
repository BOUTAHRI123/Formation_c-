using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Systéme_bancaire
{
    public class Transaction
    {
        public string NumeroTransaction { get; set; }
        public DateTime Date { get; set; }
        public decimal Montant { get; set; }
        public int CompteSource { get; set; }
        public int CompteDestination { get; set; }
        public string Status { get; set; }

        public Transaction(string num,DateTime date,decimal montant,int compte1,int compte2)
        {
            NumeroTransaction = num;
            Date = date;
            Montant = montant;
            CompteSource = compte1;
            CompteDestination = compte2;
        }

    }
}
