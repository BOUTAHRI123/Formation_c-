using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Or.Business;
using System.IO;
using System.Xml.Serialization;

namespace Or.Models
{
    public class GestionExportCompte
    {

        public void SerialiserComptesTransaction(long Id,string fichierExport)
        {
            List<Compte> Comptes = SqlRequests.ListeComptesAssociesCarte(Id);
            List<ExportCompte> ExportComptes = new List<ExportCompte>();

            foreach(Compte cpt in Comptes)
            {
                ExportCompte compte = new ExportCompte
                {
                    Id = cpt.Id,
                    TypeDuCompte = cpt.TypeDuCompte,
                    Solde = cpt.Solde.ToString("C2"),
                    ListeTransactions = new List<ExportCompteTransacctions>()
                };
                Carte carte = SqlRequests.InfosCarte(Id);
                List<Transaction> Transactions = carte.Historique;
                foreach(Transaction t in Transactions)
                {
                    ExportCompteTransacctions transac = new ExportCompteTransacctions
                    {
                        IdTransaction = t.IdTransaction,
                        Horodatage = t.Horodatage.Value,
                        Montant = t.Montant,
                        TypeTransaction = t.TypeTransaction,
                        Expediteur = t.Expediteur,
                        Destinataire = t.Destinataire
                    };
                    compte.ListeTransactions.Add(transac);
                    
                }
                ExportComptes.Add(compte);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<ExportCompte>));
            using (TextWriter writer = new StreamWriter(fichierExport))
            {
                serializer.Serialize(writer, ExportComptes);
            }
        }
    }
}
