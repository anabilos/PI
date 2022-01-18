  
using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class Normativi
    {
        public Normativi()
        {
            Usluga = new HashSet<Usluga>();
        }

        public int Id { get; set; }
        public string Opis { get; set; }

        public virtual ICollection<Usluga> Usluga { get; set; }
    }
}
