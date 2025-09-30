using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie1
{
    public static class SpeakingClock
    {
        
        public static string GoodDay(int heure)
        {
            String message ="";

            switch (heure)
            {
                case int n when n >= 23 || n < 6:
                    message = " Merveilleuse nuit !";
                    break;

                case int n when n >= 6 && n < 12:
                    message = " Bonne matinée!";
                    break;

                case 12:
                    message = " Bon appétit !";
                    break;

                case int n when n >=13 && n < 18:
                    message = " Profitez de votre aprés midi !";
                    break;
                case int n when n >= 18 && n < 23:
                    message = " Passez une bonne soirée!";
                    break;
            }
            Console.WriteLine($" Il est {heure} H, {message}");
            return string.Empty;
        }
    }
}
