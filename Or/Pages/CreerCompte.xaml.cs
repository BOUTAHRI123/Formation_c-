using Or.Business;
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
    public partial class CreerCompte : PageFunction<long>
    {
        public CreerCompte()
        {
            InitializeComponent();
        }
        private void BtnCreer_Click(object sender, RoutedEventArgs e)
        {
            string nom = TxtNom.Text.Trim();
            string prenom = TxtPrenom.Text.Trim();
            int IdCon = Int32.Parse(TxtConseiller.Text.Trim());

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom))
            {
                MessageBox.Show("Veuillez saisir le nom et le prénom.");
                return;
            }

            // Génération d’un numéro de carte unique à 16 chiffres
            long numCarte = GenererNumeroCarteUnique();

            // Création de la carte
            SqlRequests.AjouterCarte(numCarte, nom, prenom,IdCon);

            // Création du compte courant associé
            SqlRequests.AjouterCompte(numCarte, "Courant", 0m);

            MessageBox.Show($"Compte créé avec succès !\nNuméro de carte : {numCarte}");

            NavigationService.GoBack();
        }

        private long GenererNumeroCarteUnique()
        {
            Random rand = new Random();
            long numCarte;

            do
            {
                string numStr = "";
                for (int i = 0; i < 16; i++)
                    numStr += rand.Next(0, 10);
                    numCarte = long.Parse(numStr);
            }
            while (SqlRequests.CarteExiste(numCarte));

            return numCarte;
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }

    }
}
