using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class KategorijaPoslova
    {
        public KategorijaPoslova()
        {
            KategorijaZaposlenik = new HashSet<KategorijaZaposlenik>();
            UslugaKategorija = new HashSet<UslugaKategorija>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<KategorijaZaposlenik> KategorijaZaposlenik { get; set; }
        public virtual ICollection<UslugaKategorija> UslugaKategorija { get; set; }
    }
}
