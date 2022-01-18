using System;
using System.Collections.Generic;

namespace OzoMvc.Models
{
    public partial class PosaoOprema
    {
        public int Id { get; set; }
        public int PosaoId { get; set; }
        public int OpremaId { get; set; }
        public TimeSpan? Satnica { get; set; }

        public virtual Oprema Oprema { get; set; }
        public virtual Posao Posao { get; set; }
    }
}
