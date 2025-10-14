using Or.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Or.Models
{
    class GestionImportCompte
    {
        List<ExportCompte> ComptesImport;
        // Déserialiser les comptes à partir de fichier XML 
        public List<ExportCompte> DeSerialiserTransactions(string nomFichier)
        {
            
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ExportCompte>));
                using (TextReader reader = new StreamReader(nomFichier))
                {
                    return ComptesImport=(List<ExportCompte>)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la désérialisation : " + ex.Message);
                return ComptesImport = new List<ExportCompte>();
            }
        }
        // Traiter les transactions Importées 
        public void TransactionsImportees(List<Transaction> transactions)
        {
            foreach (var t in transactions)
            {
                
                // Vérifier si les comptes existent, le solde est suffisant, le type est correct
                if (SqlRequests.ObtenirCompteParId(t.Expediteur) == null || SqlRequests.ObtenirCompteParId(t.Destinataire) == null)
                    continue;

                // Vérifie que la transaction est valide 
                Operation typeOpe = Tools.TypeTransaction(t.Expediteur, t.Destinataire);
                if (typeOpe == Operation.InterCompte || typeOpe == Operation.DepotSimple || typeOpe == Operation.RetraitSimple)
                {
                    
                    SqlRequests.EffectuerModificationOperationSimple(t, t.Expediteur);
                }
            }
        }
    }
}

