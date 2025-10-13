using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Or.Models;
using Or.Business;

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class ListeBeneficiaire : PageFunction<long>
    {
        private Carte CartePorteur;
        private long CarteId;
        private List<Beneficiaire> Beneficiaires;
        public ListeBeneficiaire(long numCarte)
        {
            InitializeComponent();
            CarteId = numCarte;
            ChargerInfosCarte();
            Donnees();


        }

        private void ChargerInfosCarte()
        {
            CartePorteur = SqlRequests.InfosCarte(CarteId);

            if (CartePorteur != null)
            {
                IdCarte.Text = CartePorteur.Id.ToString();
                Nom.Text = CartePorteur.NomClient;
                Prenom.Text = CartePorteur.PrenomClient;
            }
        }
        public void Donnees()
        {
            Beneficiaires = SqlRequests.ListeBeneficiairesAssocieClient(CarteId);
            listbeneficiaire.ItemsSource = Beneficiaires;

        }
         private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }
        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            AjouterBeneficiaire pageAjout = new AjouterBeneficiaire(CarteId);
            this.NavigationService.Navigate(pageAjout);
        }
        public void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            Beneficiaire benef = (sender as Button).DataContext as Beneficiaire;
            if (benef == null)
            {
                MessageBox.Show("Veuillez sélectionner un bénéficiaire à supprimer.");
                return;
            }

            MessageBoxResult confirm = MessageBox.Show(
            $"Souhaitez-vous vraiment supprimer le bénéficiaire {benef.Nom} {benef.Prenom} (Compte n° {benef.IdCompte}) ?",
            "Confirmation de suppression",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);
            if (confirm == MessageBoxResult.Yes)
            {
                try
                {
                    SqlRequests.SupprimerBeneficiaire(benef.IdBenef);
                    MessageBox.Show("Bénéficiaire supprimé avec succès !");
                    Donnees();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la suppression : {ex.Message}");
                }

            }
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //GridView gridView = listView.View as GridView;
            //if (gridView != null)
            //{
               // double totalWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
               // gridView.Columns[0].Width = totalWidth * 0.10; // 10%
               // gridView.Columns[1].Width = totalWidth * 0.45; // 40%
               // gridView.Columns[2].Width = totalWidth * 0.45; // 20%
            //}
        }
    }
}
