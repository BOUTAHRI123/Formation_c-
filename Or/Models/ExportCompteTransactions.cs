using System;
using System.Xml.Serialization;

namespace Or.Models
{
    [XmlRoot]
    public class ExportCompteTransacctions
    {
        [XmlElement("Identifiant")]
        public int IdTransaction { get; set; }

        [XmlIgnore]
        public DateTime? Horodatage { get; set; }
        [XmlElement("Date")]
        public string DateSting
        {
            get
            {
                return Horodatage.HasValue ? Horodatage.Value.ToString("dd/MM/yyyy HH:mm:ss") : null;
            }
            set
            {
                Horodatage = string.IsNullOrEmpty(value)
                ? (DateTime?)null
                : DateTime.Parse(value);
            }
        }

        
        [XmlIgnore]
        public decimal Montant { get; set; }
        [XmlElement("Montant")]
        public string Montantstring
        {
            get
            { return Montant.ToString("C2"); }

            set
            {
                if (decimal.TryParse(value, out decimal m))
                {
                    Montant = m;
                }
            }

        }

        
        [XmlIgnore]
        public int Expediteur { get; set; }
        [XmlElement("CompteExpediteur")]
        public int? ExpediteurXml
        {
            get { return Expediteur == 0 ? (int?)null : Expediteur; }
            set { Expediteur = value ?? 0; }
        }

        
        [XmlIgnore]
        public int Destinataire { get; set; }
        [XmlElement("CompteDestinataire")]
        public int? DestinataireXml
        {
            get { return Destinataire == 0 ? (int?)null : Destinataire; }
            set { Destinataire = value ?? 0; }
        }

        [XmlElement("Operation")]
        public string TypeTransaction { get; set; }

    }
}
