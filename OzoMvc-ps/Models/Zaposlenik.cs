using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Zaposlenik
    {
      
        [Display(Name = "Id zaposlenika")]
        [Required(ErrorMessage="Oznaka zaposlenika je obvezno polje")]
        public int Id { get; set; }

        [Display(Name = "Ime zaposlenika", Prompt = "Unesite ime zaposlenika")]
        [Required(ErrorMessage="Ime zaposlenika je obvezno polje")]
        public string Ime { get; set; }

        [Display(Name = "Prezime zaposlenika", Prompt = "Unesite prezime zaposlenika")]
        [Required(ErrorMessage="Prezime zaposlenika je obvezno polje")]
        public string Prezime { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        [Display(Name = "Datum rođenja", Prompt = "Odaberite datum rođenja zaposlenika")]
        [Required(ErrorMessage="Datum rođenja je obavezno polje")]
        public DateTime DatumRodjenja { get; set; }

        [Display(Name = "Mjesečni trošak", Prompt = "Unesite mjesečni trošak zaposlenika")]
        [Required(ErrorMessage="Mjesečni trošak zaposlenika je obvezno polje")]
        public double MjesecniTrosak { get; set; }

        [Display(Name = "Mjesto stanovanja zaposlenika", Prompt = "Odaberite mjesto stanovanja stanovnika")]
        [Required(ErrorMessage="Mjesto stanovanja je obavezno polje")]
        public int MjestoId { get; set; }

        public virtual Mjesto Mjesto { get; set; }
   
        public virtual ICollection<CertifikatZaposlenik> CertifikatZaposlenik { get; set; }
        public virtual ICollection<KategorijaZaposlenik> KategorijaZaposlenik { get; set; }
        public virtual ICollection<ZaposlenikPosao> ZaposlenikPosao { get; set; }
    }
}
