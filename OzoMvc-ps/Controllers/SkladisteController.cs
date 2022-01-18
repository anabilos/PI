using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OzoMvc.Extensions;
using OzoMvc.Models;
using OzoMvc.ViewModels;

namespace OzoMvc.Controllers
{
    public class SkladisteController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<SkladisteController> logger;

        public SkladisteController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<SkladisteController> logger)
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
        public IActionResult Index(int page = 1, int sort = 1, bool ascending = true)
        {
            int pagesize = appSettings.PageSize;

            var query = ctx.Skladište
                        .AsNoTracking();

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
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, sort, ascending });
            }

            System.Linq.Expressions.Expression<Func<Skladište, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = rt => rt.Naziv;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var skladiste = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new SkladisteViewModel
            {
                Skladište = skladiste,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(String id, int page = 1, int sort = 1, bool ascending = true)
        {
            var skladište = ctx.Skladište.AsNoTracking().Where(vo => vo.Naziv == id).SingleOrDefault();
            if (skladište == null)
            {
                logger.LogWarning("Ne postoji skladiste s nazivom: {0} ", id);
                return NotFound("Ne postoji skladiste s nazivom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(skladište);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, int page = 1, int sort = 1, bool ascending = true)
        {
            //za različite mogućnosti ažuriranja pogledati
            //attach, update, samo id, ...
            //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/crud#update-the-edit-page

            try
            {
                Skladište skladište = await ctx.Skladište.FindAsync(id);
                if (skladište == null)
                {
                    return NotFound("Neispravna naziv referentnog tipa: " + id);
                }

                if (await TryUpdateModelAsync<Skladište>(skladište, "",
                    s => s.Naziv
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "Skladiste ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(skladište);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o skladistu nije moguće povezati s forme");
                    return View(skladište);
                }
            }
            catch (Exception exc)
            {
                TempData[Constants.Message] = exc.CompleteExceptionMessage();
                TempData[Constants.ErrorOccurred] = true;
                return RedirectToAction(nameof(Edit), id);
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
        public IActionResult Create(Skladište skladište)
        {
            logger.LogTrace(JsonSerializer.Serialize(skladište));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(skladište);
                    ctx.SaveChanges();

                    logger.LogInformation(new EventId(1000), $"skladiste {skladište.Naziv} dodan.");

                    TempData[Constants.Message] = $"skladiste {skladište.Naziv} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje novog refernetnog tipa: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(skladište);
                }
            }
            else
            {
                return View(skladište);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string Naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            var skladiste = ctx.Skladište
                             .AsNoTracking()
                             .Where(rt => rt.Naziv == Naziv)
                             .SingleOrDefault();
            if (skladiste != null)
            {
                try
                {
                    string naziv = skladiste.Naziv;
                    ctx.Remove(skladiste);
                    ctx.SaveChanges();
                    logger.LogInformation($"Skladiste {naziv} uspješno obrisana");
                    TempData[Constants.Message] = $"Skladiste {naziv} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja skladista " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;

                    logger.LogError("Pogreška prilikom brisanja skladista: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji skladiste s oznakom: {0} ", Naziv);
                TempData[Constants.Message] = "Ne postoji skladiste s oznakom: " + Naziv;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
        private void PrepareDropDownLists()
        {
            var mjesto = ctx.Mjesto
                            .OrderBy(vo => vo.Naziv)
                            .Select(vo => new { vo.Id, vo.Naziv })
                            .ToList();

            ViewBag.Mjesto = new SelectList(mjesto, "Id", "Naziv");

        }
    }
}