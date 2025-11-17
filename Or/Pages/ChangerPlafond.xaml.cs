using Or.Business;
using Or.Models;
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

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class ChangerPlafond : PageFunction<long>
    {
        long NumCarte;
        Carte CartePorteur;

        public ChangerPlafond(long numCarte)
        {
            InitializeComponent();
            NumCarte = numCarte;

            // Charger la carte
            CartePorteur = SqlRequests.InfosCarte(numCarte);

            TxtActuel.Text = CartePorteur.Plafond.ToString("C2");
        }

        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtNouveau.Text.Replace(".", ",").Trim(), out decimal nouveau))
            {
                MessageBox.Show("Montant invalide !", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (nouveau <= 0)
            {
                MessageBox.Show("Le plafond doit être strictement positif !", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Confirmation
            if (MessageBox.Show("Confirmer la modification du plafond ?","Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question)!= MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                SqlRequests.ModifierPlafondCarte(NumCarte, nouveau);
                MessageBox.Show("Plafond modifié avec succès !","Modification faite ", MessageBoxButton.OK, MessageBoxImage.Information);
                OnReturn(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }
    }
}
