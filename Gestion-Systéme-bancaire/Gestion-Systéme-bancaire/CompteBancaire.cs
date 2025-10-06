using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gestion_Systéme_bancaire
{
    public class CompteBancaire
    {
        public int Identifiant { get; set; }
        public decimal Solde { get; set; }
        public string TypeCompte { get; set; }
        public string NumeroCarte { get; set; } // lié à la carte
        
        
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
