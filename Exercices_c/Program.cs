using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serie2;
using Serie3;
using Serie4;


namespace Serie1
{
    class Program
    {
        public static void Main(string[] args)
        {
            int a ;
            int b ;
            char Operation;
            ElementaryOperations.BasicOperation(3,4,'+');
            ElementaryOperations.BasicOperation(5, 9, '*');
            ElementaryOperations.BasicOperation(30, 0, '/');
            ElementaryOperations.BasicOperation(3, 4, 'M');
            ElementaryOperations.BasicOperation(7, 4, '-');
            ElementaryOperations.BasicOperation(30, 5, '/');
            ElementaryOperations.IntegerDivision(12, 5);
            ElementaryOperations.IntegerDivision(20,5);
            ElementaryOperations.IntegerDivision(10, 0);
            ElementaryOperations.Pow(3, 2);
            ElementaryOperations.Pow(3, -1);
            SpeakingClock.GoodDay(12);
            SpeakingClock.GoodDay(24);
            SpeakingClock.GoodDay(8);
            SpeakingClock.GoodDay(16);
            SpeakingClock.GoodDay(21);
            Pyramid.PyramidConstruction(10, true);
            Pyramid.PyramidConstruction(10, false);
            Factorial.Factorial_(3);
            Factorial.Factorial_(10);
            Factorial.Factorial_(0);
            Factorial.FactorialRecursive(3);
            Factorial.FactorialRecursive(0);
            int[] T1 = new int[] { 2, 4, 8, 9, 3, 10};
            int[] T2 = new int[] { -2, 6, 25, 7, 13, -5 };
            TasksTables.SumTab(T1);
            TasksTables.OpeTab(T1, '*', 3);
            TasksTables.OpeTab(T2, '+', 10);
            TasksTables.ConcatTab(T1, T2);
            char[,] T4  = {{'X','O','X'}, {'O','X','O'}, {'X','O','O'}};
            char[,] T5 = { { ' ', 'O', 'X' }, { 'O', 'X', 'O' }, { 'X', 'O', 'O' } };
            int[] T6 = new int[] { -5, -2, 6, 7, 13, 25 };
            Morpion.DisplayMorpion(T4);
            Morpion.CheckMorpion(T4);
            Morpion.DisplayMorpion(T5);
            Morpion.CheckMorpion(T5);
            Search.LinearSearch(T2, 7);
            Search.LinearSearch(T2, 67);
            Search.BinarySearch(T6, 13);
            Search.BinarySearch(T6, 10);
            String[] mots = new string[] { "dollars", "Reagan", "Afghanistan", "ouest", "crime", "défaite" };
            String text = "je dois aller à l'ouest";
            String text2 = "ou as-tu caché mes dollars? Je dois aller à l'ouest! L'armée m'appelle pour aller en Afghanistan .";
            AdministrativeTasks.EliminateSeditiousThoughts(text, mots);
            AdministrativeTasks.EliminateSeditiousThoughts(text2, mots);
            String Line = "[M. Plenko Andrej 24]";
            String Line2 = "[Mr Plenko Andrej 08]";
            AdministrativeTasks.ControlFormat(Line);
            AdministrativeTasks.ControlFormat(Line2);
            String Date = "1982-10-09";
            AdministrativeTasks.ChangeDate(Date);
            String l = "ABCDEFGHIJKLMZ";
            Cesar C = new Cesar();
            String L2=C.CesarCode(l);
            C.DecryptCesarCode(L2);
            String L3=C.GeneralCesarCode(l, 3);
            C.GeneralDecryptCesarCode(L3, 3);
            String code = "S...H...A...M...E.......B...O...U...T...A...H...R...I";
            Morse morse = new Morse();
            morse.LettersCount(code);
            morse.WordsCount(code);
            String Code2 = "===.=.===.=...===.===.===...===.=.=...=.....===.===...===.===.===...=.===.=...=.=.=...=";
            morse.MorseTranslation(Code2);
            String Code3 = "===.=.===.=...===.===.===...===.=.=....=........===.===...===..===..===...=.===.=...=.=.=...=";
            morse.EfficientMorseTranslation(Code3);
            morse.MorseEncryption("CODE");
            String filePath = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\\fichier-Note.csv";
            String fileout = @"C:\Users\Formation\Desktop\formation_c#\FormationCSharp\\Notes.csv";
            ClassCouncil.SchoolMeans(filePath, fileout);
            char[,] tab = { { '_', '_', '_' }, { '_', '_', '_' }, { '_', '_', '_' } };
            Console.ReadKey();

        }

        public static int Addition(int a, int b)
        {
            return a + b;
            
        }


    }
}
