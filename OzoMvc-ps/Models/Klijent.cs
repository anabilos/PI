using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class Klijent
    {
        public Klijent()
        {
            Transakcija = new HashSet<Transakcija>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Transakcija> Transakcija { get; set; }
    }
}
