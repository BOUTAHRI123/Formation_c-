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
                    foreach(CarteBancaire c in Cartes)
                    {
                        if (valeur[1] == c.NumeroCarte)
                        {
                            if(valeur[2] == "Courant" || valeur[2] == "Livret")
                            {
                                if (string.IsNullOrEmpty(valeur[3]))
                                {
                                    valeur[3] = "0";
                                }
                                CompteBancaire compte = new CompteBancaire((Int32.Parse(valeur[0])), (decimal.Parse(valeur[3])), valeur[2], valeur[1]);
                                Comptes.Add(compte);
                            }
                        }
                            
                                  
                    }
                }
                   
            }
            foreach (CarteBancaire c in Cartes)
            {
                c.ComptesAssocies = CompteAssocie();
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
                    if(string.IsNullOrEmpty(valeur[1]))
                    {
                        valeur[1] = "500";
                    }

                    CarteBancaire carte = new CarteBancaire(valeur[0], decimal.Parse(valeur[1]));

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
                    if ((D = DateTime.ParseExact(valeur[1], "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture)) != null)
                    {
                        Transaction T = new Transaction(valeur[0], D, decimal.Parse(valeur[2]), Int32.Parse(valeur[3]), Int32.Parse(valeur[4]));
                        Transactions.Add(T);
                    }
                    else
                    {
                        continue;
                    }

                }

            }
            foreach(CarteBancaire c in Cartes)
            {
                c.Historique = LireHistorique();
            }
            
            return Transactions;
        }

        public List<Transaction> LireHistorique()
        {
            List<Transaction> liste = null;
            foreach(Transaction t in Transactions)
            {
                foreach (CarteBancaire carte in Cartes)
                {
                    liste = carte.Historique = new List<Transaction>();
                    foreach (CompteBancaire cpt in Comptes)
                    {
                        if (t.CompteSource == cpt.Identifiant || t.CompteDestination == cpt.Identifiant)
                        {
                                liste.Add(t);
                      
                        }

                    }


                }

            }
            return liste;
        }
        public  static List<CompteBancaire> CompteAssocie()
        {
            List<CompteBancaire> liste2 = null;
            foreach (CompteBancaire cpt in Comptes)
            {
                foreach (CarteBancaire carte in Cartes)
                {
                    liste2 = carte.ComptesAssocies = new List<CompteBancaire>();
                   
                        if (carte.NumeroCarte == cpt.NumeroCarte)
                        {
                            liste2.Add(cpt);

                        }
                   
                }

            }
            return liste2;
        }



        public CompteBancaire TestCompte(int Id)
        {
            CompteBancaire compte = null;
            foreach (CompteBancaire C in Comptes)
            {
                if(C.Identifiant == Id)
                {

                    compte = C;

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
            if (t.CompteSource == 0)
            {
                destination = t.CompteDestination;
                
                if((cpt = TestCompte(destination)) != null){

                    statut = cpt.DepotArgent(t.Montant);
                }
                /*foreach ( CompteBancaire cpt in Comptes)
                {
                    if(cpt.Identifiant == destination)
                    {
                        statut = cpt.DepotArgent(t.Montant);
                    }
                }*/
                
            }
            else if(t.CompteDestination == 0)
            {
                source = t.CompteSource;

                if ((cpt = TestCompte(source)) != null)
                {
                    if((carte = TestCarte(cpt.NumeroCarte)) != null)
                    {            
                    
                       
                       bool sttransaction = carte.PeutEffectuer(t.Montant, t.Date);
                       if (sttransaction)
                       {
                           statut = cpt.RetireArgent(t.Montant);
                       }
                            
                        
                    }
                }
                
            }
            else
            {
                source = t.CompteSource;
                destination = t.CompteDestination;
                CompteBancaire cpt2;
                if ((cpt = TestCompte(source)) != null )
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
                        sortie.WriteLine($"{(t.NumeroTransaction)};OK");
                       
                    }
                    else
                    {
                        sortie.WriteLine($"{(t.NumeroTransaction)};KO");
                    }
                }
                              
            }

            Console.WriteLine("C'est bien fait!");
        }



    }

}


