using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using OzoMvc.Models;
using OzoMvc.TagHelpers;

namespace OzoMvc.ViewModels
{
  public class NatjecajFilter : IPageFilter
  {
 
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
    public DateTime? TrajanjeOd { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy.}", ApplyFormatInEditMode = false)]
    public DateTime? TrajanjeDo { get; set; }
    public double? IznosOd { get; set; }
    public double? IznosDo { get; set; }

      public bool IsEmpty()
    {
      bool active =  TrajanjeOd.HasValue
                    || TrajanjeDo.HasValue
                    || IznosOd.HasValue
                    || IznosDo.HasValue;
      return !active;
    }
    public override string ToString()
    {
      return string.Format("{0}-{1}-{2}-{3}",
         
          TrajanjeOd?.ToString("dd.MM.yyyy"),
          TrajanjeDo?.ToString("dd.MM.yyyy"),
          IznosOd,
          IznosDo);
    }
     public static NatjecajFilter FromString(string s)
    {
      var filter = new NatjecajFilter();
      var arr = s.Split(new char[] { '-' }, StringSplitOptions.None);
      try
      {
        
        filter.TrajanjeOd = string.IsNullOrWhiteSpace(arr[0]) ? new DateTime?() : DateTime.ParseExact(arr[0], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        filter.TrajanjeDo = string.IsNullOrWhiteSpace(arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        filter.IznosOd = string.IsNullOrWhiteSpace(arr[2]) ? new double?() : double.Parse(arr[2]);
        filter.IznosDo = string.IsNullOrWhiteSpace(arr[3]) ? new double?() : double.Parse(arr[3]);
      }
      catch { } 
      return filter;
    }
    public IQueryable<Natjecaj> Apply(IQueryable<Natjecaj> query)
    {
     
      if (TrajanjeOd.HasValue)
      {
        query = query.Where(d => d.TrajanjeOd == TrajanjeOd.Value);
      }
      if (TrajanjeDo.HasValue)
      {
        query = query.Where(d => d.TrajanjeDo == TrajanjeDo.Value);
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