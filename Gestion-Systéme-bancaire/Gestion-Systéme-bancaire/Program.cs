using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_Systéme_bancaire
{
    class Program
    {
        static void Main(string[] args)
        {
            string fichierCarte = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\Gestion-Systéme-bancaire\Gestion-Systéme-bancaire\\Cartes.csv";
            string fichierCompte = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\Gestion-Systéme-bancaire\Gestion-Systéme-bancaire\\Comptes.csv";
            string fichierTransaction = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\Gestion-Systéme-bancaire\Gestion-Systéme-bancaire\\Transactions.csv";
            string fichierSortie = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\Gestion-Systéme-bancaire\Gestion-Systéme-bancaire\\Sortieresultat.csv";
            Banque Banque = new Banque();
            Banque.Cartes = Banque.LireCarte(fichierCarte);
            Banque.Comptes = Banque.LireCompte(fichierCompte);
            Banque.Transactions = Banque.LireTransaction(fichierTransaction);
           
            
            Banque.EcritSortie(fichierSortie);

        }
    }
}
