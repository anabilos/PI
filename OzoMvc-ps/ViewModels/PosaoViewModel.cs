using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OzoMvc.Models;

namespace OzoMvc.ViewModels
{
    public class PosaoViewModel

    {


        public int Id { get; set; }
        public DateTime? Vrijeme { get; set; }
        public string UslugaNaziv { get; set; }
        public string MjestoNaziv { get; set; }

        [Display(Name = "Cijena")]
        [Required(ErrorMessage = "Cijena je obvezno polje")]
        public double Cijena { get; set; }


        [Display(Name = "Troskovi")]
        [Required(ErrorMessage = "Troskovi su obvezno polje")]
        public double Troskovi { get; set; }


        [Display(Name = "Usluga")]
        [Required(ErrorMessage = "Usluga je obvezno polje")]
        public int UslugaId { get; set; }
        public int[] ZaposlenikId { get; set; }
        public int[] OpremaId { get; set; }

        [Display(Name = "Mjesto")]
        [Required(ErrorMessage = "Mjesto je obvezno polje")]
        public int MjestoId { get; set; }

        public TimeSpan? SatnicaZaposlenika { get; set; }
        public TimeSpan? SatnicaOpreme { get; set; }
        public virtual ICollection<ZaposlenikPosao> ZaposlenikPosao { get; set; }
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
       public virtual ICollection<Zaposlenik> Zaposlenik { get; set; }
       public virtual ICollection<Oprema> Oprema { get; set; }
        public virtual Usluga UslugaNavigation { get; set; }
        public virtual Mjesto MjestoNavigation { get; set; }


    }
}