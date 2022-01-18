using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OzoMvc.Models
{
  public class OpremaDenorm
  {
    public int InventarniBroj { get; set; }
      public string Naziv { get; set; }
     public double? KupovnaVrijednost { get; set; }
     public double? KnjigovodstvenaVrijednost { get; set; }
     public double VrijemeAmortizacije { get; set; }
    public int OpremaId { get; set; }
    public TimeSpan? Satnica { get; set; }
}
 }