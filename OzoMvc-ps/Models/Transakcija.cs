using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class Transakcija
    {
        public int Id { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public int OpremaId { get; set; }
        public int KlijentId { get; set; }

        public virtual Klijent Klijent { get; set; }
        public virtual Oprema Oprema { get; set; }
    }
}
