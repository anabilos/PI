using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using OzoMvc.Extensions;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.Controllers
{
    public class OpremaController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<OpremaController> logger;

        public OpremaController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<OpremaController> logger)
        {
            this.ctx = ctx;
            appSettings = optionsSnapshot.Value;
            this.logger = logger;
        }
        //public IActionResult Index()
        //{
        //  var oprema = ctx.Oprema
        //                .AsNoTracking()
        //              .OrderBy(o => o.Naziv)
        //            .ToList();
        //return View("SimpleIndex", oprema);
        //}
        public IActionResult Index(string filter,int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Oprema
                        .AsNoTracking();

            int count = query.Count();
            OpremaFilter of = new OpremaFilter();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                of = OpremaFilter.FromString(filter);
                if (!of.IsEmpty())
                {
                    
                    query = of.Apply(query);
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
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            System.Linq.Expressions.Expression<Func<Oprema, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = o => o.Naziv;
                    break;
                case 2:
                    orderSelector = o => o.KupovnaVrijednost;
                    break;
                case 3:
                    orderSelector = o => o.KnjigovodstvenaVrijednost;
                    break;
                case 4:
                    orderSelector = o => o.VrijemeAmortizacije;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var oprema = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new OpremaViewModel
            {
                Oprema = oprema,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        public IActionResult Filter(OpremaFilter filter)
        {
            return RedirectToAction(nameof(Index), new { filter = filter.ToString() });
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            PrepareDropDownLists();
            var oprema = ctx.Oprema.AsNoTracking().Where(o => o.InventarniBroj==id).SingleOrDefault();
            if (oprema == null)
            {
                logger.LogWarning("Ne postoji oprema s nazivom: {0} ", oprema.Naziv);
                return NotFound("Ne postoji oprema s nazivom: " + oprema.Naziv);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(oprema);
            }
        }

        /*  public async Task<IActionResult> Update(int id, int page = 1, int sort = 1, bool ascending = true)
          {
              //za različite mogućnosti ažuriranja pogledati
              //attach, update, samo id, ...
              //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud#update-the-edit-page

              try
              {
                  Oprema oprema = await ctx.Oprema.FindAsync(id);
                  if (oprema == null)
                  {
                      return NotFound("Neispravna naziv Opreme: " + id);
                  }

                  if (await TryUpdateModelAsync<Oprema>(oprema, "",
                      o => o.Naziv, o => o.KnjigovodstvenaVrijednost, o => o.VrijemeAmortizacije
                  ))
                  {
                      ViewBag.Page = page;
                      ViewBag.Sort = sort;
                      ViewBag.Ascending = ascending;
                      try
                      {
                          await ctx.SaveChangesAsync();
                          TempData[Constants.Message] = "Oprema ažurirana.";
                          TempData[Constants.ErrorOccurred] = false;
                          return RedirectToAction(nameof(Index), new { page, sort, ascending });
                      }
                      catch (Exception exc)
                      {
                          ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                          return View(oprema);
                      }
                  }
                  else
                  {
                      ModelState.AddModelError(string.Empty, "Podatke o opremi nije moguće povezati s forme");
                      return View(oprema);
                  }
              }
              catch (Exception exc)
              {
                  TempData[Constants.Message] = exc.CompleteExceptionMessage();
                  TempData[Constants.ErrorOccurred] = true;
                  return RedirectToAction(nameof(Edit), id);
              }
          }*/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Oprema oprema, int page = 1, int sort = 1, bool ascending = true)
        {
            if (oprema == null)
            {
                return NotFound("Nema poslanih podataka");
            }
            bool checkId = ctx.Oprema.Any(m => m.InventarniBroj == oprema.InventarniBroj);
            if (!checkId)
            {
                return NotFound($"Neispravan inventarni broj opreme: {oprema?.InventarniBroj}");
            }

            PrepareDropDownLists();
            if (ModelState.IsValid)
            {
                try
                { 

                    ctx.Update(oprema);
                    ctx.SaveChanges();

                    TempData[Constants.Message] = $"Oprema {oprema.Naziv} ažurirano.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index), new { page, sort, ascending });
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(oprema);
                }
            }
            else
            {
                return View(oprema);
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            PrepareDropDownLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Oprema oprema)
        {
            logger.LogTrace(JsonSerializer.Serialize(oprema));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(oprema);
                    ctx.SaveChanges();

                    logger.LogInformation(new EventId(1000), $"Oprema {oprema.Naziv} dodana.");

                    TempData[Constants.Message] = $"Oprema {oprema.Naziv} dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove opreme: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(oprema);
                }
            }
            else
            {
                return View(oprema);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int page = 1, int sort = 1, bool ascending = true)
        {
            var oprema = ctx.Oprema
                             .AsNoTracking()
                             .Where(o => o.InventarniBroj  == id)
                             .SingleOrDefault();
            if (oprema != null)
            {
                try
                {
                   
                    ctx.Remove(oprema);
                    ctx.SaveChanges();
                    logger.LogInformation($"Oprema {id} uspješno obrisana");
                    TempData[Constants.Message] = $"Oprema {id} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;

                    logger.LogError("Pogreška prilikom brisanja opreme: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji opremea s oznakom: {0} ",id);
                TempData[Constants.Message] = "Ne postoji oprema s oznakom: " +id ;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
        private void PrepareDropDownLists()
        {
            var vrstaopreme = ctx.VrstaOpreme
                            .OrderBy(vo => vo.Naziv)
                            .Select(vo => new { vo.Id, vo.Naziv})
                            .ToList();

            ViewBag.VrstaOpreme = new SelectList(vrstaopreme, "Id", "Naziv");

            var rt = ctx.ReferentniTip
                            .OrderBy(rt => rt.Naziv)
                            .Select(rt => new { rt.Id, rt.Naziv })
                            .ToList();
           
            ViewBag.ReferentniTip = new SelectList(rt, "Id", "Naziv");

            var s = ctx.Skladište
                            .OrderBy(s => s.Id)
                            .Select(s => new { s.Id, s.Naziv })
                            .ToList();

            ViewBag.Skladiste = new SelectList(s, "Id", "Naziv");
        }
    }
}