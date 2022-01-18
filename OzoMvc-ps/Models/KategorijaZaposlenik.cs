using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class KategorijaZaposlenik
    {
        public int Id { get; set; }
        public int ZaposlenikId { get; set; }
        public int KategorijaId { get; set; }

        public virtual KategorijaPoslova Kategorija { get; set; }
        public virtual Zaposlenik Zaposlenik { get; set; }
    }
}
