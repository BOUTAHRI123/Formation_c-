using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Or.Models
{
    [XmlRoot]
    public class ExportCompte
    {
        [XmlElement("ID")]
        public int Id  { get; set; }

        [XmlElement("Type")]
        public TypeCompte TypeDuCompte { get; set; }

        [XmlElement("Solde")]
        public string Solde { get; set; }

        [XmlArray("ListeTransactions")]
        [XmlArrayItem("Transaction", typeof(ExportCompteTransacctions))]
        public List<ExportCompteTransacctions> ListeTransactions { get; set; }

        public ExportCompte()
        {

        }

    }
}
