using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class UslugaVrstaOpreme
    {
        public int Id { get; set; }
        public int UslugaId { get; set; }
        public int VrstaId { get; set; }

        public virtual Usluga Usluga { get; set; }
        public virtual VrstaOpreme Vrsta { get; set; }
    }
}
