using OzoMvc.Models;
using OzoMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using OzoMvc.Extensions;

namespace OzoMvc.Controllers{
    public class MjestoController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;

        public MjestoController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
        }
        
         [HttpGet]
        public IActionResult Create(){
            PrepareDropDownLists();
            return View();
        }
        private void PrepareDropDownLists(){
            var gradovi=ctx.Grad
                          .OrderBy(g=>g.Naziv)
                          .Select(g=>new{g.Naziv,g.Id})
                          .ToList();
            ViewBag.Gradovi= new SelectList(gradovi, nameof(Grad.Id),nameof(Grad.Naziv));              
        }

     [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Mjesto mjesto){
            if(ModelState.IsValid){
            try{
                ctx.Add(mjesto);
                ctx.SaveChanges();  
                TempData[Constants.Message] = $"Mjesto {mjesto.Naziv} uspješno dodano";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index)); 
            }
            catch(Exception exc){
             ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
             PrepareDropDownLists();
             return View(mjesto);
            }
            }
            else{
                PrepareDropDownLists();
                return View(mjesto);
            }

        }
    [HttpGet]
    public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;

      var mjesto = ctx.Mjesto
                       .AsNoTracking()
                       .Where(m => m.Id == id)
                       .SingleOrDefault();
      if (mjesto != null)
      {
        PrepareDropDownLists();
        return View(mjesto);
      }
      else
      {
        return NotFound($"Neispravan id mjesta: {id}");
      }
    }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken] 
    public IActionResult Edit(Mjesto mjesto, int page = 1, int sort = 1, bool ascending = true)
    {
      if (mjesto == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      bool checkId = ctx.Mjesto.Any(m => m.Id == mjesto.Id);
      if (!checkId)
      {
        return NotFound($"Neispravan id mjesta: {mjesto?.Id}");
      }

      PrepareDropDownLists();
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(mjesto);
          ctx.SaveChanges();

          TempData[Constants.Message] = $"Mjesto {mjesto.Naziv} ažurirano.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending });          
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(mjesto);
        }
      }
      else
      {
        return View(mjesto);
      }
    }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int Id, int page=1, int sort=1, bool ascending=true){
             var mjesto = ctx.Mjesto
                       .AsNoTracking() 
                       .Where(m=> m.Id == Id)
                       .SingleOrDefault();
            if( mjesto == null){
                return NotFound();
            }
            else{
                try{
                    string ime=mjesto.Naziv;
                    ctx.Remove(mjesto);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Mjesto {ime} uspješno izbrisano";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch(Exception){
                    TempData[Constants.Message] = "Ne možete izbrisati mjesto povezano s određenim podacima";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new{page,sort,ascending});
            }
         
        }
      
      
      
      public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
    {      
      
      int pagesize = appSettings.PageSize;
      var query = ctx.Mjesto.AsNoTracking();
      int count = query.Count();

      var pagingInfo = new PagingInfo
      {
        CurrentPage = page,
        Sort = sort,
        Ascending = ascending,
        ItemsPerPage = pagesize,
        TotalItems = count
      };
      if (page < 1)
      {
        page = 1;
      }
      else if (page > pagingInfo.TotalPages)
      {
        return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort = sort, ascending = ascending });
      }

      System.Linq.Expressions.Expression<Func<Mjesto, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = m => m.Id;
          break;
        case 2:
          orderSelector = m => m.Naziv;
          break;
        case 3:
          orderSelector = m => m.Grad.Naziv;
          break;
        
      }
      if (orderSelector != null)
      {
        query = ascending ?
               query.OrderBy(orderSelector) :
               query.OrderByDescending(orderSelector);
      }

      var mjesta = query
                  .Select(m => new MjestoViewModel
                  {
                    Id = m.Id,
                    Naziv = m.Naziv,
                    NazivGrada = m.Grad.Naziv
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
      var model = new MjestaViewModel
      {
        Mjesta = mjesta,
        PagingInfo = pagingInfo
      };

      return View(model);
    }
    }
}
