using Or.Business;
using Or.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Page
    {
        public Accueil()
        {
          
            InitializeComponent();
        }

        public void GoConsultationCarte(object sender, RoutedEventArgs e)
        {
            
            bool estCarteValide = long.TryParse(NumeroCarte.Text, out long result);
            Carte c = SqlRequests.InfosCarte(result);
            if (estCarteValide && c != null)
            {
                NavigationService.Navigate(new ConsultationCarte(result));
            }
            else
            {
                MessageBox.Show("Numéro de carte invalide", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GoMouse(object sender, RoutedEvent e)
        {

        }
        private void GoCreerCompte_Click(object sender, RoutedEventArgs e)
        {
            // Navigation vers la page de création de compte
            CreerCompte pageCreation = new CreerCompte();
            this.NavigationService.Navigate(pageCreation);
        }

        private void GoSupprimerCarte_Click(object sender, RoutedEventArgs e)
        {
            // Boîte simple pour demander le numéro de carte
            string num = Microsoft.VisualBasic.Interaction.InputBox("Entrez le numéro de la carte à supprimer :","Suppression d'une carte", "");

            if (string.IsNullOrWhiteSpace(num))
                return;

            if (!long.TryParse(num, out long idCarte))
            {
                MessageBox.Show("Numéro de carte invalide.");
                return;
            }

            // Vérifier si la carte existe
            Carte c = SqlRequests.InfosCarte(idCarte);
            if (c == null)
            {
                MessageBox.Show("Cette carte n'existe pas !");
                return;
            }

            // Confirmation
            if (MessageBox.Show("Voulez-vous VRAIMENT supprimer cette carte et tous ses comptes associés ?",  "Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            try
            {
                SqlRequests.SupprimerCarteEtComptes(idCarte);
                MessageBox.Show("Carte supprimée avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
            }
        }
    }
}
