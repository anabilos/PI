
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Oprema
    {
        public Oprema()
        {
            PosaoOprema = new HashSet<PosaoOprema>();
            Transakcija = new HashSet<Transakcija>();
        }

        public int InventarniBroj { get; set; }

        [Display(Name = "Naziv opreme", Prompt ="Unesite anziv")]
        public string Naziv { get; set; }
        [Display(Name = "Kupovna Vrijednost opreme")]
        public double? KupovnaVrijednost { get; set; }
        [Display(Name = "Knjigovodstvena vrijednsot opreme")]
        public double? KnjigovodstvenaVrijednost { get; set; }
        [Display(Name = "Vrijeme amortizacije opreme")]
        public double VrijemeAmortizacije { get; set; }
        [Display(Name = "Vrsta opreme")]
        public int VrstaId { get; set; }
        [Display(Name = "Referentni tip opreme")]
        public int ReferentniId { get; set; }

        [Display(Name = "Skladiste opreme")]
        public int SkladisteId { get; set; }

        public virtual ReferentniTip Referentni { get; set; }
        public virtual Skladište Skladiste { get; set; }
        public virtual VrstaOpreme Vrsta { get; set; }
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
        public virtual ICollection<Transakcija> Transakcija { get; set; }
    }
}
