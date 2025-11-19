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
    public partial class SimulationCredit : Window
    {
        long IdCarte;
        List<Compte> ComptesCarte;
        decimal SoldeTotal;
        public SimulationCredit(long idCarte)
        {
            InitializeComponent();
            IdCarte = idCarte;
            // Récupérer tous les comptes associés à la carte
            ComptesCarte = SqlRequests.ListeComptesAssociesCarte(idCarte);

            // Somme du solde de TOUS les comptes (courant + livrets)
            SoldeTotal = ComptesCarte.Sum(c => c.Solde);
        
        }
        private void RbOui_Checked(object sender, RoutedEventArgs e)
        {
            LblCredit.Visibility = Visibility.Visible;
            TxtCreditExistant.Visibility = Visibility.Visible;
        }

        private void RbNon_Checked(object sender, RoutedEventArgs e)
        {
            LblCredit.Visibility = Visibility.Collapsed;
            TxtCreditExistant.Visibility = Visibility.Collapsed;
        }

        private void BtnSimuler_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtSalaire.Text, out decimal salaire) || salaire <= 0)
            {
                TxtResultat.Text = "Veuillez saisir un salaire valide !";
                return;
            }

            if (!decimal.TryParse(TxtMontant.Text, out decimal montant) || montant <= 0)
            {
                TxtResultat.Text = "Veuillez saisir un montant valide !";
                return;
            }

            if (!int.TryParse(CmbDuree.Text, out int duree) || duree <= 0)
            {
                TxtResultat.Text = "Veuillez saisir une durée valide !";
                return;
            }

            // Solde total déjà calculé dans le constructeur
            if (SoldeTotal <= 0)
            {
                TxtResultat.Text = " Crédit refusé : votre solde total est insuffisant.";
                return;
            }

            // Taux de la banque
            decimal taux = 0.04m;

            decimal tauxMensuel = taux / 12;
            decimal mensualite = (montant * tauxMensuel) / (1 - (decimal)Math.Pow(1 + (double)tauxMensuel, -duree));

            // Salaire doit être > 3 × mensualité
            if (salaire < mensualite * 3)
            {
                TxtResultat.Text =
                " Crédit refusé.\n" + $"Mensualité : {mensualite:C2}\n" + $"Votre salaire est insuffisant pour couvrir cette mensualité.";
                return;
            }

            TxtResultat.Text =
            "Crédit accepté !\n\n" + $"Montant : {montant:C2}\n" + $"Durée : {duree} mois\n" + $"Mensualité : {mensualite:C2}\n" + $"Solde total pris en compte : {SoldeTotal:C2}";
        }



        private void BtnFermer_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
