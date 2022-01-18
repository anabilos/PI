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
    public class VrstaOpremeController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<VrstaOpremeController> logger;

        public VrstaOpremeController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<VrstaOpremeController> logger)
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

            var query = ctx.VrstaOpreme
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

            System.Linq.Expressions.Expression<Func<VrstaOpreme, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = vo => vo.Naziv;
                    break;
            }
            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var vrstaopreme = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new VrsatOpremeViewModel
            {
                VrstaOpreme = vrstaopreme,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(String id, int page = 1, int sort = 1, bool ascending = true)
        {
            var vrstaoprme = ctx.VrstaOpreme.AsNoTracking().Where(vo => vo.Naziv == id).SingleOrDefault();
            if (vrstaoprme == null)
            {
                logger.LogWarning("Ne postoji oprema s nazivom: {0} ", id);
                return NotFound("Ne postoji oprema s nazivom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(vrstaoprme);
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
                VrstaOpreme vrstaopreme = await ctx.VrstaOpreme.FindAsync(id);
                if (vrstaopreme == null)
                {
                    return NotFound("Neispravna naziv Opreme: " + id);
                }

                if (await TryUpdateModelAsync<VrstaOpreme>(vrstaopreme, "",
                    vo => vo.Naziv
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
                        return View(vrstaopreme);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o opremi nije moguće povezati s forme");
                    return View(vrstaopreme);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VrstaOpreme vrstaopreme)
        {
            logger.LogTrace(JsonSerializer.Serialize(vrstaopreme));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(vrstaopreme);
                    ctx.SaveChanges();

                    logger.LogInformation(new EventId(1000), $"Oprema {vrstaopreme.Naziv} dodana.");

                    TempData[Constants.Message] = $"Oprema {vrstaopreme.Naziv} dodana.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje nove opreme: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(vrstaopreme);
                }
            }
            else
            {
                return View(vrstaopreme);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string Naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            var vrstaopreme = ctx.VrstaOpreme
                             .AsNoTracking()
                             .Where(vo => vo.Naziv == Naziv)
                             .SingleOrDefault();
            if (vrstaopreme != null)
            {
                try
                {
                    string naziv = vrstaopreme.Naziv;
                    ctx.Remove(vrstaopreme);
                    ctx.SaveChanges();
                    logger.LogInformation($"Vrsta opreme {naziv} uspješno obrisana");
                    TempData[Constants.Message] = $"Vrsat opreme {naziv} uspješno obrisana";
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
                logger.LogWarning("Ne postoji vrsta opreme s oznakom: {0} ", Naziv);
                TempData[Constants.Message] = "Ne postoji vrsta opreme s oznakom: " + Naziv;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
    }
}