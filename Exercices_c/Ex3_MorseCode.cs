using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Serie3
{
    public class Morse
    {
        private const string Taah = "===";
        private const string Ti = "=";
        private const string Point = ".";
        private const string PointLetter = "...";
        private const string PointWord = ".....";

        private readonly Dictionary<string, char> _alphabet;

        public Morse()
        {
            _alphabet = new Dictionary<string, char>()
            {
                {$"{Ti}.{Taah}", 'A'},
                {$"{Taah}.{Ti}.{Ti}.{Ti}", 'B'},
                {$"{Taah}.{Ti}.{Taah}.{Ti}", 'C'},
                {$"{Taah}.{Ti}.{Ti}", 'D'},
                {$"{Ti}", 'E'},
                {$"{Ti}.{Ti}.{Taah}.{Ti}", 'F'},
                {$"{Taah}.{Taah}.{Ti}", 'G'},
                {$"{Ti}.{Ti}.{Ti}.{Ti}", 'H'},
                {$"{Ti}.{Ti}", 'I'},
                {$"{Ti}.{Taah}.{Taah}.{Taah}", 'J'},
                {$"{Taah}.{Ti}.{Taah}", 'K'},
                {$"{Ti}.{Taah}.{Ti}.{Ti}", 'L'},
                {$"{Taah}.{Taah}", 'M'},
                {$"{Taah}.{Ti}", 'N'},
                {$"{Taah}.{Taah}.{Taah}", 'O'},
                {$"{Ti}.{Taah}.{Taah}.{Ti}", 'P'},
                {$"{Taah}.{Taah}.{Ti}.{Taah}", 'Q'},
                {$"{Ti}.{Taah}.{Ti}", 'R'},
                {$"{Ti}.{Ti}.{Ti}", 'S'},
                {$"{Taah}", 'T'},
                {$"{Ti}.{Ti}.{Taah}", 'U'},
                {$"{Ti}.{Ti}.{Ti}.{Taah}", 'V'},
                {$"{Ti}.{Taah}.{Taah}", 'W'},
                {$"{Taah}.{Ti}.{Ti}.{Taah}", 'X'},
                {$"{Taah}.{Ti}.{Taah}.{Taah}", 'Y'},
                {$"{Taah}.{Taah}.{Ti}.{Ti}", 'Z'},
            };
        }

        public int LettersCount(string code)
        {
            int LetterCount = 0;
            string code2 = code.Replace(".......", "m"); 
            code2 = code2.Replace("...", "l");
            string[] mots = code2.Split('m');
            foreach(string mot in mots)
            {
                string[] Letter = mot.Split('l');
                LetterCount += Letter.Length;
            }
            Console.WriteLine($"le nombre des Lettres dans le code : {code} est : {LetterCount}");
            return LetterCount;
        }

        public int WordsCount(string code)
        {
            int WordsCount = 0;
            string code2 = code.Replace(".......", "m");
            string[] mots = code2.Split('m');
            WordsCount = mots.Length;
            Console.WriteLine($"le nombre des mots dans le code : {code} est : {WordsCount}");
            return WordsCount;
        }

        public string MorseTranslation(string code)
        {
            
            StringBuilder result = new StringBuilder();
            string code2 = code.Replace(".....", "m");
            code2 = code2.Replace("...", "l");
            string[] mots = code2.Split('m');
            foreach (string mot in mots)
            {
                string[] Letter = mot.Split('l');
                foreach(string c in Letter)
                {
                    if (_alphabet.ContainsKey(c))
                    {
                        result = result.Append(_alphabet[c]);
                        
                    }
                    else
                    {
                        result = result.Append('?');
                    }
                  

                }
                result = result.Append(' ');
            }
            Console.WriteLine("Traduction Morse :");
            Console.WriteLine($"{code} : {result}");
            return result.ToString();
        }

        public string EfficientMorseTranslation(string code)
        {
            StringBuilder result = new StringBuilder();
            code = code.TrimStart('.');
            code = code.TrimEnd('.');
            string code2 = Regex.Replace(code, @"\.{7,}", "m");
            //string code2 = code.Replace(".......", "m");
            code2 = Regex.Replace(code2, @"\.{3,4}", "l");
            //code2 = code2.Replace("....", "...");
            //code2 = code2.Replace("...", "l");
            code2 = code2.Replace("..", ".");
            string[] mots = code2.Split('m');
            foreach (string mot in mots)
            {
                string mot2 = mot.Trim(' ');

                string[] Letter = mot2.Split('l');
                foreach (string c in Letter)
                {
                    if (_alphabet.ContainsKey(c))
                    {
                        result = result.Append(_alphabet[c]);

                    }
                    else
                    {
                        result = result.Append('?');
                    }


                }
                result = result.Append(' ');
            }
            Console.WriteLine("Traduction Morse 2 :");
            Console.WriteLine($"{code} : {result}");
            return result.ToString();
        }

        public string MorseEncryption(string sentence)
        {
           StringBuilder result = new StringBuilder();
           int i;
           string[] mots = sentence.Split(' ');
           foreach (string mot in mots)
           {

               foreach(char c in mot)
               {
                    foreach(KeyValuePair<string,char> K in _alphabet)
                    {
                        if (c == K.Value)
                        {
                            result = result.Append(K.Key);
                        }
                    }
                    result = result.Append('.');
                    result = result.Append('.');
                }

               result = result.Append(' ');
           }
           Console.WriteLine("Traduction Morse 3 :");
           Console.WriteLine($"{sentence} : {result}");

            return result.ToString();
        }
    }
}
