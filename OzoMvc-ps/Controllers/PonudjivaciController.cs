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
public class PonudjivaciController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        public PonudjivaciController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
        }

        public IActionResult Index( int page=1, int sort=1, bool ascending=true){
            int pagesize=appSettings.PageSize;
            var query=ctx.Ponudjivaci.AsNoTracking();
            int count=query.Count();

            var pagingInfo = new PagingInfo(){
                CurrentPage=page,
                Sort=sort,
                Ascending=ascending,
                TotalItems=count,
                ItemsPerPage=pagesize
            };

            if(page > pagingInfo.TotalPages){
                return RedirectToAction(nameof(Index), new{page=pagingInfo.TotalPages, sort, ascending });
            }
            System.Linq.Expressions.Expression<Func<Ponudjivaci,object>> orderSelector=null;
            switch(sort){
                case 1:
                    orderSelector=p=>p.Id;
                    break;
                case 2:
                    orderSelector=p=>p.Naziv;
                    break;
                
             }
            if(orderSelector!=null){
                query=ascending ? query.OrderBy(orderSelector):query.OrderByDescending(orderSelector);
            }
            var ponudjivaci=query
                             .Skip((page-1)*pagesize)
                             .Take(pagesize)
                             .ToList();
            

        var model=new PonudjivaciViewModel{
            ponudjivaci=ponudjivaci,
            PagingInfo=pagingInfo
        };
        return View(model);
    }
    [HttpGet]
    public IActionResult Create()
    {   
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Ponudjivaci ponudjivac)
    {
      
          bool exists = ctx.Ponudjivaci.Any(a => a.Naziv == ponudjivac.Naziv);
        if (exists)
        {
          ModelState.AddModelError(nameof(Ponudjivaci.Naziv), "Ponuđivač s navedenom imenom već postoji");
        }
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Add(ponudjivac);
          ctx.SaveChanges();


          TempData[Constants.Message] = $"Ponuđivač {ponudjivac.Naziv} dodan.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index));
        }
        catch (Exception exc)
        {
         
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(ponudjivac);
        }
      }
      else
      {
        return View(ponudjivac);
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int Id, int page = 1, int sort = 1, bool ascending = true)
    {
      var ponudjivac = ctx.Ponudjivaci
                       .AsNoTracking()
                       .Where(p => p.Id == Id)
                       .SingleOrDefault();
      if (ponudjivac != null)
      {
        try
        {
          string naziv = ponudjivac.Naziv;
          ctx.Remove(ponudjivac);
          ctx.SaveChanges();
         
          TempData[Constants.Message] = $"Ponuđivač {naziv} uspješno izbrisan";
          TempData[Constants.ErrorOccurred] = false;
        }
        catch (Exception exc)
        {
          TempData[Constants.Message] = "Pogreška prilikom brisanja ponuđivača: " + exc.CompleteExceptionMessage();
          TempData[Constants.ErrorOccurred] = true;

           }
      }
      else
      {
       
        TempData[Constants.Message] = "Ne postoji ponuđivač s id: " + Id;
        TempData[Constants.ErrorOccurred] = true;
      }
      return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
    }

    [HttpGet]
    public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      var ponudjivac = ctx.Ponudjivaci.AsNoTracking().Where(p => p.Id == id).SingleOrDefault();
      if (ponudjivac == null)
      {
        
        return NotFound("Ne postoji ponuđivač s id: " + id);
      }
      else
      {
        ViewBag.Page = page;
        ViewBag.Sort = sort;
        ViewBag.Ascending = ascending;
        return View(ponudjivac);
      }
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
     public IActionResult Update(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      Ponudjivaci ponudjivaci=ctx.Ponudjivaci.Find(id);
     
      if (ponudjivaci == null)
      {
        return NotFound($"Neispravan id ponuđivača: {id}");
      }

      
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(ponudjivaci);
          ctx.SaveChanges();

          TempData[Constants.Message] = "Mjesto ažurirano.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });          
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(ponudjivaci);
        }
      }
      else
      {
        return View(ponudjivaci);
      }
    }

   
  }
}