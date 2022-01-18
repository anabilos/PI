using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OzoMvc.Extensions;
using OzoMvc.Models;
using OzoMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace OzoMvc.Controllers
{
    public class CertifikatiController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<CertifikatiController> logger;

        public CertifikatiController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<CertifikatiController> logger){
            this.ctx=ctx;
            this.logger=logger;
            appSettings = optionsSnapshot.Value;
        }
        public IActionResult Index( int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.Certifikati.AsNoTracking();

      /*  CertifikatiFilter cf = new CertifikatiFilter();
        if (!string.IsNullOrWhiteSpace(filter)){
        cf = CertifikatiFilter.FromString(filter);
        if (!cf.IsEmpty())
        {
          if (cf.Id.HasValue)
          {
            cf.Naziv = ctx.Certifikati
                                .Where(c => c.Id == c.Id)
                                .Select(c => c.Naziv)
                                .FirstOrDefault();
          }
          query = cf.Apply(query);
        }
      }*/
            int count = query.Count();

            var pagingInfo = new PagingInfo{
                CurrentPage = page,
                Sort = sort,
                Ascending= ascending,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page<1){
                page=1;
            }
            else if(page>pagingInfo.TotalPages){
                if (page != 1  /*cf.IsEmpty()*/)
                    {
                        return RedirectToAction(nameof(Index), 
                        new { page = pagingInfo.TotalPages, sort, ascending });
                    }            
            }

       query = ApplySort(sort, ascending, query);

           
            var certifikati = query
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToList();
            
            var model = new CertifikatiViewModel{
                certifikati = certifikati,
                PagingInfo = pagingInfo,
                //Filter=cf
            };
            return View(model);
        }

        private IQueryable<Certifikati> ApplySort(int sort, bool ascending, IQueryable<Certifikati> query)
        {
        System.Linq.Expressions.Expression<Func<Certifikati, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.Id;
          break;
        case 2:
          orderSelector = d => d.Naziv;
          break;
        case 3:
          orderSelector = d => d.Opis;
          break;        }
          if (orderSelector != null)
         {
          query = ascending ?
                 query.OrderBy(orderSelector) :
                 query.OrderByDescending(orderSelector);
             }

        return query;
        }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Certifikati certifikati){
        logger.LogTrace(JsonSerializer.Serialize(certifikati), new JsonSerializerOptions{IgnoreNullValues=true});

        if(ModelState.IsValid){
            try{
                ctx.Add(certifikati);
                ctx.SaveChanges();
                TempData[Constants.Message] = $"Certifikat {certifikati.Naziv} dodan.";
                TempData[Constants.ErrorOccurred] = false;
                logger.LogInformation(new EventId(1000), $"Certifikat {certifikati.Naziv} dodan.");

                return RedirectToAction(nameof(Index));
            }
            catch(Exception exc){
                logger.LogError("Pogreška prilikom dodavanja novog certifikata: {0}", exc.CompleteExceptionMessage());
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return View(certifikati);
            }
        }
        else{
            return View(certifikati);
        }
    }
    public IActionResult Delete (int Id, int page = 1, int sort = 1, bool ascending = true){
        var certifikati = ctx.Certifikati.Find(Id);
        if(certifikati==null){
            return NotFound();
        }
        else{
            try{
                int id=certifikati.Id;
                ctx.Remove(certifikati);
                ctx.SaveChanges();
                TempData[Constants.Message] = $"Certifikat {id} obrisan.";
                TempData[Constants.ErrorOccurred] = false;
            }
            catch(Exception exc){
                TempData[Constants.Message] = "Greška prilikom brisanja države."+ exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
    }
     [HttpGet]
    public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
    {
        var certifikati = ctx.Certifikati.AsNoTracking().Where(c => c.Id == Id).SingleOrDefault();
        if(certifikati==null){
            return NotFound($"Ne postoji država s oznakom: " + Id);
        }
        else{
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            return View(certifikati);
        }
    }
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true){
        try{
            Certifikati certifikati = await ctx.Certifikati.FindAsync(Id);
            if(certifikati==null){
                return NotFound($"Ne postoji država s oznakom: " + Id);
            }
            ViewBag.Page=page;
            ViewBag.Sort=sort;
            ViewBag.Ascending=ascending;
        bool ok =   await TryUpdateModelAsync<Certifikati>(certifikati, "",c=> c.Naziv, c=>c.Opis);
        if(ok){
            try{
                await ctx.SaveChangesAsync();
                TempData[Constants.Message] = $"Certifikat {certifikati.Naziv} ažuriran.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });

            }
            catch(Exception exc){
            ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
            return View(certifikati);
            }
        }
        else{
            ModelState.AddModelError(string.Empty, "Podatke o certifikatima nije moguce povezati");
            return View(certifikati);
        }
        }
        catch(Exception exc){
            TempData[Constants.Message]=exc.CompleteExceptionMessage();
            TempData[Constants.ErrorOccurred] = true;
            return RedirectToAction(nameof(Index), new {Id, page = page, sort = sort, ascending = ascending });

        }
    } 
}
}