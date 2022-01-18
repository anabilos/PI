using System.Collections.Generic;
using OzoMvc.ViewModels;


namespace OzoMvc.ViewModels
{
    public class UslugeViewModel
    {
        public List<UslugaViewModel> Usluge { get; set; }
        public PagingInfo PagingInfo { get; set; }
         public UslugaFilter Filter { get; set; }
    }
}