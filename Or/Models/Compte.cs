using System.Collections.Generic;
using Or.Business;

namespace Or.Models
{
    public enum TypeCompte { Courant, Livret }

    public class Compte
    {
        public int Id { get; set; }
        public long IdentifiantCarte { get; set; }
        public TypeCompte TypeDuCompte { get; set; }
        public decimal Solde { get; private set; }

        public Compte(int id, long identifiantCarte, TypeCompte type, decimal soldeInitial)
        {
            Id = id;
            IdentifiantCarte = identifiantCarte;
            TypeDuCompte = type;
            Solde = soldeInitial;
        }

        /// <summary>
        /// Action de dépôt d'argent sur le compte bancaire
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Statut du dépôt</returns>
        public CodeResultat EstDepotValide(Transaction transaction)
        {
            if (transaction.Montant <= 0)
            {
                return CodeResultat.MontantInvalide;
            }
            return CodeResultat.OK;
        }

        /// <summary>
        /// Action de retrait d'argent sur le compte bancaire
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Statut du retrait</returns>
        public CodeResultat EstRetraitValide(Transaction transaction)
        {
            //if (!EstRetraitAutorise(transaction.Montant))
            //switch (EstRetraitAutorise(transaction.Montant))
            if(!EstRetraitAutorise(transaction.Montant))
            {
                //case CodeResultat.SoldeInsuffisant:
                if(Solde < transaction.Montant)
                {
                    return CodeResultat.SoldeInsuffisant;

                }
                else
                {
                    return CodeResultat.MontantInvalide;
                }
                
                //case CodeResultat.MontantInvalide:
                    //return CodeResultat.MontantInvalide;

                //default:
                
            }

            return CodeResultat.OK;
        }

        private bool EstRetraitAutorise(decimal montant)
        {
            /*if(Solde < montant)
            {
                return CodeResultat.SoldeInsuffisant;
            }
            if(montant <= 0)
            {
                return CodeResultat.MontantInvalide;
            }
            return CodeResultat.OK;*/

            return Solde >= montant && montant > 0;
        }

    }
}
