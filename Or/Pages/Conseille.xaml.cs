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
    public partial class Conseille : PageFunction<long>
    {
        private long CarteId;
        private Conseiller C;
        public Conseille(long numCarte)
        {
            InitializeComponent();
            CarteId = numCarte;
            Donnees();
            if (C != null)
            {
                TxtNom.Text = C.Nom;
                TxtPrenom.Text = C.Prenom;
                TxtTel.Text = C.NTel;
                TxtMail.Text = C.Mail;

            }

        }
        public void Donnees()
        {
            C = SqlRequests.InfoConseiller(CarteId);
            
        }
        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }

    }
}
