using System.Collections.Generic;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.ViewModels
{
    public class GradoviViewModel
    {
        public IEnumerable<GradViewModel> Gradovi { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}