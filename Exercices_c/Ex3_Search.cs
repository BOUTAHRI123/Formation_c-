using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie2
{
    public static class Search
    {
        public static int LinearSearch(int[] tableau, int valeur)
        {
            int i;
            int index = -1;
            for (i = 0; i < tableau.Length; i++)
            {
                if (tableau[i] == valeur)
                {
                    index = i;
                }
                
            }
            if(index != -1)
            {
                Console.WriteLine($"L'indice est : {index} , Valeur trouvé ");
            }
            else
            {
                Console.WriteLine($"L'indice est : {index} , Valeur introuvable !");
            }
            
            return index;
        }

        public static int BinarySearch(int[] tableau, int valeur)
        {
            int debut = 0;
            int fin = (tableau.Length)-1;
            int index = -1;
            while(debut <= fin)
            {
                decimal m = (debut + fin) / 2;
                int milieu = (int)m;
                if(tableau[milieu] == valeur)
                    {
                    index = milieu;
                    Console.WriteLine($"L'indice est : {index} , Valeur trouvé ");
                    return index;
                    }
                    else if(tableau[milieu] < valeur){

                          debut = milieu + 1;
                    }
                    else if(tableau[milieu] > valeur)
                    {
                          fin = milieu - 1;

                    }
               
            }
            
            if (index == -1)
            {
            
                Console.WriteLine($"L'indice est : {index} , Valeur introuvable !");
            }

            return index;

        }
    }
}
