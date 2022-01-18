using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OzoMvc.Models
{
    public partial class Posao
    {
        public Posao()
        {
            PosaoOprema = new HashSet<PosaoOprema>();
            ZaposlenikPosao = new HashSet<ZaposlenikPosao>();
        }

  
        public int Id { get; set; }
      
        public DateTime? Vrijeme { get; set; }
        
      
        public int UslugaId { get; set; }


        public int MjestoId { get; set; }


    
        public double Cijena { get; set; }
       
        public double Troskovi { get; set; }

        public virtual Usluga UslugaNavigation { get; set; }
        public virtual Mjesto MjestoNavigation { get; set; }
             
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
   
        public virtual ICollection<ZaposlenikPosao> ZaposlenikPosao { get; set; }
    }
}
