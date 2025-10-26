using System.Collections.Generic;
using System.Xml.Serialization;


namespace Or.Models
{

    [XmlType("Comptes")]
    public class ExportComptes
    {
        [XmlElement("Compte")]
        public List<ExportCompte> ListeComptes = new List<ExportCompte>();
    }

    [XmlType("Compte")]
    public class ExportCompte
    {
        [XmlElement("Identifiant")]
        public int Id  { get; set; }

        [XmlElement("Type")]
        public TypeCompte TypeDuCompte { get; set; }

        [XmlElement("Solde")]
        public string Solde { get; set; }

        [XmlArray("Transactions")]
        [XmlArrayItem("Transaction", typeof(ExportCompteTransacctions))]
        public List<ExportCompteTransacctions> ListeTransactions { get; set; }

        public ExportCompte()
        {

        }

    }
}
