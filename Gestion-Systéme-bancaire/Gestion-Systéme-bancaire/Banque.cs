using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Gestion_Systéme_bancaire
{
   // Suppression du code mort

    public class Banque
    {
        private static readonly decimal _plafondMin = 500;
        private static readonly decimal _plafondMax = 3000;
        private static readonly int _longueurNumCarte = 16;

        public static List<CompteBancaire> Comptes { get; set; } 
        public static List<CarteBancaire> Cartes { get; set; }
        public static List<Transaction> Transactions;

        public static List<CompteBancaire> LireCompte(string chemin)
        {

            Comptes = new List<CompteBancaire>();
            using (StreamReader File = new StreamReader(chemin))
            {
                string line;
                while ((line = File.ReadLine()) != null)
                {                 
                    string[] valeur;
                    valeur = line.Split(';');
                    if(valeur.Length < 4)
                    {
                        continue;
                    }
                    if(int.Parse(valeur[0]) <= 0)
                    {
                        continue;
                    }
                    // Un dictionnaire serait plus pratique
                    foreach(CarteBancaire c in Cartes)
                    {
                        // Couteux
                        if ((long)Convert.ToDouble(valeur[1]) == c.NumeroCarte)
                        {
                            if(valeur[2] == "Courant" || valeur[2] == "Livret")
                            {
                                if (string.IsNullOrEmpty(valeur[3]))
                                {
                                    valeur[3] = "0";
                                }
                                // Pas de vérification unicité
                                CompteBancaire compte = new CompteBancaire((int.Parse(valeur[0])), (decimal.Parse(valeur[3])), valeur[2], (long)Convert.ToDouble(valeur[1]));
                                Comptes.Add(compte);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }     
                    }
                }
                   
            }
            foreach (CarteBancaire c in Cartes)
            {
                // Tu mets à jour, pk ne pas juste rajouter le compte au lieu d'à chaque fois rechercher tous les comptes associés ? 
                c.ComptesAssocies = CompteAssocie(c);
            }
            return Comptes;
        }

        public static List<CarteBancaire> LireCarte(string chemin)
        {

            Cartes = new List<CarteBancaire>();
            using (StreamReader F = new StreamReader(chemin))
            {
                string line;
                while ((line = F.ReadLine()) != null)
                {
                    string[] valeur;
                    valeur = line.Split(';');
                    long num=0;
                    
                    // Pas de vérification unicité
                    if (long.TryParse(valeur[0], out num) == false && valeur[0].Length != _longueurNumCarte)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(valeur[1]))
                    {
                        valeur[1] = "500";
                    }

                    if (decimal.Parse(valeur[1]) < _plafondMin || decimal.Parse(valeur[1]) > _plafondMax)
                    {
                        continue;
                    }
                   
                    CarteBancaire carte = new CarteBancaire(num,decimal.Parse(valeur[1]));

                    Cartes.Add(carte);
                }
            }
            return Cartes;
        }

        public List<Transaction> LireTransaction(string chemin)
        {
            Transactions = new List<Transaction>();
            using (StreamReader File = new StreamReader(chemin))
            {
                string line;
                while ((line = File.ReadLine()) != null)
                {
                    string[] valeur;
                    valeur = line.Split(';');
                    DateTime date;
                    int source;
                    int Des;
                    if (valeur.Length < 5)
                    {
                        continue;
                    }
                    if (decimal.Parse(valeur[2]) < 0)
                    {
                        continue;
                    }
                    if (int.Parse(valeur[0]) <= 0)
                    {
                        continue;
                    }
                    if (!DateTime.TryParseExact(valeur[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture,DateTimeStyles.None, out date))
                    {
                        continue;
                    }
                    if(!int.TryParse(valeur[3], out source)){
                        continue;
                    }
                    if (!int.TryParse(valeur[4], out Des))
                    {
                        continue;
                    }
                    // Il serait bien de ne pas traiter 0 pour Destinataire et 0 en Source en même temps
                    Transaction T = new Transaction(Int32.Parse(valeur[0]), date, decimal.Parse(valeur[2]), source, Des);
                    Transactions.Add(T);

                }

            }
            foreach(CarteBancaire c in Cartes)
            {
                c.Historique = LireHistorique(c);
            }
            
            return Transactions;
        }

        private List<Transaction> LireHistorique(CarteBancaire c)
        {
            List<Transaction> liste = new List<Transaction>();
            foreach(Transaction t in Transactions)
            {
                foreach (CompteBancaire cpt in c.ComptesAssocies)
                {
                    if (t.CompteSource == cpt.Identifiant || t.CompteDestination == cpt.Identifiant)
                    {
                        liste.Add(t);
                    }
                }
            }
            return liste;
        }

        // Pourquoi static ? Pourquoi tu ne crées pas une instance de Banque ? Vu que tu utilises new Banque() dans Program
        private static List<CompteBancaire> CompteAssocie(CarteBancaire C)
        {
            List<CompteBancaire> liste2 = new List<CompteBancaire>();
            foreach (CompteBancaire cpt in Comptes)
            {
                if (C.NumeroCarte == cpt.NumeroCarte)
                {
                    liste2.Add(cpt);
                }
            }
            return liste2;
        }


        // Mise en privée
        private CompteBancaire TestCompte(int Id)
        {
            CompteBancaire compte = null;
            foreach (CompteBancaire C in Comptes)
            {
                if (C.Identifiant == Id)
                {
                    compte = C;
                }
            }
            return compte;
        }

        private CarteBancaire TestCarte(long Num)
        {
            CarteBancaire Carte = null;
            foreach (CarteBancaire Car in Cartes)
            {
                if (Car.NumeroCarte == Num)
                {
                    Carte = Car;
                }
            }
            return Carte;
        }

        public bool EffectuerTransaction(Transaction t)
        {
            bool statut = false;
            int source;
            int destination;
            CompteBancaire cpt;
            CarteBancaire carte;
            // Dépôt 
            if (t.CompteSource == 0)
            {
                destination = t.CompteDestination;

                if ((cpt = TestCompte(destination)) != null)
                {

                    statut = cpt.DepotArgent(t.Montant);
                }
            }
            // Retrait
            else if (t.CompteDestination == 0)
            {
                source = t.CompteSource;

                if ((cpt = TestCompte(source)) != null)
                {
                    if ((carte = TestCarte(cpt.NumeroCarte)) != null)
                    {
                        bool sttransaction = carte.PeutEffectuer(t.Montant, t.Date);
                        if (sttransaction)
                        {
                            statut = cpt.RetireArgent(t.Montant);
                        }
                    }
                }
            }
            // OK pour la cinématique
            else
            {
                source = t.CompteSource;
                destination = t.CompteDestination;
                CompteBancaire cpt2;
                if ((cpt = TestCompte(source)) != null)
                {
                    if ((carte = TestCarte(cpt.NumeroCarte)) != null)
                    {
                        bool sttransaction = carte.PeutEffectuer(t.Montant, t.Date);
                        if (sttransaction)
                        {
                            statut = cpt.RetireArgent(t.Montant);
                            if ((cpt2 = TestCompte(destination)) != null)
                            {
                                statut = cpt2.DepotArgent(t.Montant);
                            }
                        }
                    }
                }
            }
            return statut;
        }

        public void EcritSortie(string chemin)
        {
            using(StreamWriter sortie = new StreamWriter(chemin))
            { 
                foreach(Transaction t in Transactions)
                {
                    bool statut = EffectuerTransaction(t);
                    if (statut)
                    {
                        t.Status = "OK";
                    }
                    else
                    {
                        t.Status = "KO";
                    }

                    sortie.WriteLine($"{(t.NumeroTransaction)}; {t.Status}");
                }
            }
            Console.WriteLine("C'est bien fait!");
        }
    }

}


