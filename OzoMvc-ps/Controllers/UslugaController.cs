using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OzoMvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using OzoMvc.ViewModels;
using OzoMvc.Extensions;

namespace OzoMvc.Controllers{
public class UslugaController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        public UslugaController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
        }

        public IActionResult Index( string filter, int page=1, int sort=1, bool ascending=true){
            int pagesize=appSettings.PageSize;
            var query=ctx.Usluga.AsNoTracking();
             UslugaFilter uf = new UslugaFilter();
      if (!string.IsNullOrWhiteSpace(filter))
      {
        uf = UslugaFilter.FromString(filter);
        if (!uf.IsEmpty())
        {
          if (uf.NormativId.HasValue)
          {
            uf.NazNormative = ctx.Normativi
                                .Where(p => p.Id == uf.NormativId)
                                .Select(vp => vp.Opis)
                                .FirstOrDefault();
          }
          if(uf.Id.HasValue){
            uf.Naziv = ctx.Usluga
                                .Where(p => p.Id == uf.Id)
                                .Select(vp => vp.Naziv)
                                .FirstOrDefault();
          }
          query = uf.Apply(query);
        }
      }
            int count=query.Count();

            var pagingInfo = new PagingInfo(){
                CurrentPage=page,
                Sort=sort,
                Ascending=ascending,
                TotalItems=count,
                ItemsPerPage=pagesize
            };

            if(page > pagingInfo.TotalPages){
                return RedirectToAction(nameof(Index), new{page=pagingInfo.TotalPages, sort, ascending, filter});
            }
            System.Linq.Expressions.Expression<Func<Usluga,object>> orderSelector=null;
            switch(sort){
                case 1:
                    orderSelector=n=>n.Id;
                    break;
                case 2:
                    orderSelector=n=>n.Naziv;
                    break;
                case 3:
                    orderSelector=n=>n.Cijena;
                    break;
                case 4:
                    orderSelector=n=>n.Opis;
                    break;
                case 5:
                    orderSelector=n=>n.Normativ.Opis;
                    break;
             }
            if(orderSelector!=null){
                query=ascending ? query.OrderBy(orderSelector):query.OrderByDescending(orderSelector);
            }
            var usluge=query
                             .Select(u=>new UslugaViewModel{
                                Id=u.Id,
                                Naziv=u.Naziv,
                                Cijena=u.Cijena,
                                Opis=u.Opis,
                                OpisNormative=u.Normativ.Opis
                             })
                             .Skip((page-1)*pagesize)
                             .Take(pagesize)
                             .ToList();
            

        var model=new UslugeViewModel{
            Usluge=usluge,
            PagingInfo=pagingInfo,
            Filter = uf
        };
        return View(model);
    }
    [HttpGet]
    public IActionResult Create()
    {   PrepareDropDownLists();
      return View();
    }

        private void PrepareDropDownLists()
        {
            var normativi=ctx.Normativi
                        .OrderBy(d => d.Opis)
                        .Select(d => new { d.Opis, d.Id})
                        .ToList();
                                   
            ViewBag.Normativi=new SelectList(normativi,nameof(Normativi.Id),nameof(Normativi.Opis));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Usluga usluga)
    {
      
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Add(usluga);
          ctx.SaveChanges();

          TempData[Constants.Message] = $"Usluga {usluga.Naziv} dodana.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));
        }
        catch (Exception exc)
        {
        
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          PrepareDropDownLists();
          return View(usluga);
        }
      }
      else
      {
          PrepareDropDownLists();
        return View(usluga);
      }
    }
        [HttpPost]
    public IActionResult Filter(UslugaFilter filter)
    {
      return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int Id,string filter, int page=1,int sort=1, bool ascending=true){
        var usluga=ctx.Usluga
                      .AsNoTracking()
                      .Where(d=>d.Id==Id)
                      .SingleOrDefault();

            if(usluga==null){
                return NotFound($"Usluga sa šifrom {Id} ne postoji");
            }
            else{
                try{

                    string naziv=usluga.Naziv;
                    ctx.Remove(usluga);
                    ctx.SaveChanges();
                     TempData[Constants.Message] = $"Usluga {naziv} izbrisana.";
                     TempData[Constants.ErrorOccurred] = false;

                }catch (Exception exc){
                         TempData[Constants.Message] = "Pogreška prilikom brisanja usluge: " + exc.CompleteExceptionMessage();
                        TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index),new { page = page, sort = sort, ascending = ascending , filter=filter});
            }
    }
    [HttpGet]
     public IActionResult Edit(int id, string filter,int page = 1, int sort = 1, bool ascending = true)
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;
      ViewBag.Filter = filter;

      var usluga = ctx.Usluga
                       .AsNoTracking()
                       .Where(u => u.Id == id)
                       .SingleOrDefault();
      if (usluga != null)
      {
        PrepareDropDownLists();
        return View(usluga);
      }
      else
      {
        return NotFound($"Neispravan id usluge: {id}");
      }
    }
    [HttpPost , ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Usluga usluga, string filter,int page = 1, int sort = 1, bool ascending = true)
    {
      if (usluga == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      bool checkId = ctx.Usluga.Any(u => u.Id == usluga.Id);
      if (!checkId)
      {
        return NotFound($"Neispravan id usluge: {usluga?.Id}");
      }

      PrepareDropDownLists();
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(usluga);
          ctx.SaveChanges();

          TempData[Constants.Message] = "Usluga ažurirana.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending,filter });          
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(usluga);
        }
      }
      else
      {
        return View(usluga);
      }
    }

 }
}