using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class ZaposlenikPosao
    {
       

      

        public int Id { get; set; }
        public int ZaposlenikId { get; set; }
        public int PosaoId { get; set; }
        public TimeSpan? Satnica { get; set; }

        public virtual Posao Posao { get; set; }
        public virtual Zaposlenik ZaposlenikNavigation { get; set; }
    }
}
