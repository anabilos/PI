using OzoMvc.Extensions;
using OzoMvc.Models;
using OzoMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OzoMvc.Controllers
{
    public class CertifikatZaposlenikController : Controller
    {
        private readonly PI05Context ctx;
        private readonly AppSettings appSettings;

        public CertifikatZaposlenikController(PI05Context ctx, IOptionsSnapshot<AppSettings> optionsSnapshot){
            this.ctx=ctx;
            appSettings = optionsSnapshot.Value;
        }
        
        [HttpGet]
    public IActionResult Create()
    {
        PrepareDropDownLists();
      return View();
    }

        private void PrepareDropDownLists()
        {
            var certifikati = ctx.Certifikati
                                .Where(c=>c.Id!=7)
                                .OrderBy(c=>c.Naziv)
                                .Select(c=> new {c.Id,c.Naziv})
                                .ToList();

            var zastitar = ctx.Certifikati
                                .Where(c=>c.Id!=7)
                                .Select(c=> new {c.Id,c.Naziv})
                                .FirstOrDefault();
                                
            if(zastitar!=null){
                certifikati.Insert(0,zastitar);
            }
            ViewBag.Certifikati = new SelectList(certifikati, nameof(Certifikati.Id), nameof(Certifikati.Naziv));
        }

        [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CertifikatZaposlenik katzap){
        if(ModelState.IsValid){
            try{
                ctx.Add(katzap);
                ctx.SaveChanges();
                TempData[Constants.Message] = $"CertifikatZaposlenik {katzap.Id} dodan.";
                TempData[Constants.ErrorOccurred] = false;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception exc){
                ModelState.AddModelError(string.Empty, exc.CompleteExceptionMessage());
                PrepareDropDownLists();
                return View(katzap);
            }
        }
        else{
            return View(katzap);
        }
    }
    
    } 
}
