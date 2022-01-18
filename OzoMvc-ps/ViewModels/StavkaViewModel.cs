using System;

namespace OzoMvc.ViewModels{
    public class StavkaViewModel{
    public int Id { get; set; }
        
        public double CijenaPonude { get; set; }
        public bool DobivenNatjecaj { get; set; }
        public string Ponudjivac { get; set; }
        public int PonudjivacId{get; set;}
        public int NatjecajId{get; set;}
      
    }
}