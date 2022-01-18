using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class ReferentniTip
    {
        public ReferentniTip()
        {
            Oprema = new HashSet<Oprema>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public int? Cijena { get; set; }

        public virtual ICollection<Oprema> Oprema { get; set; }
    }
}
