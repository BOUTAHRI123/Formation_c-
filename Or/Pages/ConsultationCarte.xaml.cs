using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Or.Business;
using Or.Models;
using Microsoft.VisualBasic;




namespace Or.Pages
{
    /// <summary>
    /// Logique ht'interaction pour ConsultationCarte.xaml
    /// </summary>
    public partial class ConsultationCarte : PageFunction<long>
    {
        public ConsultationCarte(long numCarte)
        {
            InitializeComponent();
            Carte c = SqlRequests.InfosCarte(numCarte);
            if (c != null)
            {
                Numero.Text = c.Id.ToString();
                Prenom.Text = c.PrenomClient;
                Nom.Text = c.NomClient;

                listView.ItemsSource = SqlRequests.ListeComptesAssociesCarte(numCarte);
            }
            else
            {
                MessageBox.Show("Numéro de carte invalide", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
        private void GoDetailsCompte(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new DetailsCompte(long.Parse(Numero.Text), (int)(sender as Button).CommandParameter));
        }

        private void GoHistoTransactions(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new HistoriqueTransactions(long.Parse(Numero.Text)));
        }

        private void GoVirement(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new Virement(long.Parse(Numero.Text)));
        }

        private void GoRetrait(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new Retrait(long.Parse(Numero.Text)));
        }

        private void GoDepot(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new Depot(long.Parse(Numero.Text)));
        }
        private void GoConseiller(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new Conseille(long.Parse(Numero.Text)));
        }

        void PageFunctionNavigate(PageFunction<long> page)
        {
            page.Return += new ReturnEventHandler<long>(PageFunction_Return);
            NavigationService.Navigate(page);
        }

        void PageFunction_Return(object sender, ReturnEventArgs<long> e)
        {
            listView.ItemsSource = SqlRequests.ListeComptesAssociesCarte(long.Parse(Numero.Text));
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gridView = listView.View as GridView;
            if (gridView != null)
            {
                double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
                gridView.Columns[0].Width = totalWidth * 0.10; // 10%
                gridView.Columns[1].Width = totalWidth * 0.30; // 40%
                gridView.Columns[2].Width = totalWidth * 0.30; // 20%
                gridView.Columns[3].Width = totalWidth * 0.30; // 20%
            }
        }
        private void Exportfichier(object sender, RoutedEventArgs e)
        {
            string nomfichier = "fichierExport.xml";
            GestionExportCompte G = new GestionExportCompte();
            G.SerialiserComptesTransaction(long.Parse(Numero.Text), nomfichier);
        }
        private void GoListeBeneficiaires_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListeBeneficiaire(long.Parse(Numero.Text)));
        }
        // Fonction du bouton Importer Transactions 
        private void BtnImporterTransactions_Click(object sender, RoutedEventArgs e)
        {
            
            // Si l'utilisateur n'a pas saisi de fichier
            if (string.IsNullOrWhiteSpace(TxtFichierImport.Text))
            {
                MessageBox.Show("Veuillez indiquer le chemin du fichier à importer.");
                return;
            }

            string fichierImport = TxtFichierImport.Text.Trim();

            // Vérification d'existence du fichier
            if (!File.Exists(fichierImport))
            {
                MessageBox.Show("Fichier introuvable !");
                return;
            }
            // Création de la liste des exportComptes afin de stocker les comptes importer 
            List<ExportCompte> ComptesImport = new List<ExportCompte>();
            GestionImportCompte import = new GestionImportCompte();
            // implimenter la liste des Comptes avec la liste return par la Fonction 
            ComptesImport = import.DeSerialiserTransactions(fichierImport);
            List<Transaction> Transaction = new List<Transaction>();

            foreach(ExportCompte C in ComptesImport)
            {
                if(C.ListeTransactions != null)
                {
                    foreach(var t in C.ListeTransactions)
                    {
                        Transaction trans = new Transaction(
                        t.IdTransaction,
                        t.Horodatage.Value,
                        t.Montant,
                        t.Expediteur,
                        t.Destinataire
                        );
                        Transaction.Add(trans);
                    }
                }
                import.TransactionsImportees(Transaction);
            }
        
            if(ComptesImport.Count != 0)
            {
                MessageBox.Show("Importation des transactions réussie !");
            }
            
          
        }
        private void GoAjouteCompte(object sender, RoutedEventArgs e)
        {
            try
            {
                long numCarte = long.Parse(Numero.Text);

                // Création automatique d’un nouveau compte livret avec solde 0
                SqlRequests.AjouterCompte(numCarte, "Livret", 0m);

                MessageBox.Show("Nouveau compte Livret ajouté avec succès !", "Ajout réussi", MessageBoxButton.OK, MessageBoxImage.Information);

                // Rafraîchir la liste des comptes affichés
                listView.ItemsSource = SqlRequests.ListeComptesAssociesCarte(numCarte);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création du compte Livret : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoSuppCompte(object sender, RoutedEventArgs e)
        {
            long numCarte = long.Parse(Numero.Text);

            // Récupère tous les comptes livret de la carte
            var comptesLivret = SqlRequests.ListeComptesAssociesCarte(numCarte).Where(c => c.TypeDuCompte == TypeCompte.Livret).ToList();

            if (comptesLivret.Count == 0)
            {
                MessageBox.Show("Aucun compte livret associé à cette carte.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Si plusieurs livrets, on demande lequel supprimer
            Compte compteASupprimer = null;

            if (comptesLivret.Count == 1)
            {
                compteASupprimer = comptesLivret.First();
            }
            else
            {
                string message = "Sélectionnez le compte Livret à supprimer :\n\n";
                for (int i = 0; i < comptesLivret.Count; i++)
                {
                    message += $"{i + 1}. ID: {comptesLivret[i].Id} — Solde: {comptesLivret[i].Solde:C2}\n";
                }

                string choix = Interaction.InputBox(message, "Suppression compte Livret", "1");

                if (int.TryParse(choix, out int index) && index >= 1 && index <= comptesLivret.Count)
                {
                    compteASupprimer = comptesLivret[index - 1];
                }
                else
                {
                    MessageBox.Show("Choix invalide. Opération annulée.");
                    return;
                }
            }

            // Confirmation avant suppression
            if (compteASupprimer != null)
            {
                var confirmation = MessageBox.Show($"Voulez-vous vraiment supprimer le compte Livret (ID {compteASupprimer.Id}) ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

                if (confirmation == MessageBoxResult.Yes)
                {
                    bool ok = SqlRequests.SupprimerCompteLivret(compteASupprimer.Id);
                    if (ok)
                    {
                        MessageBox.Show("Compte Livret supprimé avec succès !");
                        listView.ItemsSource = SqlRequests.ListeComptesAssociesCarte(numCarte); // rafraîchit l’affichage
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du compte.");
                    }
                }
            }
        }
        private void GoChangerPlafond(object sender, RoutedEventArgs e)
        {
            PageFunctionNavigate(new ChangerPlafond(long.Parse(Numero.Text)));
        }




    }
}
