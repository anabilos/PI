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
    public class ReferentniTipController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;
        private readonly ILogger<ReferentniTipController> logger;

        public ReferentniTipController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot, ILogger<ReferentniTipController> logger)
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

            var query = ctx.ReferentniTip
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

            System.Linq.Expressions.Expression<Func<ReferentniTip, object>> orderSelector = null;
            switch (sort)
            {
                case 1:
                    orderSelector = rt => rt.Naziv;
                    break;
                case 2:
                    orderSelector = rt => rt.Cijena;
                    break;
            }

            if (orderSelector != null)
            {
                query = ascending ?
                       query.OrderBy(orderSelector) :
                       query.OrderByDescending(orderSelector);
            }
            var referentnitip = query
                        .Skip((page - 1) * pagesize)
                        .Take(pagesize)
                        .ToList();
            var model = new ReferentniTipViewModel
            {
                ReferentniTip = referentnitip,
                PagingInfo = pagingInfo
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(String id, int page = 1, int sort = 1, bool ascending = true)
        {
            var referentnitip = ctx.ReferentniTip.AsNoTracking().Where(vo => vo.Naziv == id).SingleOrDefault();
            if (referentnitip == null)
            {
                logger.LogWarning("Ne postoji referentni tip s nazivom: {0} ", id);
                return NotFound("Ne postoji referentni tip s nazivom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Sort = sort;
                ViewBag.Ascending = ascending;
                return View(referentnitip);
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
                ReferentniTip referentnitip = await ctx.ReferentniTip.FindAsync(id);
                if (referentnitip == null)
                {
                    return NotFound("Neispravna naziv referentnog tipa: " + id);
                }

                if (await TryUpdateModelAsync<ReferentniTip>(referentnitip, "",
                    rt => rt.Naziv
                ))
                {
                    ViewBag.Page = page;
                    ViewBag.Sort = sort;
                    ViewBag.Ascending = ascending;
                    try
                    {
                        await ctx.SaveChangesAsync();
                        TempData[Constants.Message] = "referentni tip ažurirana.";
                        TempData[Constants.ErrorOccurred] = false;
                        return RedirectToAction(nameof(Index), new { page, sort, ascending });
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                        return View(referentnitip);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podatke o referentnom tipu nije moguće povezati s forme");
                    return View(referentnitip);
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
        public IActionResult Create(ReferentniTip referentnitip)
        {
            logger.LogTrace(JsonSerializer.Serialize(referentnitip));
            if (ModelState.IsValid)
            {
                try
                {
                    ctx.Add(referentnitip);
                    ctx.SaveChanges();

                    logger.LogInformation(new EventId(1000), $"Referentni tip {referentnitip.Naziv} dodan.");

                    TempData[Constants.Message] = $"Referentni tip {referentnitip.Naziv} dodan.";
                    TempData[Constants.ErrorOccurred] = false;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exc)
                {
                    logger.LogError("Pogreška prilikom dodavanje novog refernetnog tipa: {0}", exc.CompleteExceptionMessage());
                    ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                    return View(referentnitip);
                }
            }
            else
            {
                return View(referentnitip);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string Naziv, int page = 1, int sort = 1, bool ascending = true)
        {
            var referentnitip = ctx.ReferentniTip
                             .AsNoTracking()
                             .Where(rt => rt.Naziv == Naziv)
                             .SingleOrDefault();
            if (referentnitip != null)
            {
                try
                {
                    string naziv = referentnitip.Naziv;
                    ctx.Remove(referentnitip);
                    ctx.SaveChanges();
                    logger.LogInformation($"Referentni tip {naziv} uspješno obrisana");
                    TempData[Constants.Message] = $"Referentni tip {naziv} uspješno obrisana";
                    TempData[Constants.ErrorOccurred] = false;
                }
                catch (Exception exc)
                {
                    TempData[Constants.Message] = "Pogreška prilikom brisanja referentnog tipa " + exc.CompleteExceptionMessage();
                    TempData[Constants.ErrorOccurred] = true;

                    logger.LogError("Pogreška prilikom brisanja referentnog tipa: " + exc.CompleteExceptionMessage());
                }
            }
            else
            {
                logger.LogWarning("Ne postoji referentni tip s oznakom: {0} ", Naziv);
                TempData[Constants.Message] = "Ne postoji referentni tip s oznakom: " + Naziv;
                TempData[Constants.ErrorOccurred] = true;
            }
            return RedirectToAction(nameof(Index), new { page = page, sort = sort, ascending = ascending });
        }
    }
}