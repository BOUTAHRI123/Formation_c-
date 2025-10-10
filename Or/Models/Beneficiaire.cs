using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Or.Models
{
    public class Beneficiaire
    {
        public int IdBenef { get; set; }
        public int IdCompte { get; set; }
        public int IdCarte { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }


    }
}
