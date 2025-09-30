using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Serie1
{
    public static class ElementaryOperations
    {
        public static void BasicOperation(int a, int b, char operation)
        {
            string resultat;
            switch (operation)
            {
                case '/':
                    if (b != 0)
                    {
                        resultat = (a / b).ToString();

                    }
                    else
                    {
                        resultat = "Opération invalide";
                        
                    }
                    break;

                case '*':                  
                        resultat = (a * b).ToString();
                        
                    break;

                case '+':
                    resultat = (a + b).ToString();
                    break;

                case '-':
                    resultat = (a - b).ToString();
                    break;

                
                default:
                    resultat = "Opération invalide";
                    break;


            }

            Console.WriteLine($"{a}  {operation}  {b} = {resultat}");


        }

        public static void IntegerDivision(int a, int b)
        {
            int q;
            int r;
            string resultat = "Opération invalide";
            if(b != 0)
            {
                q = a / b;
                r = a % b;
                if(r != 0)
                {
                    Console.WriteLine($"{a} = {q} * {b} + {r}");
                }
                else
                {
                    Console.WriteLine($"{a} = {q} * {b}");
                } 

            }
            else
            {
                Console.WriteLine($"{a} / {b} = {resultat}");
            }
            

        }

        public static void Pow(int a, int b)
        {
            string resultat;
            if (b < 0)
            {
                resultat = "Opération invalise";
            }
            else
            {
                resultat = (Math.Pow(a,b)).ToString();

            }
            Console.WriteLine($"{a} ^ {b} = {resultat}");
        }
    }
}
