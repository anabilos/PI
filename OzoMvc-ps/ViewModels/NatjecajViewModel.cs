using System;
using OzoMvc.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.ViewModels{
    public class NatjecajViewModel{
    public int Id { get; set; }
         [Required(ErrorMessage = "Potrebno je unijeti opis natjecaja")]
        [Display(Name = "Opis",Prompt = "Unesite opis najecaja")]
        public string Opis { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        [Display(Name = "TrajanjeOd")]
        [Required(ErrorMessage = "Potrebno je odabrati trajanje od natjecaja")]
        public DateTime TrajanjeOd { get; set; }
          [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        [DataType(DataType.Date)]
        [Display(Name = "TrajanjeDo")]
        [Required(ErrorMessage = "Potrebno je odabrati trajanje do natjecaja")]
        public DateTime TrajanjeDo { get; set; }
          [Required(ErrorMessage = "Potrebno je unijeti cijenu koja se nalazi u natjecaju")]
        [Display(Name = "Cijena",Prompt = "Unesite cijenu")]
        public double Cijena { get; set; }
       
        
        public string Dobiven { get; set; }
         [Required(ErrorMessage = "Potrebno je unijeti naziv usluge")]
        [Display(Name = "Naziv usluge")]
         public int UslugaId { get; set; }

        public string Naziv { get; set; }
        [Required(ErrorMessage = "Potrebno je unijeti ponudu ponuđača")]
        [Display(Name = "Ponuda ponuđača")]
        public double Ponuda { get; set; }

        public IEnumerable<StavkaViewModel> Stavke { get; set; }  =new List <StavkaViewModel>();
    }
}