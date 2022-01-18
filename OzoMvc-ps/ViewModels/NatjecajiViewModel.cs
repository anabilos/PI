using System.Collections.Generic;



namespace OzoMvc.ViewModels
{
    public class NatjecajiViewModel
    {
        public List<NatjecajViewModel> Natjecaji { get; set; }
        
        public PagingInfo PagingInfo { get; set; }
         public NatjecajFilter Filter { get; set; }
    }
}