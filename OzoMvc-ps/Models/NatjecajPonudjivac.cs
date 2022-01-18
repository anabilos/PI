using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class NatjecajPonudjivac
    {
        public int Id { get; set; }
        public int NatjecajId { get; set; }
        [Required(ErrorMessage = "Potrebno je unijeti ponuđivača")]
        [Display(Name = "Ponuđivač",Prompt = "Unesite ponuđivača")]
        public int PonudjivacId { get; set; }
        [Required(ErrorMessage = "Potrebno je unijeti cijenu ponude")]
        [Display(Name = "Cijena ponude",Prompt = "Unesite cijenu ponude")]
        public double CijenaPonude{ get; set;}
    
        public bool DobivenNatjecaj { get; set; }

        public virtual Natjecaj Natjecaj { get; set; }
        public virtual Ponudjivaci Ponudjivac { get; set; }
    }
}