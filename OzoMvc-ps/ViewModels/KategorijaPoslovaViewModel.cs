using System.Collections.Generic;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.ViewModels
{
    public class KategorijaPoslovaViewModel
    {
        public IEnumerable<KategorijaPoslova> kategorijaposlova { get; set; }
        public PagingInfo PagingInfo { get; set; }

       
    }
}