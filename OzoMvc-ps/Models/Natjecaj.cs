using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace OzoMvc.Models
{
    public partial class Natjecaj
    {
        public Natjecaj()
        {
            NatjecajPonudjivac = new HashSet<NatjecajPonudjivac>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Potrebno je unijeti opis natjecaja")]
        [Display(Name = "Opis",Prompt = "Unesite opis najecaja")]
        public string Opis { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
         [Required(ErrorMessage = "Potrebno je unijeti trajanje_od")]
        [Display(Name = "Trajanje Od",Prompt = "Unesite trajanje od")]
        public DateTime TrajanjeOd { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
         [Required(ErrorMessage = "Potrebno je unijeti trajanje_do")]
        [Display(Name = "Trajanje Do",Prompt = "Unesite trajanje do")]
        public DateTime TrajanjeDo { get; set; }
         [Required(ErrorMessage = "Potrebno je unijeti cijenu koja se nalazi u natjecaju")]
        [Display(Name = "Cijena",Prompt = "Unesite cijenu")]
        public double Cijena { get; set; }
         [Required(ErrorMessage = "Potrebno je unijeti naziv usluge")]
        [Display(Name = "Naziv usluge")]
        public int UslugaId { get; set; }

        public virtual Usluga Usluga { get; set; }
        public virtual ICollection<NatjecajPonudjivac> NatjecajPonudjivac { get; set; }
    }
}