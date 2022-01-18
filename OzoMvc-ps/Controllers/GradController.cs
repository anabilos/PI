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
    public class GradController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;

        public GradController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
        {
            this.ctx = ctx;
            appSettings=optionsSnapshot.Value;
        }
        
         [HttpGet]
        public IActionResult Create(){
            return View();
        }
        

     [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Grad grad){
            if(ModelState.IsValid){
            try{
                ctx.Add(grad);
                ctx.SaveChanges();  
                TempData[Constants.Message] = $"Grad {grad.Naziv} uspješno dodan";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index)); 
            }
            catch(Exception exc){
            ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
             return View(grad);
            }
            }
            else{
                return View(grad);
            }

        }
    

    
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int Id, int page=1, int sort=1, bool ascending=true){
             var grad = ctx.Grad
                       .AsNoTracking() 
                       .Where(g=> g.Id == Id)
                       .SingleOrDefault();
            if( grad == null){
                return NotFound();
            }
            else{
                try{
                    string ime=grad.Naziv;
                    ctx.Remove(grad);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Grad {ime} uspješno izbrisan";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch(Exception){
                    TempData[Constants.Message] = "Ne možete izbrisati grad povezan s određenim podacima";
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new{page,sort,ascending});
            }
         
        }
      
      
      
      public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
    {      
      
      int pagesize = appSettings.PageSize;
      var query = ctx.Grad.AsNoTracking();
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

      System.Linq.Expressions.Expression<Func<Grad, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = g => g.Id;
          break;
        case 2:
          orderSelector = g => g.Naziv;
          break;
        
      }
      if (orderSelector != null)
      {
        query = ascending ?
               query.OrderBy(orderSelector) :
               query.OrderByDescending(orderSelector);
      }

      var gradovi = query
                  .Select(g => new GradViewModel
                  {
                    Id = g.Id,
                    Naziv = g.Naziv
                   
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
      var model = new GradoviViewModel
      {
        Gradovi = gradovi,
        PagingInfo = pagingInfo
      };

      return View(model);
    }
    }
}
