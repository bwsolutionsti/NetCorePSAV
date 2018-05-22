using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCorePSAV.Controllers
{
    public class CRatioController : Controller
    {
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;
        public CRatioController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult ExportCRResult(NetCorePSAV.Models.NCRModel.SearchNCR nCR)
        {
            GCCorePSAV.Data.clsQuery cls = new GCCorePSAV.Data.clsQuery();
            try
            {
                var fileDownloadName = "CaptureRatio.xlsx";
                var reportsFolder = "reports";
                var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
                byte[] ReportArray;
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ews = package.Workbook.Worksheets[1];
                    //obtiene lista entera de consulta de ncr
                    List<NetCorePSAV.Models.NCRModel.NCRReporte> reportes = cls.GetCRReportes(nCR);
                    //llena campos de lista ncr
                    for(int i = 0; i < reportes.Count; i++)
                    {
                        ews.Cells["A" + (i + 3).ToString()].Value = reportes[i].nloc;
                        ews.Cells["b" + (i + 3).ToString()].Value = reportes[i].numloc;
                        ews.Cells["c" + (i + 3).ToString()].Value = reportes[i].region;
                        ews.Cells["d" + (i + 3).ToString()].Value = reportes[i].ciudad;
                        ews.Cells["e" + (i + 3).ToString()].Value = reportes[i].det;
                        ews.Cells["f" + (i + 3).ToString()].Value = reportes[i].smgr;
                        ews.Cells["g" + (i + 3).ToString()].Value = reportes[i].prospecto;
                        ews.Cells["h" + (i + 3).ToString()].Value = reportes[i].empresa;
                        ews.Cells["i" + (i + 3).ToString()].Value = reportes[i].correo;
                        ews.Cells["j" + (i + 3).ToString()].Value = reportes[i].telefono;
                        ews.Cells["k" + (i + 3).ToString()].Value = reportes[i].etiqueta;
                        ews.Cells["l" + (i + 3).ToString()].Value = reportes[i].tipoevento;
                        ews.Cells["m" + (i + 3).ToString()].Value = reportes[i].nomevento;
                        ews.Cells["n" + (i + 3).ToString()].Value = reportes[i].finicio;
                        ews.Cells["o" + (i + 3).ToString()].Value = reportes[i].ffin;
                        ews.Cells["p" + (i + 3).ToString()].Value = reportes[i].ciaav;
                        ews.Cells["q" + (i + 3).ToString()].Value = reportes[i].lb;
                        ews.Cells["r" + (i + 3).ToString()].Value = reportes[i].lbm;
                        ews.Cells["s" + (i + 3).ToString()].Value = reportes[i].servadic;
                        ews.Cells["t" + (i + 3).ToString()].Value = reportes[i].lpe;
                        ews.Cells["u" + (i + 3).ToString()].Value = reportes[i].fpe;
                        ews.Cells["v" + (i + 3).ToString()].Value = reportes[i].comentarios;
                    }
                    //termina excel
                    ReportArray = package.GetAsByteArray();
                }
                return File(ReportArray, XlsxContentType, "CaptureRatio_" + System.DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
            catch (Exception ex) { ViewBag.datashow = ex.Message; return View(); }
        }
        [HttpPost]
        public IActionResult CaptureRatio(NetCorePSAV.Models.NCRModel.SearchNCR nCR, string Consulta, string Exporta)
        {
            GCCorePSAV.Data.clsQuery cls = new GCCorePSAV.Data.clsQuery();
            if (!string.IsNullOrEmpty(Exporta))
            {
                return RedirectToAction("ExportCRResult", nCR);
            }
            if (Consulta.Equals("1"))
            {
                List<NetCorePSAV.Models.NCRModel.PreConsultaNCR> preConsultaNCRs = cls.GetPreConsultaNCRs(nCR);
                if (preConsultaNCRs.Count > 0)
                {
                    ViewBag.TieneFilas = true; //Location - Cascade
                    ViewBag.Locations = cls.GetNLocations();
                    //MotivoLb
                    ViewBag.LBMotivo = cls.GetMotivos();
                }
                else
                {
                    ViewBag.Locations = cls.GetNLocations();
                    //MotivoLb
                    ViewBag.LBMotivo = cls.GetMotivos();
                    ViewBag.TieneFilas = false;
                }
                ViewBag.CRResult = preConsultaNCRs;
            }
            return View(nCR);
        }
        // GET: /<controller>/
        public IActionResult CaptureRatio()
        {
            GCCorePSAV.Data.clsQuery cls = new GCCorePSAV.Data.clsQuery();
            ViewBag.TieneFilas = false;
            //Location - Cascade
            ViewBag.Locations = cls.GetNLocations();
            //MotivoLb
            ViewBag.LBMotivo = cls.GetMotivos();
            return View();
        }
        public IActionResult NewCR()
        {
            GCCorePSAV.Data.clsQuery cls = new GCCorePSAV.Data.clsQuery();
            //get all data from DB
            //DET
            ViewBag.DET = cls.GetDETs();
            //SAles Manager
            ViewBag.RepVtas = cls.RepVtas();
            //Location - Cascade
            ViewBag.Locations = cls.GetNLocations();
            //Etiqueta
            ViewBag.Etiquetas = cls.GetEtiquetas();
            //TipoEvento
            ViewBag.TipoEvento = cls.GetTipoEventos();
            //MotivoLb
            ViewBag.LBMotivo = cls.GetMotivos();
            //Sadic
            ViewBag.sadic = cls.GetSadics();
            return View();
        }
        [HttpPost]
        public IActionResult NewCR(NetCorePSAV.Models.NCRModel.newCRView NCR)
        {
            GCCorePSAV.Data.clsQuery cls = new GCCorePSAV.Data.clsQuery();
            if (ModelState.IsValid)
            {
                cls.SaveNCR(NCR);
                return RedirectToAction("CaptureRatio");
            }
            else
            {
                //get all data from DB
                //DET
                ViewBag.DET = cls.GetDETs();
                //SAles Manager
                ViewBag.RepVtas = cls.RepVtas();
                //Location - Cascade
                ViewBag.Locations = cls.GetNLocations();
                //Etiqueta
                ViewBag.Etiquetas = cls.GetEtiquetas();
                //TipoEvento
                ViewBag.TipoEvento = cls.GetTipoEventos();
                //MotivoLb
                ViewBag.LBMotivo = cls.GetMotivos();
                //Sadic
                ViewBag.sadic = cls.GetSadics();
                return View();
            }
        }
    }
}
