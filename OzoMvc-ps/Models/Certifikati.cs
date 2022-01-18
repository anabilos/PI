using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class Certifikati
    {
        public Certifikati()
        {
            CertifikatZaposlenik = new HashSet<CertifikatZaposlenik>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<CertifikatZaposlenik> CertifikatZaposlenik { get; set; }
    }
}
