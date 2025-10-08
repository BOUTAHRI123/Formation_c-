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
            if (estCarteValide)
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
    }
}
