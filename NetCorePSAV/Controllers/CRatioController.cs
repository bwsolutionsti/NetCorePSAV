using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCorePSAV.Controllers
{
    public class CRatioController : Controller
    {
        // GET: /<controller>/
        public IActionResult CaptureRatio()
        {
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
