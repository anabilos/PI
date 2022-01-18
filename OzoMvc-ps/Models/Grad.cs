using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Grad
    {
        public Grad()
        {
            Mjesto = new HashSet<Mjesto>();
        }

        public int Id { get; set; }
        [Display(Name = "Naziv grada")]
        [Required(ErrorMessage="Naziv grada je obvezno polje")]
        public string Naziv { get; set; }

        public virtual ICollection<Mjesto> Mjesto { get; set; }
    }
}
