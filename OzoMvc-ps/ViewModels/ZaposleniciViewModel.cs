using System.Collections.Generic;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.ViewModels
{
    public class ZaposleniciViewModel
    {
        public IEnumerable<ZaposlenikViewModel> Zaposlenici { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public ZaposlenikFilter Filter{ get; set;}
    }
}