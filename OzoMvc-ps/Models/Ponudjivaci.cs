using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Ponudjivaci
    {
         public Ponudjivaci()
        {
            NatjecajPonudjivac = new HashSet<NatjecajPonudjivac>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Potrebno je unijeti naziv ponuđivača")]
        [Display(Name = "Naziv",Prompt = "Unesite naziv ponuđivača")]
         public string Naziv { get; set; }
         public virtual ICollection<NatjecajPonudjivac> NatjecajPonudjivac { get; set; }
    }
}
