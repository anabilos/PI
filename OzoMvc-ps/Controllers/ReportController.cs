using OzoMvc.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfRpt.ColumnsItemsTemplates;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PI.Controllers
{
  public class ReportController : Controller
  {
    private readonly PI05Context ctx;

    public ReportController(PI05Context ctx)
    {
      this.ctx = ctx;
    }

    public IActionResult Index()
    {
      return View();
    }

    public async Task<IActionResult> Usluge()
    {
      string naslov = "Deset najskupljih usluga";
      var usluge = await ctx.Usluga
                            .AsNoTracking()
                            .OrderByDescending(d => d.Cijena)
                            .Take(10)
                            .ToListAsync();

      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); 
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(usluge));

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
       {
          column.PropertyName(nameof(Usluga.Id));
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(2);
          column.HeaderCell("Id usluge");
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Usluga>(x => x.Naziv);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(2);
          column.Width(3);
          column.HeaderCell("Naziv usluge", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Usluga>(x => x.Cijena);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(1);
          column.HeaderCell("Cijena", horizontalAlignment: HorizontalAlignment.Center);
           column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  
        });

      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=usluge.pdf");
        return File(pdf, "application/pdf");
        
      }
      else
        return NotFound();
    }
       public async Task<IActionResult> Opreme1()
    {
      string naslov = "Deset najskuplji oprema";
      var opreme = await ctx.Oprema
                            .AsNoTracking()
                            .OrderByDescending(d => d.KupovnaVrijednost)
                            .Take(10)
                            .ToListAsync();

      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); 
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(opreme));

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
       {
          column.PropertyName(nameof(Oprema.InventarniBroj));
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(2);
          column.HeaderCell("Inventarni broj");
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.Naziv);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(2);
          column.Width(3);
          column.HeaderCell("Naziv", horizontalAlignment: HorizontalAlignment.Center);
        });
            columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.KnjigovodstvenaVrijednost);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(1);
          column.HeaderCell("Knjigovodstvena vrijednost", horizontalAlignment: HorizontalAlignment.Center);
          column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  
        });

  
                  

      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=oprema1.pdf");
        return File(pdf, "application/pdf");
       
      }
      else
        return NotFound();
    }
     public async Task<IActionResult> Radnici()
    {
      string naslov = "Deset najplaćenijih radnika";
      var zaposlenici = await ctx.Zaposlenik
                            .AsNoTracking()
                            .OrderByDescending(d => d.MjesecniTrosak)
                            .Take(10)
                            .ToListAsync();

      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); 
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(zaposlenici));

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
       {
          column.PropertyName(nameof(Zaposlenik.Id));
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(2);
          column.HeaderCell("Id zaposlenika");
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Zaposlenik>(x => x.Ime);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(2);
          column.Width(3);
          column.HeaderCell("Ime", horizontalAlignment: HorizontalAlignment.Center);
        });
            columns.AddColumn(column =>
        {
          column.PropertyName<Zaposlenik>(x => x.Prezime);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(3);
          column.HeaderCell("Prezime", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Zaposlenik>(x => x.MjesecniTrosak);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(4);
          column.Width(1);
          column.HeaderCell("Mjesečni trošak", horizontalAlignment: HorizontalAlignment.Center);
           column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  
        });

      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=zaposlenici.pdf");
        return File(pdf, "application/pdf");
       
      }
      else
        return NotFound();
    }
    public async Task<IActionResult> Opreme()
    {
      string naslov = "Popis opreme s knjigovodstvenom vrijednosti nula";
      var opreme = await ctx.Oprema
                            .AsNoTracking()
                            .Where(d=> d.KnjigovodstvenaVrijednost.Value==0)
                            .OrderBy(d => d.Naziv)
                            .ToListAsync();
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true); // It's a default setting to improve the performance.
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(opreme));

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
           column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
       {
          column.PropertyName(nameof(Oprema.InventarniBroj));
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(1);
          column.HeaderCell("Inventarni broj");
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.Naziv);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(2);
          column.Width(2);
          column.HeaderCell("Naziv opreme", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.KupovnaVrijednost);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(1);
          column.HeaderCell("Cijena", horizontalAlignment: HorizontalAlignment.Center);
           column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  
          
        });
          columns.AddColumn(column =>
        {
          column.PropertyName<Oprema>(x => x.VrijemeAmortizacije);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(4);
          column.Width(1);
          column.HeaderCell("Vrijeme amortizacije", horizontalAlignment: HorizontalAlignment.Center); 
          
        });
           

      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=oprema.pdf");
        return File(pdf, "application/pdf");
        //return File(pdf, "application/pdf", "ref0.pdf"); //Otvara save as dialog
      }
      else
        return NotFound();
    }

  
    #region Private methods
    private PdfReport CreateReport(string naslov)
    {
      var pdf = new PdfReport();

      pdf.DocumentPreferences(doc =>
      {
        doc.Orientation(PageOrientation.Portrait);
        doc.PageSize(PdfPageSize.A4);
        doc.DocumentMetadata(new DocumentMetadata
        {
          Author = "FSRE",
          Application = "OZO",
          Title = naslov
        });
        doc.Compression(new CompressionSettings
        {
          EnableCompression = true,
          EnableFullCompression = true
        });
      })
      .MainTableTemplate(template =>
      {
        template.BasicTemplate(BasicTemplate.ProfessionalTemplate);
      })
      .MainTablePreferences(table =>
      {
        table.ColumnsWidthsType(TableColumnWidthType.Relative);
        //table.NumberOfDataRowsPerPage(20);
        table.GroupsPreferences(new GroupsPreferences
        {
          GroupType = GroupType.HideGroupingColumns,
          RepeatHeaderRowPerGroup = true,
          ShowOneGroupPerPage = true,
          SpacingBeforeAllGroupsSummary = 5f,
          NewGroupAvailableSpacingThreshold = 150,
          SpacingAfterAllGroupsSummary = 5f
        });
        table.SpacingAfter(4f);
      });

      return pdf;
    }
    #endregion

     public async Task<IActionResult> Poslovi()
    {
      string naslov = "Ispis profitabilnosti sklopljenih poslova";
      var posao = await ctx.Posao
                            .AsNoTracking()
                            .OrderBy(d => d.Id)
                            .ToListAsync();
      PdfReport report = CreateReport(naslov);
      #region Podnožje i zaglavlje
      report.PagesFooter(footer =>
      {
        footer.DefaultFooter(DateTime.Now.ToString("dd.MM.yyyy."));
      })
      .PagesHeader(header =>
      {
        header.CacheHeader(cache: true);
        header.DefaultHeader(defaultHeader =>
        {
          defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
          defaultHeader.Message(naslov);
        });
      });
      #endregion
      #region Postavljanje izvora podataka i stupaca
      report.MainTableDataSource(dataSource => dataSource.StronglyTypedList(posao));

      report.MainTableColumns(columns =>
      {
        columns.AddColumn(column =>
        {
          column.IsRowNumber(true);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(0);
          column.Width(1);
          column.HeaderCell("#", horizontalAlignment: HorizontalAlignment.Center);
        });

        columns.AddColumn(column =>
       {
          column.PropertyName(nameof(Posao.Vrijeme));
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(1);
          column.Width(1);
          column.HeaderCell("Vrijeme održavanja");
        });

         columns.AddColumn(column =>
        {
          column.PropertyName<Posao>(x => x.Cijena);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(2);
          column.Width(1);
          column.HeaderCell("Cijena", horizontalAlignment: HorizontalAlignment.Center);
           column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  

        columns.AddColumn(column =>
        {
          column.PropertyName<Posao>(x => x.Troskovi);
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);
          column.Width(1);
          column.HeaderCell("Troškovi", horizontalAlignment: HorizontalAlignment.Center);
           column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });  
          
        });
         columns.AddColumn(column =>
        {
          column.PropertyName("Dobit/Gubitak");
          column.CellsHorizontalAlignment(HorizontalAlignment.Center);
          column.IsVisible(true);
          column.Order(3);         
          column.Width(1);
          column.HeaderCell("Dobit/Gubitak", horizontalAlignment: HorizontalAlignment.Center);
          column.ColumnItemsTemplate(template =>
          {
            template.TextBlock();
            template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });
          column.AggregateFunction(aggregateFunction =>
          {
            aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
            aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                                ? string.Empty : string.Format("{0} KM", obj));
          });
          column.CalculatedField(
                        list =>
                        {
                          if (list == null) return string.Empty;
                          double cijena = (double)list.GetValueOf(nameof(Posao.Cijena));
                          double troskovi = (double)list.GetValueOf(nameof(Posao.Troskovi));
                          var iznos = cijena-troskovi;
                          return iznos;
                        });
        });
      });
           

      });

      #endregion      
      byte[] pdf = report.GenerateAsByteArray();

      if (pdf != null)
      {
        Response.Headers.Add("content-disposition", "inline; filename=oprema.pdf");
        return File(pdf, "application/pdf");
        //return File(pdf, "application/pdf", "poslovi.pdf"); //Otvara save as dialog
      }
      else
        return NotFound();
    }
  }
}