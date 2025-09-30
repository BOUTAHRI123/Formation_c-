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
            //TODO
            return -1;
        }
    }
}
