using System.Collections.Generic;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.ViewModels
{
    public class MjestaViewModel
    {
        public IEnumerable<MjestoViewModel> Mjesta { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}