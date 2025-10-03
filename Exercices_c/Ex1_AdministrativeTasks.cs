using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie3
{
    public static class AdministrativeTasks
    {
        public static string EliminateSeditiousThoughts(string text, string[] prohibitedTerms)
        {
            // Afficher le fichier d'entrée 
            Console.WriteLine($"le texte d'entrée est : {text}");
            // declaration du StringBuilder afin de stocker le mot en sortie du code 
            StringBuilder mot2 = new StringBuilder();
            string textM = " ";
            string m;
            int i;
            // Parcouris tous les mots afin de verfier si le mot existe dans mon texte ou pas 
            foreach(string mot in prohibitedTerms)
            {
                // Initialiser le mot codée
                mot2.Clear();
                m = mot;
                if (text.Contains(m))
                {
                    for (i = 0; i < m.Length; i++)
                    {
                        // Ajouter les X au mot2 jusqu'on attend la longeur du mot 
                        mot2.Append("X");
                    }
                    // remplacer le mot dans le texte par le mot codée et stocker le resultat dans textM
                    textM=text.Replace(m, mot2.ToString());
                    // reprendre le texte d'entrée par le texte modifié
                    text=text.Replace(text, textM);
                }
                

            }
            Console.WriteLine($"Le texte de sortie est : {textM}");
            return textM;
        }

        public static bool ControlFormat(string line)
        {
            string format = "OK";
            bool Statut = true;
            if (!line.StartsWith("[") || !line.EndsWith("]"))
            {
                format = "KO";
                Statut = false;
                

            }
                
            string content = line.Substring(1, line.Length - 2);
            string[] parts = content.Split(' ');

            if (parts.Length != 4)
            {
                format = "KO";
                Statut = false;

            }
                
            string civilite = parts[0];
            string nom = parts[1];
            string prenom = parts[2];
            string ageStr = parts[3];

            // Vérifier civilité
            if (civilite != "M." && civilite != "Mme" && civilite != "Mlle")
            {
                format = "KO";
                Statut = false;
            }
                
            // Vérifier nom et prénom : lettres
            foreach(char c1 in nom)
            {
                if (!char.IsLetter(c1) || nom.Length > 12)
                {
                    format = "KO";
                    Statut = false;
                }

            }
            foreach (char c2 in prenom)
            {
                if (!char.IsLetter(c2) || prenom.Length > 12)
                {
                    format = "KO";
                    Statut = false;
                }
                    

            }

            // Vérifier âge : 2 chiffres
            if (ageStr.Length != 2 || !int.TryParse(ageStr, out int age))
            {
                format = "KO";
                Statut = false;
            }
            Console.WriteLine($"Line : {line}");
            Console.WriteLine($"Format : {format}");
            return Statut;
        }

        public static string ChangeDate(string report)
        {
            string[] parts = report.Split('-');
            string Ann = parts[0];
            string Mois = parts[1];
            string Jour = parts[2];
            string A = Ann.Substring(2,2);
            Console.WriteLine($"La date en entrée est : {report}");
            Console.WriteLine($"La date en sortie  est : {Jour}.{Mois}.{A}");

            return string.Empty;
        }
    }
}
