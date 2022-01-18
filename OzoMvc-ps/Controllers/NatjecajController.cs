using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OzoMvc.Models;
using System.Linq;
using System;
using OzoMvc.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using OzoMvc.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OzoMvc.Controllers{
public class NatjecajController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        public NatjecajController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
        }

        public IActionResult Index( string filter,int page=1, int sort=1, bool ascending=true){
            int pagesize=appSettings.PageSize;
            var query=ctx.Natjecaj.AsNoTracking();

            int count=query.Count();
             NatjecajFilter nf = new NatjecajFilter();
              if (!string.IsNullOrWhiteSpace(filter))
      {
        nf = NatjecajFilter.FromString(filter);
        if (!nf.IsEmpty())
        {
       
          query = nf.Apply(query);
        }
      }

            var pagingInfo = new PagingInfo(){
                CurrentPage=page,
                Sort=sort,
                Ascending=ascending,
                TotalItems=count,
                ItemsPerPage=pagesize
            };

            if(page > pagingInfo.TotalPages){
                 if (page != 1 || nf.IsEmpty()){
                    return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending, filter });
                  }        
            }
            System.Linq.Expressions.Expression<Func<Natjecaj,object>> orderSelector=null;
            switch(sort){
                case 1:
                    orderSelector=n=>n.Id;
                    break;
                case 2:
                    orderSelector=n=>n.TrajanjeOd;
                    break;
                case 3:
                    orderSelector=n=>n.TrajanjeOd;
                    break;
                case 4:
                    orderSelector=n=>n.Cijena;
                    break;
                }
            if(orderSelector!=null){
                query=ascending ? query.OrderBy(orderSelector):query.OrderByDescending(orderSelector);
            }
        
            var natjecaji=query
                             .Select(u=>new NatjecajViewModel{
                                Id=u.Id,
                                Opis=u.Opis,
                                TrajanjeOd=u.TrajanjeOd,
                                TrajanjeDo=u.TrajanjeDo,
                                Cijena=u.Cijena,
                                Naziv=u.Usluga.Naziv,
                                Ponuda=u.Usluga.Cijena
                             })
                             .Skip((page-1)*pagesize)
                             .Take(pagesize)
                             .ToList();
            
                        

        var model=new NatjecajiViewModel{
            Natjecaji=natjecaji,
            PagingInfo=pagingInfo,
            Filter = nf
        };
        return View(model);
    }
       [HttpPost]
    public IActionResult Filter(NatjecajFilter filter)
    {
      return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
    }
 [HttpGet]
    public IActionResult Create()
    {
      PrepareDropDownLists();
      
      var natjecaj = new NatjecajViewModel
      {
        TrajanjeDo = DateTime.Now,
        TrajanjeOd=DateTime.Now
        
      };
      return View(natjecaj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NatjecajViewModel model)
    {
      if (ModelState.IsValid)
      {
        Natjecaj d = new Natjecaj();
        d.Opis = model.Opis;
        d.TrajanjeDo = model.TrajanjeDo.Date;
        d.TrajanjeOd = model.TrajanjeOd;
        d.Cijena = model.Cijena;
        d.UslugaId = model.UslugaId;
  
        foreach (var stavka in model.Stavke)
        {
          NatjecajPonudjivac novaStavka = new NatjecajPonudjivac();
          novaStavka.PonudjivacId = stavka.PonudjivacId;
          novaStavka.NatjecajId = stavka.NatjecajId;
          novaStavka.DobivenNatjecaj = stavka.DobivenNatjecaj;
          novaStavka.CijenaPonude=stavka.CijenaPonude;
         
          d.NatjecajPonudjivac.Add(novaStavka);          
        }

      
        try
        {
          ctx.Add(d);
          ctx.SaveChanges();

          TempData[Constants.Message] = $"Natječaj uspješno dodan. Id={d.Id}";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Edit), new { id = d.Id });

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }


        private void PrepareDropDownLists()
        {
            var usluge=ctx.Usluga
                        .OrderBy(d => d.Naziv)
                        .Select(d => new { d.Naziv, d.Id})
                        .ToList();
                                   
            ViewBag.Usluga=new SelectList(usluge,nameof(Usluga.Id),nameof(Usluga.Naziv));
    }
   
    public IActionResult Show(int id,string filter, int page = 1, int sort = 1, bool ascending = true,string viewName = nameof(Show))
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;
      ViewBag.Filter = filter;
    

      var natjecaj = ctx.Natjecaj
                        .Where(n => n.Id == id)
                        .Select(n => new NatjecajViewModel
                        {
                            Id=n.Id,
                          Opis = n.Opis,
                          TrajanjeOd = n.TrajanjeOd,
                          TrajanjeDo = n.TrajanjeDo,
                          Cijena = n.Cijena,
                          UslugaId = n.UslugaId,
                          Naziv = n.Usluga.Naziv,
                          Ponuda = n.Usluga.Cijena
                        })
                        .FirstOrDefault();
      if (natjecaj == null)
      {
        return NotFound($"Natječaj {id} ne postoji");
      }
      
        
        var stavke = ctx.NatjecajPonudjivac
                        .Where(s => s.NatjecajId == natjecaj.Id)
                        .OrderBy(s => s.Id)
                        .Select(s => new StavkaViewModel
                        {
                          Id = s.Id,
                          CijenaPonude = s.CijenaPonude,
                          DobivenNatjecaj = s.DobivenNatjecaj,
                          Ponudjivac = s.Ponudjivac.Naziv,
                          PonudjivacId = s.Ponudjivac.Id,
                          NatjecajId=s.NatjecajId
                         
                        })
                        .ToList();
        natjecaj.Stavke=stavke;
        return View( viewName ,natjecaj);
      }

      [HttpGet]
    public IActionResult Edit(int id, string filter,int page = 1, int sort = 1, bool ascending = true)
    {  PrepareDropDownLists();
      return Show(id, filter, page, sort, ascending, viewName: nameof(Edit));
    }

     [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(NatjecajViewModel model, string filter, int page = 1, int sort = 1, bool ascending = true)
    {
     PrepareDropDownLists();
      ViewBag.Page = page;
      ViewBag.Sort = sort;
       ViewBag.Filter = filter;
      ViewBag.Ascending = ascending;
   
      if (ModelState.IsValid)
      {
        var natjecaj = ctx.Natjecaj
                          .Include(n => n.NatjecajPonudjivac)
                          .Where(d => d.Id == model.Id)
                          .FirstOrDefault();
        if (natjecaj == null)
        {
          return NotFound("Ne postoji natječaj s id-om: " + model.Id);
        }

        natjecaj.Id = model.Id;
        natjecaj.Opis = model.Opis;
        natjecaj.Cijena = model.Cijena;
        natjecaj.TrajanjeOd = model.TrajanjeOd;
        natjecaj.TrajanjeDo = model.TrajanjeDo;
        natjecaj.UslugaId=model.UslugaId;
        
        
        

        List<int> idnp = model.Stavke
                                  .Where(s => s.Id > 0)
                                  .Select(s => s.Id)
                                  .ToList();
        List<bool> select=model.Stavke
                                .Where(s=>s.DobivenNatjecaj!=false)
                                .Select(s=>s.DobivenNatjecaj)
                                .ToList();
       

        

        
        if(select.Count>1){
          TempData[Constants.Message] = "Natječaj može dobiti samo jedna osoba.";
          TempData[Constants.ErrorOccurred] = true;
          return RedirectToAction (nameof(Edit), new
          {
            id = natjecaj.Id,
            filter,
            page,
            sort,
            ascending,
           
          });
        
        }
         
        ctx.RemoveRange(natjecaj.NatjecajPonudjivac.Where(n => !idnp.Contains(n.Id)));
  
        foreach (var stavka in model.Stavke)
        {
          
          NatjecajPonudjivac novaStavka;
          
          if (stavka.Id > 0)
          {
            novaStavka = natjecaj.NatjecajPonudjivac.First(n => n.Id == stavka.Id);
          }
          else
          {
              
             List<int> id=model.Stavke
                          .Where(s=>s.Id!=stavka.Id)
                          .Select(s=>s.PonudjivacId) 
                          .ToList();         
                          
            var invalid=model.Stavke
                              .Where(s=>s.Id==stavka.Id)
                              .Select(s=>s.PonudjivacId)
                            .ToList();
              int idd=invalid.FirstOrDefault();
            if(id.Contains(idd)){
          TempData[Constants.Message] = "Ponuđivač se već nalazi na popisu.";
          TempData[Constants.ErrorOccurred] = true;
          return RedirectToAction (nameof(Edit), new
          {
            id = natjecaj.Id,
            filter,
            page,
            sort,
            ascending
          });
            }
            novaStavka = new NatjecajPonudjivac();
            natjecaj.NatjecajPonudjivac.Add(novaStavka);
                        
  
          }
          novaStavka.PonudjivacId=stavka.PonudjivacId;
          novaStavka.CijenaPonude = stavka.CijenaPonude;
          novaStavka.DobivenNatjecaj = stavka.DobivenNatjecaj;
       
        
        }
        

        try
        {
         
          ctx.SaveChanges();
          

          TempData[Constants.Message] = $"Natjecaj {natjecaj.Id} uspješno ažuriran.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Edit), new
          {
            id = natjecaj.Id,
            filter,
            page,
            sort,
            ascending
          });

        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(model);
        }
      }
      else
      {
        return View(model);
      }
    }

 [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int Id,string filter, int page = 1, int sort = 1, bool ascending = true)
    {
      var natjecaj = await ctx.Natjecaj
                       .AsNoTracking()
                       .Where(n => n.Id == Id)
                       .SingleOrDefaultAsync();
      if (natjecaj != null)
      {
        try
        {
          ctx.Remove(natjecaj);
          await ctx.SaveChangesAsync();
          TempData[Constants.Message] = $"Natjecaj {natjecaj.Id} uspješno obrisan.";
          TempData[Constants.ErrorOccurred] = false;
        }
        catch (Exception exc)
        {
          TempData[Constants.Message] = "Pogreška prilikom brisanja natječaja: " + exc.CompleteExceptionMessage();
          TempData[Constants.ErrorOccurred] = true;
        }
      }
      else
      {
        TempData[Constants.Message] = "Ne postoji dokument s id-om: " + Id;
        TempData[Constants.ErrorOccurred] = true;
      }
      return RedirectToAction(nameof(Index), new {  page = page, sort = sort, ascending = ascending,filter=filter });
    }


 }
}
