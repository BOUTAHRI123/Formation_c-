using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Or.Models
{
    [XmlRoot]
    public class ExportCompteTransacctions
    {
        [XmlElement("Identifiant")]
        public int IdTransaction { get; set; }

        [XmlElement("Date")]
        public DateTime Horodatage { get; set; }

        [XmlElement("Montant")]
        public decimal Montant { get; set; }

        [XmlElement("Operation")]
        public string TypeTransaction { get; set; }

        [XmlElement("CompteExpediteur")]
        public int Expediteur { get; set; }

        [XmlElement("CompteDestinataire")]
        public int Destinataire { get; set; }

    }
}
