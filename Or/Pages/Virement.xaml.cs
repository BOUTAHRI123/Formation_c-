using Or.Business;
using Or.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Or.Pages
{
    /// <summary>
    /// Logique d'interaction pour Virement.xaml
    /// </summary>
    public partial class Virement : PageFunction<long>
    {

        Carte CartePorteur { get; set; }
        Compte ComptePorteur { get; set; }
       
        public Virement(long numCarte)
        {
            InitializeComponent();

            Montant.Text = 0M.ToString("C2");

            CartePorteur = SqlRequests.InfosCarte(numCarte);
            CartePorteur.AlimenterHistoriqueEtListeComptes(SqlRequests.ListeTransactionsAssociesCarte(numCarte), SqlRequests.ListeComptesAssociesCarte(CartePorteur.Id).Select(x=>x.Id).ToList());
            ComptePorteur = SqlRequests.ListeComptesAssociesCarte(CartePorteur.Id).Find(x => x.TypeDuCompte == TypeCompte.Courant);
            PlafondMaxRetrait.Text = CartePorteur.Plafond.ToString("C2");
            Solde.Text = ComptePorteur.Solde.ToString("C2");
            // OK - fonctionnement normal
            PlafondActuelRetrait.Text = CartePorteur.SoldeCarteActuel(DateTime.Now).ToString("C2");

            var viewExpediteur = CollectionViewSource.GetDefaultView(SqlRequests.ListeComptesAssociesCarte(numCarte));
            viewExpediteur.GroupDescriptions.Add(new PropertyGroupDescription("TypeDuCompte"));
            viewExpediteur.SortDescriptions.Add(new SortDescription("TypeDuCompte", ListSortDirection.Ascending));
            viewExpediteur.SortDescriptions.Add(new SortDescription("IdentifiantCarte", ListSortDirection.Ascending));
            Expediteur.ItemsSource = viewExpediteur;

            ListeDestinataires();

   
        }

        private void Expediteur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListeDestinataires();

        }
        private void ListeDestinataires()
        {
            try
            {
                List<Compte> listeDestinataires = new List<Compte>();

                if (Expediteur.SelectedItem is Compte ex)
                {
                    // Si l'expéditeur est un compte Livret
                    if (ex.TypeDuCompte == TypeCompte.Livret)
                    {
                        Solde.Text = ex.Solde.ToString("C2");
                        // On ne peut faire un virement que vers le compte Courant de la même carte
                        var comptesCarte = SqlRequests.ListeComptesAssociesCarte(CartePorteur.Id);
                        var compteCourant = comptesCarte.FirstOrDefault(c => c.TypeDuCompte == TypeCompte.Courant);

                        if (compteCourant != null && compteCourant.Id != ex.Id)
                        {
                            listeDestinataires.Add(compteCourant);
                        }
                    }
                    else
                    {
                        /*Si l'expéditeur est un compte Courant
                         On peut faire un virement vers :les bénéficiaires enregistrésle compte Livret de la même carte*/

                        // Ajouter les bénéficiaires de la carte
                        var benefs = SqlRequests.ListeBeneficiairesAssocieClient(CartePorteur.Id);
                        if (benefs != null && benefs.Count > 0)
                        {
                            var comptesBenef = benefs
                            .Select(b => SqlRequests.ObtenirCompteParId(b.IdCompte))
                            .Where(c => c != null)
                            .ToList();
                            listeDestinataires.AddRange(comptesBenef);
                        }

                        // Ajouter le compte Livret de la même carte
                        var comptesCarte = SqlRequests.ListeComptesAssociesCarte(CartePorteur.Id);
                        var compteLivret = comptesCarte.FirstOrDefault(c => c.TypeDuCompte == TypeCompte.Livret);
                        if (compteLivret != null && compteLivret.Id != ex.Id)
                        {
                            // On ne le rajoute que s'il n'est pas déjà dans la liste
                            if (!listeDestinataires.Any(c => c.Id == compteLivret.Id))
                            {
                                listeDestinataires.Add(compteLivret);
                            }
                        }
                    }

                    // Exclure toujours le compte expéditeur lui-même
                    listeDestinataires = listeDestinataires.Where(c => c.Id != ex.Id).ToList();

                    // Affecter la liste des destinataires à la ComboBox
                    Destinataire.ItemsSource = listeDestinataires;
                }
                else
                {
                    // Si aucun expéditeur n'est encore sélectionné
                    Destinataire.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des destinataires : " + ex.Message);
            }
            
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }

        private void ValiderVirement_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(Montant.Text.Replace(".", ",").Trim(new char[] { '€', ' ' }), out decimal montant))
            {
                Compte ex = Expediteur.SelectedItem as Compte;
                Compte de = Destinataire.SelectedItem as Compte;
                if (ex != null)
                {
                    if(de != null)
                    {
                        Transaction t = new Transaction(0, DateTime.Now, montant, ex.Id, de.Id);
                        CodeResultat C1 = (Expediteur.SelectedItem as Compte).EstRetraitValide(t);
                        CodeResultat C2 = CartePorteur.EstRetraitAutoriseNiveauCarte(t, ex, de);
                        if (C1 == CodeResultat.OK && C2 == CodeResultat.OK)
                        {
                            SqlRequests.EffectuerModificationOperationInterCompte(t, ex.IdentifiantCarte, de.IdentifiantCarte);
                            OnReturn(null);
                        }
                        else if (C1 != CodeResultat.OK && C2 == CodeResultat.OK)
                        {
                            MessageBox.Show(Tools.Label(C1));
                        }
                        else if (C2 != CodeResultat.OK && C1 == CodeResultat.OK)
                        {
                            MessageBox.Show(Tools.Label(C2));
                        }
                        else if (C2 != CodeResultat.OK && C1 != CodeResultat.OK)
                        {
                            MessageBox.Show(Tools.Label(CodeResultat.MontantInvalide));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur de destinataire! Indiquez le destinataire souhaité.", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Erreur d'expéditeur! Merci de préciser l'expéditeur souhaité.", "Saisie invalide", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
               
            }
            else
            {
                MessageBox.Show(Tools.Label(CodeResultat.MontantInvalide));
            }

        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            AjouterBeneficiaire pageAjout = new AjouterBeneficiaire(CartePorteur.Id);
            this.NavigationService.Navigate(pageAjout);
        }
    }
}
