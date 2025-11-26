using Or.Business;
using Or.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Navigation;

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Depot.xaml
    /// </summary>
    public partial class Depot : PageFunction<long>
    {
        public Depot(long numCarte)
        {
            InitializeComponent();
            Montant.Text = 0M.ToString("C2");

            var view = CollectionViewSource.GetDefaultView(SqlRequests.ListeComptesAssociesCarte(numCarte));
            view.GroupDescriptions.Add(new PropertyGroupDescription("TypeDuCompte"));
            view.SortDescriptions.Add(new SortDescription("TypeDuCompte", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("IdentifiantCarte", ListSortDirection.Ascending));
            Destinataire.ItemsSource = view;
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }

        private void ValiderDepot_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(Montant.Text.Replace(".", ",").Trim(new char[] { '€', ' ' }), out decimal montant) && montant > 0)
            {
                //Compte fictif pour permettre la transaction
                Compte compteBanque = new Compte(0, 0, TypeCompte.Courant, 0);
                Compte de = Destinataire.SelectedItem as Compte;
                if(de != null){
                    Transaction t = new Transaction(0, DateTime.Now, montant, compteBanque.Id, de.Id);
                    CodeResultat C1 = de.EstDepotValide(t);
                    if (C1 == CodeResultat.OK)
                    {
                        SqlRequests.EffectuerModificationOperationSimple(t, de.IdentifiantCarte);

                        OnReturn(null);
                    }
                    else
                    {
                        MessageBox.Show(Tools.Label(C1));
                    }
                }
                else
                {
                    MessageBox.Show("Erreur de destinataire! Indiquez le destinataire souhaité.", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
            else
            {
                MessageBox.Show(Tools.Label(CodeResultat.MontantInvalide), "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
