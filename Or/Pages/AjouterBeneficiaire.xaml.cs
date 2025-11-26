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
    /// Logique d'interaction pour AjouterBeneficiaire.xaml
    /// </summary>
    public partial class AjouterBeneficiaire : PageFunction<long>
    {
        long IdCarte;
        public AjouterBeneficiaire(long idCarte)
        {
            InitializeComponent();
            IdCarte = idCarte;

        }
        private void Valider_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            // Vérification de la saisie
            if (!int.TryParse(IdCompte.Text, out int idCompte))
            {
                MessageBox.Show("Numéro de compte invalide !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Vérification que le compte existe et est de type "Courant"
            Compte compte = SqlRequests.ObtenirCompteParId(idCompte);
            if (compte == null)
            {
                MessageBox.Show("Ce compte n’existe pas !", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Vérifier si ce bénéficiaire existe déjà
            if (SqlRequests.ExisteBeneficiaire(IdCarte, idCompte))
            {
                MessageBox.Show("Ce bénéficiaire existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (compte.TypeDuCompte == TypeCompte.Livret)
            {
                if (compte.IdentifiantCarte == IdCarte)
                {
                    // Cas autorisé : Livret appartenant à la même carte
                    result = MessageBox.Show(
                    "Voulez-vous vraiment ajouter votre propre compte Livret comme bénéficiaire ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SqlRequests.AjouterBeneficiaire(IdCarte, idCompte);
                            MessageBox.Show("Bénéficiaire ajouté avec succès !", "Bien faite", MessageBoxButton.OK, MessageBoxImage.Information);
                            OnReturn(null);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur lors de l’ajout : " + ex.Message);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Impossible d’ajouter un compte Livret comme bénéficiaire !");
                    return;
                }

            }
            else
            {
                // Demande de confirmation avant ajout
                result = MessageBox.Show(
                "Voulez-vous vraiment ajouter ce bénéficiaire ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        SqlRequests.AjouterBeneficiaire(IdCarte, idCompte);
                        MessageBox.Show($"Bénéficiaire  ajouté avec succès !", "Bien faite", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnReturn(null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l’ajout : " + ex.Message);
                    }
                }
            }
        }
    
        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }

    }
}
