using System;
using Or.Business;
using System.Xml.Serialization;

namespace Or.Models
{
    [XmlType ("Transaction")]
    public class Transaction
    {
        public int IdTransaction { get; set; }

        [XmlIgnore]
        public DateTime? Horodatage { get; set; }
        [XmlElement("Date", Order = 4)]
        public string DateSting
        {
            get
            {
                return Horodatage.HasValue ? Horodatage.Value.ToString("dd/mm/yyyy HH:mm:ss") : null;
            }
            set
            {
                Horodatage = string.IsNullOrEmpty(value)
                ? (DateTime?)null
                : DateTime.Parse(value);
            }
        }
        public decimal Montant { get; set; }
        public int Expediteur { get; set; }
        public int Destinataire { get; set; }
        public string TypeTransaction { get; set; }

        public Transaction(int idTransaction, DateTime horodatage, decimal montant, int expediteur, int destinataire)
        {
            IdTransaction = idTransaction;
            Horodatage = horodatage;
            Montant = montant;
            Expediteur = expediteur;
            Destinataire = destinataire;
            TypeTransaction = Tools.TypeTransacConverter(Tools.TypeTransaction(expediteur, destinataire));
        }

      
    }
}
