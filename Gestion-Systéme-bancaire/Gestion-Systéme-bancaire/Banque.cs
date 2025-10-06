using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Gestion_Systéme_bancaire
{
   

    public class Banque
    {


        public static List<CompteBancaire> Comptes { get; set; } 
        public static List<CarteBancaire> Cartes { get; set; }
        public static List<Transaction> Transactions;

        public static List<CompteBancaire> lireCompte(string chemin)
        {

            Comptes = new List<CompteBancaire>();
            using (StreamReader File = new StreamReader(chemin))
            {
                string line;
                while ((line = File.ReadLine()) != null)
                {
                    string[] valeur;
                    valeur = line.Split(';');
                    CompteBancaire compte = new CompteBancaire
                    {
                        Identifiant = Int32.Parse(valeur[0]),
                        TypeCompte = valeur[1],
                        Solde = decimal.Parse(valeur[3])

                    };

                    Comptes.Add(compte);
                }
            }
            return Comptes;
        }

        public static List<CarteBancaire> lireCarte(string chemin)
        {

            Cartes = new List<CarteBancaire>();
            using (StreamReader F = new StreamReader(chemin))
            {
                string line;
                while ((line = F.ReadLine()) != null)
                {
                    string[] valeur;
                    valeur = line.Split(';');
                    CarteBancaire carte = new CarteBancaire
                    {
                       NumeroCarte = valeur[0],
                       Plafond = decimal.Parse(valeur[1])
                    };

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
                    DateTime D;
                    if ((D = DateTime.ParseExact(valeur[1], "dd/MM/AAAA HH:mm:ss", CultureInfo.CurrentCulture)) != null)
                    {
                        Transaction T = new Transaction
                        {
                            NumeroTransaction = valeur[0],
                            Date = D,
                            Montant = decimal.Parse(valeur[2]),
                            CompteSource = Int32.Parse(valeur[3]),
                            CompteDestination = Int32.Parse(valeur[4])
                        };

                        Transactions.Add(T);
                    }



                }


            }
            return Transactions;
        }

        public List<Transaction> LireHistorique(string chemin)
        {
            List<Transaction> liste = null;
            foreach(CarteBancaire Carte in Cartes)
            {
                foreach (CompteBancaire cpt in Comptes)
                {
                    liste = Carte.Historique = new List<Transaction>();
                    using (StreamReader File = new StreamReader(chemin))
                    {
                        string line;
                        while ((line = File.ReadLine()) != null)
                        {
                            string[] valeur;
                            valeur = line.Split(';');
                            DateTime D;

                            if ((D = DateTime.ParseExact(valeur[1], "dd/MM/AAAA HH:mm:ss", CultureInfo.CurrentCulture)) != null)
                            {
                                if ((Int32.Parse(valeur[4])) == cpt.Identifiant || (Int32.Parse(valeur[3])) == cpt.Identifiant)
                                {
                                    Transaction T = new Transaction
                                    {
                                        NumeroTransaction = valeur[0],
                                        Date = D,
                                        Montant = decimal.Parse(valeur[2]),
                                        CompteSource = Int32.Parse(valeur[3]),
                                        CompteDestination = Int32.Parse(valeur[4])


                                    };
                                    liste.Add(T);
                                }



                            }



                        }



                    }
                }


                    
            }
              return liste;
        }
       
            
       
        public CompteBancaire TestCompte(int Id)
        {
            CompteBancaire compte = null;
            foreach (CompteBancaire C in Comptes)
            {
                if(C.Identifiant == Id)
                {
                
                 compte = new CompteBancaire
                     {
                        Identifiant = Id,
                        Solde = C.Solde,
                        TypeCompte = C.TypeCompte,
                        NumeroCarte = C.NumeroCarte
                     };

                }
            }


            return compte;
        }

        public CarteBancaire TestCarte(string Num)
        {
            CarteBancaire Carte = null;
            foreach (CarteBancaire Car in Cartes)
            {
                if (Car.NumeroCarte == Num)
                {

                    Carte = new CarteBancaire
                    {
                        NumeroCarte = Num,
                        Plafond = Car.Plafond
                     
                    };

                }
            }


            return Carte;
        }

        public bool EffectuerTransaction(Transaction t)
        {
           


            return true;
        }



    }

}


