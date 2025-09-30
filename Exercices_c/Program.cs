using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.ReadKey();

        }

        public static int Addition(int a, int b)
        {
            return a + b;
            
        }


    }
}
