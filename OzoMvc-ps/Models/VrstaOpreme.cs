using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class VrstaOpreme
    {
        public VrstaOpreme()
        {
            Oprema = new HashSet<Oprema>();
            UslugaVrstaOpreme = new HashSet<UslugaVrstaOpreme>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Oprema> Oprema { get; set; }
        public virtual ICollection<UslugaVrstaOpreme> UslugaVrstaOpreme { get; set; }
    }
}
