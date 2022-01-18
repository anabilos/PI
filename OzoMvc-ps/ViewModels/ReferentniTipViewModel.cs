using OzoMvc.Models;
using OzoMvc.ViewModels;
using System.Collections.Generic;

namespace OzoMvc.ViewModels
{
    public class ReferentniTipViewModel
    {
        public IEnumerable<ReferentniTip> ReferentniTip { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}