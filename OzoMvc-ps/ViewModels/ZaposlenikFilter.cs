using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using OzoMvc.Models;

namespace OzoMvc.ViewModels{

    public class ZaposlenikFilter : IPageFilter{
        public int? Id{get; set;}
        public string Prezime{get; set;}
        public string Mjesto{get; set;}
        public int? IznosOd{get; set;}
        public int? IznosDo{get; set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:dd.MM.yyyy.}", ApplyFormatInEditMode=false)]
        public DateTime? DatumOd{get; set;}

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:dd.MM.yyyy.}", ApplyFormatInEditMode=false)]
        public DateTime? DatumDo{get; set;}

        public bool IsEmpty() {
            bool active=Id.HasValue
                        ||DatumOd.HasValue
                        ||DatumDo.HasValue
                        ||IznosOd.HasValue
                        ||IznosDo.HasValue;
                        

                        
            return !active; 
        }

        public override string ToString(){
            return string.Format("{0}-{1}-{2}-{3}-{4}",
                Id,
                DatumOd?.ToString("dd.MM.yyyy"),
                DatumDo?.ToString("dd.MM.yyyy"),
                IznosOd,
                IznosDo
               
  
            );
        } 

        public static ZaposlenikFilter FromString(string s, ILogger logger=null){
            var filter= new ZaposlenikFilter();
            try{
                var arr=s.Split('-',StringSplitOptions.None);
                filter.Id=string.IsNullOrWhiteSpace( arr[0]) ? new int?() : int.Parse(arr[0]);
                filter.DatumOd=string.IsNullOrWhiteSpace( arr[1]) ? new DateTime?() : DateTime.ParseExact(arr[1], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filter.DatumDo=string.IsNullOrWhiteSpace( arr[2]) ? new DateTime?() : DateTime.ParseExact(arr[2 ], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                filter.IznosOd=string.IsNullOrWhiteSpace( arr[3]) ? new int?() : int.Parse(arr[3]);
                filter.IznosDo=string.IsNullOrWhiteSpace( arr[4]) ? new int?() : int.Parse(arr[4]);
               
              
            }catch{}
            return filter;
        }
        public IQueryable<Zaposlenik> Apply(IQueryable<Zaposlenik> query){
            if (Id.HasValue){
                query=query.Where(z=>z.Id==Id.Value);
            }
             if (DatumOd.HasValue){
                query=query.Where(z=>z.DatumRodjenja>=DatumOd.Value);
            }
             if (DatumDo.HasValue){
                query=query.Where(z=>z.DatumRodjenja<=DatumDo.Value);
            }
             if (IznosOd.HasValue){
                query=query.Where(z=>z.MjesecniTrosak >= IznosOd.Value);
            }
             
             if (IznosDo.HasValue){
                query=query.Where(z=>z.MjesecniTrosak<=IznosDo.Value);
            }
             
            return query;
        }

        
    }
}