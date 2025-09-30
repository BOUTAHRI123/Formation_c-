using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie2
{
    public static class TasksTables
    {
        public static int SumTab(int[] tab)
        {
            int i;
            int Somme = 0;
            int cpt = 0;
            for(i = 0; i < tab.Length; i++)
            {
                Somme += tab[i];
            }
            Console.WriteLine("Somme des élements d'un tableau :");
            Console.Write("tab : [");
            for(i=0; i< tab.Length; i++)
            {
                Console.Write(tab[i]);
                if(cpt<tab.Length-1){
                    Console.Write(",");
                    cpt += 1;
                }
                
            }
            Console.Write("]");
            Console.WriteLine();
            Console.WriteLine($"Somme : {Somme}");
            return Somme;
        }

        public static int[] OpeTab(int[] tab, char ope, int b)
        {
            int i;
            int cpt = 0;
            Console.WriteLine("Opération sur un tableau : ");
            Console.Write("tab : [");
            for (i = 0; i < tab.Length; i++)
            {
                Console.Write(tab[i]);
                if (cpt < tab.Length - 1)
                {
                    Console.Write(",");
                    cpt += 1;
                }

            }
            Console.Write("]");
            Console.WriteLine();
            switch (ope)
            {
                case '+':
                    cpt = 0;
                    Console.Write($"ope : {ope} {b}");
                    Console.WriteLine();
                    Console.Write("Res : [");
                    for (i = 0; i < tab.Length; i++)
                    {
                        Console.Write(tab[i]+b);
                        if (cpt < tab.Length - 1)
                        {
                            Console.Write(",");
                            cpt += 1;
                        }
                    }
                    Console.Write("]");
                    break;
                case '*':
                    cpt = 0;
                    Console.Write($"ope : {ope} {b}");
                    Console.WriteLine();
                    Console.Write("Res : [");
                    for (i = 0; i < tab.Length; i++)
                    {
                        Console.Write(tab[i] * b);
                        if (cpt < tab.Length - 1)
                        {
                            Console.Write(",");
                            cpt += 1;
                        }
                    }
                    Console.Write("]");
                    break;
                case '-':
                    cpt = 0;
                    Console.Write($"ope : {ope} {b}");
                    Console.WriteLine();
                    Console.Write("Res : [");
                    for (i = 0; i < tab.Length; i++)
                    {
                        Console.Write(tab[i] - b);
                        if (cpt < tab.Length - 1)
                        {
                            Console.Write(",");
                            cpt += 1;
                        }
                    }
                    Console.Write("]");
                    break;
                case '/':
                    cpt = 0;
                    Console.Write($"ope : {ope} {b}");
                    Console.WriteLine();
                    Console.Write("Res : [");
                    for (i = 0; i < tab.Length; i++)
                    {
                        Console.Write(tab[i] / b);
                        if (cpt < tab.Length - 1)
                        {
                            Console.Write(",");
                            cpt += 1;
                        }
                    }
                    Console.Write("]");
                    
                    break;

            }
            Console.WriteLine();
            return null;
        }

        public static int[] ConcatTab(int[] tab1, int[] tab2)
        {
            int n = tab1.Length + tab2.Length;
            int[] T3 = new int[n];
            int i;
            int j;
            int cpt = 0;
            Console.WriteLine("Concaténation de deux tableaux : ");
            Console.Write("tab 1 : [");
            for (i = 0; i < tab1.Length; i++)
            {
                Console.Write(tab1[i]);
                if (cpt < tab1.Length - 1)
                {
                    Console.Write(",");
                    cpt += 1;
                }

            }
            Console.Write("]");
            Console.WriteLine();
            Console.Write("tab 2 : [");
            for (i = 0; i < tab2.Length; i++)
            {
                Console.Write(tab2[i]);
                cpt = 0;
                if (cpt < tab2.Length - 1)
                {
                    Console.Write(",");
                    cpt += 1;
                }

            }
            Console.Write("]");
            Console.WriteLine();
            for (i=0;i< tab1.Length; i++)
            {
                T3[i] = tab1[i];
            }
            for (j = 0; j < tab2.Length; j++)
            {
                T3[i + j] = tab2[j];
            }
            Console.Write("tab 1 + tab 2 : [");
            for (i = 0; i < T3.Length; i++)
            {
                Console.Write(T3[i]);
                cpt = 0;
                if (cpt < T3.Length - 1)
                {
                    Console.Write(",");
                    cpt += 1;
                }

            }
            Console.Write("]");
            Console.WriteLine();
            return null;
        }
    }
}
