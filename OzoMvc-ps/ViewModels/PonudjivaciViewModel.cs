using System.Collections.Generic;
using OzoMvc.Models;


namespace OzoMvc.ViewModels
{
    public class PonudjivaciViewModel
    {
        public IEnumerable<Ponudjivaci> ponudjivaci{ get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}