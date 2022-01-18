using OzoMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OzoMvc.ViewModels
{
    public class SkladisteViewModel
    {
        public IEnumerable<Skladište> Skladište { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
