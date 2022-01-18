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
    public class ZaposlenikController : Controller{
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;

        public ZaposlenikController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot)
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
            var mjesta=ctx.Mjesto
                          .OrderBy(m=>m.Naziv)
                          .Select(m=>new{m.Naziv,m.Id})
                          .ToList();
            ViewBag.Mjesta= new SelectList(mjesta, nameof(Mjesto.Id),nameof(Mjesto.Naziv));              
        }

     [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Zaposlenik zaposlenik){
            if(ModelState.IsValid){
            try{
                ctx.Add(zaposlenik);
                ctx.SaveChanges();  
                TempData[Constants.Message] = $"Zaposlenik {zaposlenik.Ime} uspješno dodan";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index)); 
            }
            catch(Exception exc){
             ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
             PrepareDropDownLists();
             return View(zaposlenik);
            }
            }
            else{
                PrepareDropDownLists();
                return View(zaposlenik);
            }

        }
    [HttpGet]
    public IActionResult Edit(int id, string filter, int page = 1, int sort = 1, bool ascending = true)
    {
      ViewBag.Page = page;
      ViewBag.Sort = sort;
      ViewBag.Ascending = ascending;
      ViewBag.Filter=filter;

      var zaposlenik = ctx.Zaposlenik
                       .AsNoTracking()
                       .Where(z => z.Id == id)
                       .SingleOrDefault();
      if (zaposlenik != null)
      {
        PrepareDropDownLists();
        return View(zaposlenik);
      }
      else
      {
        return NotFound($"Neispravan id zaposlenika: {id}");
      }
    }

    [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken] 
    public IActionResult Edit(Zaposlenik zaposlenik, string filter, int page = 1, int sort = 1, bool ascending = true)
    {
      if (zaposlenik == null)
      {
        return NotFound("Nema poslanih podataka");
      }
      bool checkId = ctx.Zaposlenik.Any(z => z.Id == zaposlenik.Id);
      if (!checkId)
      {
        return NotFound($"Neispravan id zaposlenika: {zaposlenik?.Id}");
      }

      PrepareDropDownLists();
      if (ModelState.IsValid)
      {
        try
        {
          ctx.Update(zaposlenik);
          ctx.SaveChanges();

          TempData[Constants.Message] = $"Zaposlenik {zaposlenik.Ime} ažuriran.";
          TempData[Constants.ErrorOccurred] = false;
          return RedirectToAction(nameof(Index), new { page, sort, ascending, filter});          
        }
        catch (Exception exc)
        {
          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
          return View(zaposlenik);
        }
      }
      else
      {
        return View(zaposlenik);
      }
    }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int Id, string filter, int page=1, int sort=1, bool ascending=true){
             var zaposlenik = ctx.Zaposlenik
                       .AsNoTracking() 
                       .Where(z=> z.Id == Id)
                       .SingleOrDefault();
            if( zaposlenik == null){
                return NotFound();
            }
            else{
                try{
                    string ime=zaposlenik.Ime;
                    ctx.Remove(zaposlenik);
                    ctx.SaveChanges();
                    TempData[Constants.Message] = $"Zaposlenik {ime} uspješno izbrisan";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch(Exception exc){
                    TempData[Constants.Message] = "Pogreška prilikom brisanja zaposlenika" + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;
                }
                return RedirectToAction(nameof(Index), new{page,sort,ascending,filter});
            }
         
        }
      
      
      
      public IActionResult Index(string filter,int page = 1, int sort = 1, bool ascending = true)
    {      
      
      int pagesize = appSettings.PageSize;
      var query = ctx.Zaposlenik.AsNoTracking();
      int count = query.Count();
      ZaposlenikFilter zf=new ZaposlenikFilter();
      if(!string.IsNullOrWhiteSpace(filter)){
        zf= ZaposlenikFilter.FromString(filter);
        if(!zf.IsEmpty()){
         
          if(zf.Id.HasValue){
             zf.Prezime=ctx.Zaposlenik
                       .Where(z=>z.Id==zf.Id)
                       .Select(zf=>zf.Prezime)
                       .FirstOrDefault();


          }
          query=zf.Apply(query);
        }
      }

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
        return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort = sort, ascending = ascending, filter=filter});
      }

      System.Linq.Expressions.Expression<Func<Zaposlenik, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = z => z.Id;
          break;
        case 2:
          orderSelector = z => z.Ime;
          break;
        case 3:
          orderSelector = z => z.Prezime;
          break;
        case 4:
          orderSelector = z => z.Mjesto.Naziv;
          break;
          case 5:
          orderSelector = z => z.MjesecniTrosak;
          break;
      }
      if (orderSelector != null)
      {
        query = ascending ?
               query.OrderBy(orderSelector) :
               query.OrderByDescending(orderSelector);
      }

      var zaposlenici = query
                  .Select(z => new ZaposlenikViewModel
                  {
                    Id = z.Id,
                    Ime = z.Ime,
                    Prezime = z.Prezime,
                    DatumRodjenja = z.DatumRodjenja.Date,
                    MjesecniTrosak = z.MjesecniTrosak,
                    Naziv=z.Mjesto.Naziv
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
      var model = new ZaposleniciViewModel
      {
        Zaposlenici = zaposlenici,
        PagingInfo = pagingInfo,
        Filter=zf
      };

      return View(model);
    }
    [HttpPost]
    public IActionResult Filter(ZaposlenikFilter filter)
    {
      return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
    }
    }
}
