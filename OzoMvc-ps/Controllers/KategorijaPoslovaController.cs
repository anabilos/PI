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
    public class KategorijaPoslovaController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<KategorijaPoslovaController> logger;

        public KategorijaPoslovaController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<KategorijaPoslovaController> logger){
            this.ctx=ctx;
            this.logger=logger;
            appSettings = optionsSnapshot.Value;
        }
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;
            var query = ctx.KategorijaPoslova.AsQueryable();

        

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
                if (page != 1)
                {
                    return RedirectToAction(nameof(Index), 
                    new { page = pagingInfo.TotalPages, sort, ascending });
                }            
            }

        query = ApplySort(sort, ascending, query);


            var kategorijaposlova = query
                                .Skip((page - 1) * pagesize)
                                .Take(pagesize)
                                .ToList();

            var model = new KategorijaPoslovaViewModel{
                kategorijaposlova = kategorijaposlova,
                PagingInfo = pagingInfo,
               // Filter = kpf
            };
            return View(model);
        }

        private IQueryable<KategorijaPoslova> ApplySort(int sort, bool ascending, IQueryable<KategorijaPoslova> query)
        {
        System.Linq.Expressions.Expression<Func<KategorijaPoslova, object>> orderSelector = null;
      switch (sort)
      {
        case 1:
          orderSelector = d => d.Id;
          break;
        case 2:
          orderSelector = d => d.Naziv;
          break;
            }
          if (orderSelector != null)
            {
            query = ascending ?
                 query.OrderBy(orderSelector):
                 query.OrderByDescending(orderSelector);
             }

        return query;        }

        public IActionResult Create()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(KategorijaPoslova kategorijaPoslova){
        logger.LogTrace(JsonSerializer.Serialize(kategorijaPoslova), new JsonSerializerOptions{IgnoreNullValues=true});

        if(ModelState.IsValid){
            try{
                ctx.Add(kategorijaPoslova);
                ctx.SaveChanges();
                TempData[Constants.Message] = $"Kategorija posla {kategorijaPoslova.Naziv} dodana.";
                TempData[Constants.ErrorOccurred] = false;
                logger.LogInformation(new EventId(1000), $"Kategorija posla  {kategorijaPoslova.Naziv} dodana.");
                return RedirectToAction(nameof(Index));
            }
            catch(Exception exc){
                logger.LogError("Pogreška prilikom dodavanja nove kategorije posla: {0}", exc.CompleteExceptionMessage());
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                return View(kategorijaPoslova);
            }
        }
        else{
            return View(kategorijaPoslova);
        }
    }
    public IActionResult Delete (int Id, int page = 1, int sort = 1, bool ascending = true){
        var kategorijaPoslova = ctx.KategorijaPoslova.Find(Id);
        if(kategorijaPoslova ==null){
            return NotFound();
        }
        else{
            try{
                int id=kategorijaPoslova.Id;
                ctx.Remove(kategorijaPoslova);
                ctx.SaveChanges();
                TempData[Constants.Message] = $"Kategorija posla {id} obrisan.";
                TempData[Constants.ErrorOccurred] = false;
            }
            catch(Exception exc){
                TempData[Constants.Message] = "Greška prilikom brisanja kategorije posla."+ exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
    }
     [HttpGet]
    public IActionResult Edit(int Id, int page = 1, int sort = 1, bool ascending = true)
    {
        var kategorijaPoslova = ctx.KategorijaPoslova.AsNoTracking().Where(c => c.Id == Id).SingleOrDefault();
        if(kategorijaPoslova==null){
            return NotFound($"Ne postoji kategorija posla s oznakom: " + Id);
        }
        else{
            ViewBag.Page = page;
            ViewBag.Sort = sort;
            ViewBag.Ascending = ascending;
            return View(kategorijaPoslova);
        }
    }
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int Id, int page = 1, int sort = 1, bool ascending = true){
        try{
            KategorijaPoslova kategorijaPoslova = await ctx.KategorijaPoslova.FindAsync(Id);
            if(kategorijaPoslova==null){
                return NotFound($"Ne postoji kategorija posla s oznakom: " + Id);
            }
            ViewBag.Page=page;
            ViewBag.Sort=sort;
            ViewBag.Ascending=ascending;
        bool ok =   await TryUpdateModelAsync<KategorijaPoslova>(kategorijaPoslova, "",kp=> kp.Naziv);
        if(ok){
            try{
                await ctx.SaveChangesAsync();
                TempData[Constants.Message] = $"Kategorija posla  {kategorijaPoslova.Naziv} ažurirana.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });

            }
            catch(Exception exc){
            ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
            return View(kategorijaPoslova);
            }
        }
        else{
            ModelState.AddModelError(string.Empty, "Podatke o kategorijama poslova nije moguce povezati");
            return View(kategorijaPoslova);
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
