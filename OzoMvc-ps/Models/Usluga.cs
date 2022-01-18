using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class Usluga
    {
        public Usluga()
        {
            Natjecaj = new HashSet<Natjecaj>();
            Posao = new HashSet<Posao>();
            UslugaKategorija = new HashSet<UslugaKategorija>();
            UslugaVrstaOpreme = new HashSet<UslugaVrstaOpreme>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public int NormativId { get; set; }

        public virtual Normativi Normativ { get; set; }
        public virtual ICollection<Natjecaj> Natjecaj { get; set; }
        public virtual ICollection<Posao> Posao { get; set; }
        public virtual ICollection<UslugaKategorija> UslugaKategorija { get; set; }
        public virtual ICollection<UslugaVrstaOpreme> UslugaVrstaOpreme { get; set; }
    }
}
