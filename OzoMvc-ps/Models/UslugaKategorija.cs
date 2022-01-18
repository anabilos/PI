using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class UslugaKategorija
    {
        public int Id { get; set; }
        public int Kategorija { get; set; }
        public int UslugaId { get; set; }

        public virtual KategorijaPoslova KategorijaNavigation { get; set; }
   
        public virtual Usluga Usluga { get; set; }
    }
}
