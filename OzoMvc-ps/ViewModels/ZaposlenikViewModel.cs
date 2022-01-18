  
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.ViewModels
{
    public class ZaposlenikViewModel
    {
        public int Id { get; set; }
        public string Ime{ get; set; }
        public string Prezime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }
       public double MjesecniTrosak { get; set; }
       public string Naziv { get; set; }
    }
}