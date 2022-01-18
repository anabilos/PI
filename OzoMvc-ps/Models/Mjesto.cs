using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Mjesto
    {
       
        public int Id { get; set; }

        [Display(Name = "Naziv mjesta")]
        [Required(ErrorMessage="Naziv mjesta je obvezno polje")]
        public string Naziv { get; set; }

        [Display(Name = "Grad u kojem se mjesto nalazi", Prompt = "Odaberite grad u kojem se mjesto nalazi")]
        [Required(ErrorMessage="Grad u kojem se mjesto nalazi je obvezno polje")]
        public int GradId { get; set; }
         public virtual Grad Grad { get; set; }
        public virtual ICollection<Skladište> Skladište { get; set; }
        public virtual ICollection<Zaposlenik> Zaposlenik { get; set; }
    }
}
