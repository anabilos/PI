using OzoMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OzoMvc.ViewModels
{
    public class PosloviViewModel
    {
        
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
        public virtual ICollection<ZaposlenikPosao> ZaposlenikPosao { get; set; }
        public IEnumerable<PosaoViewModel> Poslovi { get; set; }
        public PosaoFilter Filter { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}

