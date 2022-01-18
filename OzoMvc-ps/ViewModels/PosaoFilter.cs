using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using OzoMvc.Models;
using OzoMvc.TagHelpers;

namespace OzoMvc.ViewModels
{
    public class PosaoFilter : IPageFilter
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        public DateTime? TrajanjeOd { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
        public DateTime? TrajanjeDo { get; set; }
        public int? UslugaId { get; set; }
        public double? IznosOd { get; set; }
        public double? IznosDo { get; set; }
        public string? NazivUsluge { get; set; }
       
        public bool IsEmpty()
        {
            bool active = TrajanjeOd.HasValue
                          || TrajanjeDo.HasValue
                          || IznosOd.HasValue
                          || IznosDo.HasValue
                          || UslugaId.HasValue;

                          
            return !active;
        }
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}",

                TrajanjeOd?.ToString("dd.MM.yyyy"),
                TrajanjeDo?.ToString("dd.MM.yyyy"),
                IznosOd,
                IznosDo,
                UslugaId
                );
        }
        public static PosaoFilter FromString(string s)
        {
            var filter = new PosaoFilter();
            var arr = s.Split(new char[] { '-' }, StringSplitOptions.None);
            try
            {

                filter.TrajanjeOd = string.IsNullOrWhiteSpace(arr[0]) ? new DateTime?() : DateTime.ParseExact(arr[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filter.TrajanjeDo = string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filter.IznosOd = string.IsNullOrWhiteSpace(arr[2]) ? new double?() : double.Parse(arr[2]);
                filter.IznosDo = string.IsNullOrWhiteSpace(arr[3]) ? new double?() : double.Parse(arr[3]);
                filter.UslugaId= string.IsNullOrWhiteSpace(arr[4]) ? new int?() : int.Parse(arr[4]);
            

            }
            catch { }
            return filter;
        }
        public IQueryable<Posao> Apply(IQueryable<Posao> query)
        {
            if (UslugaId.HasValue)
            {
                query = query.Where(d => d.UslugaId == UslugaId);
            }
            if (TrajanjeOd.HasValue)
            {
                query = query.Where(d => d.Vrijeme >= TrajanjeOd.Value);
            }
            if (TrajanjeDo.HasValue)
            {
                query = query.Where(d => d.Vrijeme <= TrajanjeDo.Value);
            }
            if (IznosOd.HasValue)
            {
                query = query.Where(d => d.Cijena >= IznosOd.Value);
            }
            if (IznosDo.HasValue)
            {
                query = query.Where(d => d.Cijena <= IznosDo.Value);
            }
            return query;
        }

    }
}