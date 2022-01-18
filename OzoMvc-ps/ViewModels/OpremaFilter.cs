using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using OzoMvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OzoMvc.ViewModels
{
    public class OpremaFilter : IPageFilter
    {
        public int InventarniBroj { get; set; }
        public string Naziv { get; set; }
        public double? KupovnaVrijednostOd { get; set; }
        public double? KupovnaVrijednostDo { get; set; }
        public double? KnjigovodstvenaVrijednostOd { get; set; }
        public double? KnjigovodstvenaVrijednostDo { get; set; }
        public double? VrijemeAmortizacije { get; set; }
        public int VrstaId { get; set; }
        public int ReferentniId { get; set; }
        public int SkladisteId { get; set; }
        public virtual ReferentniTip Referentni { get; set; }
        public virtual Skladište Skladiste { get; set; }
        public virtual VrstaOpreme Vrsta { get; set; }
        public virtual ICollection<PosaoOprema> PosaoOprema { get; set; }
        public virtual ICollection<Transakcija> Transakcija { get; set; }


        public bool IsEmpty()
        {
            bool active =
                         KupovnaVrijednostOd.HasValue ||
                         KupovnaVrijednostOd.HasValue
                        || KnjigovodstvenaVrijednostDo.HasValue
                        || KnjigovodstvenaVrijednostOd.HasValue;
                       



            return !active;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}",
              
                KupovnaVrijednostOd,
                KupovnaVrijednostDo ,
                KnjigovodstvenaVrijednostOd,
                KnjigovodstvenaVrijednostDo
              
            );
        }

        public static OpremaFilter FromString(string s, ILogger logger = null)
        {
            var filter = new OpremaFilter();
            try
            {
                var arr = s.Split('-', StringSplitOptions.None);
              
                filter.KupovnaVrijednostOd = string.IsNullOrWhiteSpace(arr[0]) ? new double?() : double.Parse(arr[0]);
                filter.KupovnaVrijednostDo = string.IsNullOrWhiteSpace(arr[1]) ? new double?() : double.Parse(arr[1]);
                filter.KnjigovodstvenaVrijednostOd = string.IsNullOrWhiteSpace(arr[2]) ? new double?() : double.Parse(arr[2]);
                filter.KnjigovodstvenaVrijednostDo = string.IsNullOrWhiteSpace(arr[3]) ? new double?() : double.Parse(arr[3]);
               


            }
            catch { }
            return filter;
        }
        public IQueryable<Oprema> Apply(IQueryable<Oprema> query)
        {
            if (KupovnaVrijednostOd.HasValue)
            {
                query = query.Where(z => z.KupovnaVrijednost >= KupovnaVrijednostOd.Value);
            }
            if (KupovnaVrijednostDo.HasValue)
            {
                query = query.Where(z => z.KupovnaVrijednost <= KupovnaVrijednostDo.Value);
            }
            if (KnjigovodstvenaVrijednostOd.HasValue)
            {
                query = query.Where(z => z.KnjigovodstvenaVrijednost >= KnjigovodstvenaVrijednostOd.Value);
            }
            if (KnjigovodstvenaVrijednostOd.HasValue)
            {
                query = query.Where(z => z.KnjigovodstvenaVrijednost <= KnjigovodstvenaVrijednostOd.Value);
            }

            return query;
        }


    }
}
