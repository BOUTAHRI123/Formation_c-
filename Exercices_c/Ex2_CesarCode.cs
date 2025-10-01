using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie3
{
    public class Cesar
    {
        private readonly char[,] _cesarTable;

        public Cesar()
        {
            _cesarTable = new char[,]
            {
                { 'A', 'D' },
                { 'B', 'E' },
                { 'C', 'F' },
                { 'D', 'G' },
                { 'E', 'H' },
                { 'F', 'I' },
                { 'G', 'J' },
                { 'H', 'K' },
                { 'I', 'L' },
                { 'J', 'M' },
                { 'K', 'N' },
                { 'L', 'O' },
                { 'M', 'P' },
                { 'N', 'Q' },
                { 'O', 'R' },
                { 'P', 'S' },
                { 'Q', 'T' },
                { 'R', 'U' },
                { 'S', 'V' },
                { 'T', 'W' },
                { 'U', 'X' },
                { 'V', 'Y' },
                { 'W', 'Z' },
                { 'X', 'A' },
                { 'Y', 'B' },
                { 'Z', 'C' }
            };
        }

        public string CesarCode(string line)
        {
            int i;
            int j;
            StringBuilder line2 =new StringBuilder();
            Console.WriteLine("Le texte avant le décalage :");
            Console.WriteLine(line);
            foreach ( char c in line)
            {
                for (i = 0; i < _cesarTable.GetLength(0); i++)
                {
                    if (_cesarTable[i, 0] == c)
                    {
                        line2=line2.Append(_cesarTable[i, 1]);
                        //line2 =line.Replace('c', _cesarTable[i, 1]);
                    }
                }
                //line = line.Replace(line, line2);
            }
            Console.WriteLine("Le texte aprés le décalage : ");
            Console.WriteLine(line2);
            return line2.ToString(); 
        }

        public string DecryptCesarCode(string line)
        {
            int i;
            int j;
            StringBuilder line2 = new StringBuilder();
            Console.WriteLine("Le texte avant le Decryptage :");
            Console.WriteLine(line);
            foreach (char c in line)
            {
                for (i = 0; i < _cesarTable.GetLength(0); i++)
                {
                    if (_cesarTable[i, 1] == c)
                    {
                        line2 = line2.Append(_cesarTable[i, 0]);
                        //line2 =line.Replace('c', _cesarTable[i, 1]);
                    }
                }
                //line = line.Replace(line, line2);
            }
            Console.WriteLine("Le texte aprés le Dycryptage: ");
            Console.WriteLine(line2);
            return line2.ToString();
        }

        public string GeneralCesarCode(string line, int x)
        {
            int i;
            int j;
            StringBuilder line2 = new StringBuilder();
            Console.WriteLine("Le texte avant le décalage :");
            Console.WriteLine(line);
            foreach (char c in line)
            {
                for (i = 0; i < _cesarTable.GetLength(0); i++)
                {
                    if (_cesarTable[i, 0] == c)
                    {
                        int B = i + x;
                        if (B > 25)
                        {
                            
                            B -= 26;
                        }
                        line2 = line2.Append(_cesarTable[B, 0]);
                        
                    }
                }
                
            }
            Console.WriteLine("Le texte aprés le décalage : ");
            Console.WriteLine(line2);

            return line2.ToString() ;
        }

        public string GeneralDecryptCesarCode(string line, int x)
        {
            int i;
            int j;
            StringBuilder line2 = new StringBuilder();
            Console.WriteLine("Le texte avant le décalage :");
            Console.WriteLine(line);
            foreach (char c in line)
            {
                for (i = 0; i < _cesarTable.GetLength(0); i++)
                {
                    if (_cesarTable[i, 0] == c)
                    {
                        int B = i - x;
                        if (B < 0)
                        {

                            B += 26;
                        }
                        line2 = line2.Append(_cesarTable[B, 0]);

                    }
                }

            }
            Console.WriteLine("Le texte aprés le décalage : ");
            Console.WriteLine(line2);
            return line2.ToString();
        }
    }
}
