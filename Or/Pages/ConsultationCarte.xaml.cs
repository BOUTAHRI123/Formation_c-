using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Or.Business;
using Or.Models;

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
            
            Numero.Text = c.Id.ToString();
            Prenom.Text = c.PrenomClient;
            Nom.Text = c.NomClient;

            listView.ItemsSource = SqlRequests.ListeComptesAssociesCarte(numCarte);
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


    }
}
