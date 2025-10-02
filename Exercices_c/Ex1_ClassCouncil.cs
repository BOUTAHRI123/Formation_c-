using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie4
{
    public static class ClassCouncil
    {
        public static void SchoolMeans(string input, string output)
        {
            decimal SommeMath = 0;
            decimal SommeHistoire = 0;
            int cpt1 = 0;
            int cpt2 =0;
            //using (FileStream file = new FileStream("fichier-Note.csv", FileMode.Open)) ;
            using (StreamReader filee = new StreamReader(input))
            {
               
                string line;
                while((line= filee.ReadLine()) != null)
                {
                    string[] values = line.Split(';');
                    if (values[1] == "Maths")
                    {
                        SommeMath += Convert.ToDecimal(values[2]);
                        cpt1++;

                    }
                    else if(values[1]=="Histoire")
                    {
                        SommeHistoire += Convert.ToDecimal(values[2]);
                        cpt2++;

                    }
                }
                decimal moy1 = SommeMath / cpt1;
                decimal moy2 = SommeHistoire / cpt2;
                using (StreamWriter F = new StreamWriter(output))
                {
                    F.WriteLine("Matiere;Moyenne");
                    F.Write("Histoire;");
                    F.WriteLine(moy2);
                    F.Write("Maths;");
                    F.Write(moy1);
                }
            }

            Console.WriteLine("tout à fait!");  
        }
    }
}
