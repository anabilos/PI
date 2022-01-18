using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using OzoMvc.Models;
using OzoMvc.TagHelpers;

namespace OzoMvc.ViewModels
{
  public class UslugaFilter : IPageFilter
  {
    public int? Id { get; set; }
    public string Naziv { get; set; }
     public int? NormativId { get; set; }
    public string NazNormative { get; set; }
    public double? IznosOd { get; set; }
    public double? IznosDo { get; set; }

      public bool IsEmpty()
    {
      bool active =  Id.HasValue
                    ||NormativId.HasValue
                    || IznosOd.HasValue
                    || IznosDo.HasValue;
      return !active;
    }
    public override string ToString()
    {
      return string.Format("{0}-{1}-{2}-{3}",
          Id,
          NormativId,
          IznosOd,
          IznosDo);
    }
     public static UslugaFilter FromString(string s)
    {
      var filter = new UslugaFilter();
      var arr = s.Split(new char[] { '-' }, StringSplitOptions.None);
      try
      {
         filter.Id = string.IsNullOrWhiteSpace(arr[0]) ? new int?() : int.Parse(arr[0]);
        filter.NormativId = string.IsNullOrWhiteSpace(arr[1]) ? new int?() : int.Parse(arr[1]);
        filter.IznosOd = string.IsNullOrWhiteSpace(arr[2]) ? new double?() : double.Parse(arr[2]);
        filter.IznosDo = string.IsNullOrWhiteSpace(arr[3]) ? new double?() : double.Parse(arr[3]);
      }
      catch { } 
      return filter;
    }
    public IQueryable<Usluga> Apply(IQueryable<Usluga> query)
    {
      if (Id.HasValue)
      {
        query = query.Where(d => d.Id == Id.Value);
      }
     
    if (NormativId.HasValue)
      {
        query = query.Where(d => d.NormativId == NormativId.Value);
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