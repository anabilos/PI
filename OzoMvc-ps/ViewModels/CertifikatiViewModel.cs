using System.Collections.Generic;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.ViewModels
{
    public class CertifikatiViewModel
    {
        public IEnumerable<Certifikati> certifikati { get; set; }
        public PagingInfo PagingInfo { get; set; }

       
    }
}