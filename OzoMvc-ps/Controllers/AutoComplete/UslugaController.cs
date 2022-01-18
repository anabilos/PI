using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OzoMvc.Models;
using Microsoft.Extensions.Options;



namespace OzoMvc.Controllers.AutoComplete
{
    [Route("autocomplete/[controller]")]
    public class UslugaController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appData;

        public UslugaController(PI05Context ctx, IOptionsSnapshot<AppSettings> options)
        {
            this.ctx = ctx;
            appData = options.Value;
        }
        
        [HttpGet]      
        public IEnumerable<IdLabel> Get(string term)
        {
            var query = ctx.Usluga
                            .Select(m => new IdLabel
                            {
                                Id = m.Id,
                                Label =  m.Naziv
                            })
                            .Where(l => l.Label.Contains(term));
          
            var list = query.OrderBy(l => l.Label)
                            .ThenBy(l => l.Id)
                            .Take(appData.AutoCompleteCount)
                            .ToList();           
            return list;
        }       
    }
}