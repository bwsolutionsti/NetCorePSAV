using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.JavaScript;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;
using System.Linq.Expressions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GCCorePSAV.Controllers
{

    public class PSAVController : Controller
    {
        // GET: /<controller>/
        Data.clsQuery ConSQL = new Data.clsQuery();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EPT()
        {
            ViewBag.CRList = ConSQL.EPTSearch();
            return View();
        }
        [HttpPost]
        public IActionResult EPT(string ac1)
        {
            ViewBag.CRList = ConSQL.EPTSearchOne(ac1);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EPT1(object model)
        {
            return View();
        }
        public IActionResult NewEPT()//aqui estamos contestando a un request y le estamos regresando la vista newept con toda la info necesaria
        {
            List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
            List<Models.EPTModel.pricelist> PriceList = ConSQL.GetPriceList();
            List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
            ViewBag.ClientList = ClientList;
            ViewBag.CoinsList = CoinsList;
            ViewBag.PriceList = PriceList;
            ViewBag.EPTNumberFormat = ConSQL.GetEPTNumber(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request.Cookies["pandoraRules"].ToString())));
            ViewBag.RepVtas = ConSQL.RepVtas();
            ViewBag.Productores = ConSQL.Productores();
            return View(new Models.EPTModel());
        }
        [HttpPost]
        public IActionResult NewEPT(string ac1, Models.EPTModel model, string NewValue, string folio)
        {
            ViewBag.EPTNumberFormat = ConSQL.GetEPTNumber(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Request.Cookies["pandoraRules"].ToString())));
            if (NewValue.Equals("0"))
            {
                Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
                List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
                List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
                ViewBag.ClientList = ClientList;
                ViewBag.CoinsList = CoinsList;
                ViewBag.RepVtas = ConSQL.RepVtas();
                ViewBag.Productores = ConSQL.Productores();
                List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClientID(eptM.IDClient);
                for (int i = 0; i < Clients.Count; i++)
                {
                    eptM.DomicilioComercial = Clients[i].Domicilio;
                    eptM.DomicilioFiscal = Clients[i].Fiscal;
                    eptM.IDClient = Clients[i].IDClient;
                    eptM.RazonSocial = Clients[i].Razon;
                    eptM.Phone = Clients[i].Tel;
                    eptM.Mobile = Clients[i].Cel;
                    eptM.Email = Clients[i].Email;
                    eptM.RFC = Clients[i].RFC;
                    eptM.Ext = Clients[i].Ext;
                    eptM.ContactClientName = Clients[i].NombreContacto;
                }
                return View(eptM);
            }
            else
            {
                if (!string.IsNullOrEmpty(ac1))
                {
                    List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
                    List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
                    List<Models.EPTModel.pricelist> pricelists = ConSQL.GetPriceList();
                    ViewBag.ClientList = ClientList;
                    ViewBag.CoinsList = CoinsList;
                    ViewBag.Pricelist = pricelists;
                    ViewBag.RepVtas = ConSQL.RepVtas();
                    ViewBag.Productores = ConSQL.Productores();
                    List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClient(ac1);
                    Models.EPTModel ept = new Models.EPTModel();
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        ept.DomicilioComercial = Clients[i].Domicilio;
                        ept.DomicilioFiscal = Clients[i].Fiscal;
                        ept.IDClient = Clients[i].IDClient;
                        ept.RazonSocial = Clients[i].Razon;
                        ept.Phone = Clients[i].Tel;
                        ept.Mobile = Clients[i].Cel;
                        ept.Email = Clients[i].Email;
                        ept.RFC = Clients[i].RFC;
                        ept.Ext = Clients[i].Ext;
                        ept.ContactClientName = Clients[i].NombreContacto;
                        ept.IDEmpresa = Clients[i].IDEmpresa;
                    }
                    Response.Cookies.Append("IDC", ept.IDClient, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("IDCE", ept.IDEmpresa, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });

                    return View(ept);
                }
                else
                {
                    if (ModelState.IsValid)//aqui es donde se da lo de newept
                    {
                        //save model
                        model.IDClient = Request.Cookies["IDC"].ToString();
                        model.IDEmpresa = Request.Cookies["IDCE"].ToString();
                        string IDEvent = ConSQL.InsertEPT(model);
                        ViewBag.EPTM = model;
                        Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
                        Ilem.EPTLoad = ViewBag.EPTM;
                        Response.Cookies.Append("IDE", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                        Response.Cookies.Append("EVN", model.EventName, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                        ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
                        ViewBag.SubcategoriaList = ConSQL.GetSubcategoryItemList(1);
                        Response.Cookies.Append("PriceL", model.TpListaPrecio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                        return RedirectToAction("NIL");
                    }
                    else
                    {
                        return View();

                    }
                }
            }
        }
        public static List<NetCorePSAV.Models.ILNewModel.ILGrid> ListGridIL = new List<NetCorePSAV.Models.ILNewModel.ILGrid>();
        public ActionResult ILU([FromBody]CRUDModel<NetCorePSAV.Models.ILNewModel.ILGrid> myObject2)
        {
            var ord = myObject2.Value;
            NetCorePSAV.Models.ILNewModel.ILGrid val2 = ListGridIL.Where(or => or.ID == ord.ID).FirstOrDefault();
            val2.Cantidad = ord.Cantidad;
            return Json(myObject2.Value);
        }
        public ActionResult ILD([FromBody]CRUDModel<NetCorePSAV.Models.ILNewModel.ILGrid> value)
        {
            ListGridIL.Remove(ListGridIL.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        [HttpPost]
        public IActionResult NIL(string AddCat,string Item,Models.ItemListModel.ItemListEventModel ILM,string AddOne)
        {
            ViewBag.datasource2 = WFList;
            ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Aquí se estan  guardando toda la lista de workforce
            if (!string.IsNullOrEmpty(AddCat))
            {
                ViewBag.datasourceC = ConSQL.GetCategories();
                ViewBag.datasourceSC = ConSQL.GetSubCategories();
                ViewBag.datasourceI = ConSQL.GetItems();
                if (ListGridIL.Count > 0) {
                    List<NetCorePSAV.Models.ILNewModel.ILGrid> iL = new List<NetCorePSAV.Models.ILNewModel.ILGrid>();
                    iL = ConSQL.GetGrids(Item);
                    ListGridIL.Add(iL[0]);
                }
                else
                {
                    ListGridIL = ConSQL.GetGrids(Item);
                }
                ViewBag.datasourceILI = ListGridIL;
            }
            string IDITL = "";
            string IDITLWF = "";
            if (!string.IsNullOrEmpty(AddOne))
            {
                switch (AddOne)
                {
                    case "1"://guardar
                        List<NetCorePSAV.Models.ILNewModel.ILGrid> iL = ListGridIL;
                        IDITL = ConSQL.InsertTMItemList(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos el valor 102
                        IDITLWF = ConSQL.InsertTMItemListWF(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos 90
                        ILM = new Models.ItemListModel.ItemListEventModel();//estamos accesando a las propiedades del modelo itemlistevent
                        ILM.EventoName = Request.Cookies["EVN"].ToString();//En la propiedad eventoname estamos guardando el valor de fin de mes 1
                        ILM.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());//Estamos guardando el valor de ID 0
                        //ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());//Estamos guardando en BD las variables en corchetes
                        ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        ViewBag.datasourcedrop = ConSQL.GetListCategory("1");//Se esta guardando una cuenta de 11
                        ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Se esta guardando una cuenta de 5
                        ServList = new List<Models.SyncPSAV.ItemListServices>();//estamos guardando los datos obtenidos 
                        WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ViewBag.datasource = ServList;
                        ViewBag.datasource2 = WFList;
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ConSQL.SaveIL(iL, Request.Cookies["IDE"].ToString(),IDITL);
                        ViewBag.datasourceC = ConSQL.GetCategories();
                        ViewBag.datasourceSC = ConSQL.GetSubCategories();
                        ViewBag.datasourceI = ConSQL.GetItems();
                        ListGridIL = new List<NetCorePSAV.Models.ILNewModel.ILGrid>();
                        break;
                    case "2"://vdesc
                        //List<NetCorePSAV.Models.ILNewModel.ILGrid> iL = ListGridIL;
                        IDITL = ConSQL.InsertTMItemList(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos el valor 102
                        IDITLWF = ConSQL.InsertTMItemListWF(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos 90
                        ILM = new Models.ItemListModel.ItemListEventModel();//estamos accesando a las propiedades del modelo itemlistevent
                        ILM.EventoName = Request.Cookies["EVN"].ToString();//En la propiedad eventoname estamos guardando el valor de fin de mes 1
                        ILM.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());//Estamos guardando el valor de ID 0
                        //ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());//Estamos guardando en BD las variables en corchetes
                        ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        ViewBag.datasourcedrop = ConSQL.GetListCategory("1");//Se esta guardando una cuenta de 11
                        ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Se esta guardando una cuenta de 5
                        ServList = new List<Models.SyncPSAV.ItemListServices>();//estamos guardando los datos obtenidos 
                        WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ViewBag.datasource = ServList;
                        ViewBag.datasource2 = WFList;
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ConSQL.SaveIL(ListGridIL, Request.Cookies["IDE"].ToString(),IDITL);
                        ViewBag.datasourceC = ConSQL.GetCategories();
                        ViewBag.datasourceSC = ConSQL.GetSubCategories();
                        ViewBag.datasourceI = ConSQL.GetItems();
                        ListGridIL = new List<NetCorePSAV.Models.ILNewModel.ILGrid>();
                        ServList = new List<Models.SyncPSAV.ItemListServices>();
                        ConSQL.SaveIL(ListGridIL, Request.Cookies["IDE"].ToString(),IDITL);
                        return RedirectToAction("VtaDesc"); break;
                    case "3"://borrador     
                        //List<NetCorePSAV.Models.ILNewModel.ILGrid> iL = ListGridIL;
                        IDITL = ConSQL.InsertTMItemList(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos el valor 102
                        IDITLWF = ConSQL.InsertTMItemListWF(ILM, Request.Cookies["IDE"].ToString());//en esta variable guardaremos 90
                        ILM = new Models.ItemListModel.ItemListEventModel();//estamos accesando a las propiedades del modelo itemlistevent
                        ILM.EventoName = Request.Cookies["EVN"].ToString();//En la propiedad eventoname estamos guardando el valor de fin de mes 1
                        ILM.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());//Estamos guardando el valor de ID 0
                        //ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());//Estamos guardando en BD las variables en corchetes
                        ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        ViewBag.datasourcedrop = ConSQL.GetListCategory("1");//Se esta guardando una cuenta de 11
                        ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Se esta guardando una cuenta de 5
                        ServList = new List<Models.SyncPSAV.ItemListServices>();//estamos guardando los datos obtenidos 
                        WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ViewBag.datasource = ServList;
                        ViewBag.datasource2 = WFList;
                        ILM = new Models.ItemListModel.ItemListEventModel();
                        ConSQL.SaveIL(ListGridIL, Request.Cookies["IDE"].ToString(),IDITL);
                        ViewBag.datasourceC = ConSQL.GetCategories();
                        ViewBag.datasourceSC = ConSQL.GetSubCategories();
                        ViewBag.datasourceI = ConSQL.GetItems();
                        ListGridIL = new List<NetCorePSAV.Models.ILNewModel.ILGrid>();
                        return RedirectToAction("EPT"); 
                        break;
                }
            }
            return View(ILM);
        }
        public IActionResult NIL()
        {
            ViewBag.datasourceC = ConSQL.GetCategories();
            ViewBag.datasourceSC = ConSQL.GetSubCategories();
            ViewBag.datasourceI = ConSQL.GetItems();
            return View();
        }
        [HttpPost]
        public IActionResult NewEPTs(Models.EPTModel model)
        {
            if (ModelState.IsValid)
            {
                return View("ItemList");
            }
            else
            {
                return View();
            }
        }
        public List<Models.ItemListModel.ItemListServices> serv { get; set; }
        #region itemlists
        #region Itemlist

        #region editItemlist
        [HttpPost]
        public IActionResult EditItemList2(Models.SyncPSAV.SalonIL model, string Clave, string Cantidad, string Dias, string Descripcion, string PrecioUnit, string Categoria, string subcategoria)
        {
            ServList.Add(new Models.SyncPSAV.ItemListServices() { Clave = Clave, Cantidad = Cantidad, Dias = Dias, Descripcion = Descripcion, PrecioUnit = PrecioUnit, Categoria = Categoria, IDEvento = Convert.ToInt32(Request.Cookies["IDEVNN"].ToString()), IDITL = Request.Cookies["IDIL"].ToString() });
            string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
            model.IDEvt = Request.Cookies["IDEVNN"].ToString();
            model.IDITL = Request.Cookies["IDIL"].ToString();
            model.IDS = Request.Cookies["IDIS"].ToString();
            ConSQL.SaveOneItilEdit(model, ServList, ServList[0].IDITL);
            ServList = new List<Models.SyncPSAV.ItemListServices>();
            return RedirectToAction("EdititemList");
        }


        [HttpPost]
        public IActionResult EditItemList(string IDIL, string Advance, Models.SyncPSAV.SalonIL model, string EVT)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                if (string.IsNullOrEmpty(IDIL) && Request.Cookies["IDIL"] == null)
                {
                    Models.SyncPSAV.SalonIL ILS = new Models.SyncPSAV.SalonIL();
                    ServList = new List<Models.SyncPSAV.ItemListServices>();
                    ViewBag.datasource = ServList;
                    //                    ViewBag.datasourcedrop = ConSQL.GetListCategory("1").ToList();
                    List<Models.SyncPSAV.ItemCategory> IC = ConSQL.GetListCategory("1");
                    List<Models.SyncPSAV.Caategoria> Catego = new List<Models.SyncPSAV.Caategoria>();
                    for (int i = 0; i < IC.Count; i++)
                    {
                        Catego.Add(new Models.SyncPSAV.Caategoria(IC[i].Categoria, IC[i].Categoria));
                    }
                    ViewBag.datasourcedrop = Catego.ToList();
                    Response.Cookies.Append("IDEVNN", ConSQL.GetEPTToEdit(EVT).IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return View(ILS);

                }
                else
                {
                    if (string.IsNullOrEmpty(IDIL)) { IDIL = Request.Cookies["IDIL"].ToString(); }

                    Models.SyncPSAV.SalonIL ILS = ConSQL.GetOneSalonIL(IDIL);
                    ServList = ConSQL.LILS(IDIL);
                    ViewBag.datasource = ServList;
                    //                    ViewBag.datasourcedrop = ConSQL.GetListCategory("1").ToList();
                    List<Models.SyncPSAV.ItemCategory> IC = ConSQL.GetListCategory("1");
                    List<Models.SyncPSAV.Caategoria> Catego = new List<Models.SyncPSAV.Caategoria>();
                    for (int i = 0; i < IC.Count; i++)
                    {
                        Catego.Add(new Models.SyncPSAV.Caategoria(IC[i].Categoria, IC[i].Categoria));
                    }
                    ViewBag.datasourcedrop = Catego.ToList();
                    Response.Cookies.Append("IDEVNN", ILS.IDEvt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("IDIL", IDIL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return View(ILS);
                }
            }
            else
            {
                if (Advance.Equals("2")) { Response.Cookies.Delete("IDIL"); string folio1 = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString()); Response.Cookies.Append("folio", folio1, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true }); return RedirectToAction("ResumeEPT"); }
                string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
                model.IDEvt = Request.Cookies["IDEVNN"].ToString();
                if (ServList.Count.Equals(0)) { ConSQL.UpdateITL(model, ServList, ""); } else { ConSQL.UpdateITL(model, ServList, ServList[0].IDITL); }

                Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                ServList = new List<Models.SyncPSAV.ItemListServices>();
                return RedirectToAction("ResumeEPT");
            }
        }


        #endregion
        public IActionResult ItemList()
        {
            Models.ItemListModel.ItemListEventModel mod = new Models.ItemListModel.ItemListEventModel();
            mod.EventoName = Request.Cookies["EVN"].ToString();
            mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            if (ServList.Count.Equals(0))
            {
                BindServList();
                ViewBag.datasourcedrop = ConSQL.GetListCategory("1");
                ViewBag.datasource = ServList;
            }
            return View(mod);
        }
        //[HttpPost]
        //public IActionResult ItemList(string Advance)
        //{           
        //        return View("ItemListWorkForce");            
        //}
        #region newitemlist
        [HttpGet]
        public IActionResult IL()//aqui se agregan las listas nuevas  para mandarlas a las vistas y que salgan las listas con los datos de BD como categroia o subcategoria 
        {
            Models.ItemListModel.ItemListEventModel mode = new Models.ItemListModel.ItemListEventModel();
            mode.EventoName = Request.Cookies["EVN"].ToString();
            mode.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());

            BindServList(); BindServListWF();
            ViewBag.datasourcedrop = ConSQL.GetListCategory("1");//aquí se estan guardando toda la lista de Categoria
            ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Aquí se estan  guardando toda la lista de workforce

            ViewBag.datasource = ServList;
            ViewBag.datasource2 = WFList;
            ViewBag.datasource3 = newList;


            return View(mode);
        }

        public IActionResult PN()
        {
            Models.ItemListModel.ItemListEventModel Newitems = new Models.ItemListModel.ItemListEventModel();
            Newitems.EventoName = Request.Cookies["EVN"].ToString();
            BindSubList();
            Newitems.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            ViewBag.datasourcedrop3 = ConSQL.GetListsubCategory("3");//Aquí se estan  guardando toda la lista de subcategoria
            ViewBag.datasourcedrop4 = ConSQL.GetListAcceso("4");

            return View(Newitems);
        }

        public IActionResult PN(Models.ItemListModel.ItemListEventModel mod2, string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))//Esta parte es cuando guardamos salon
            {
                string IDPM = "";//aqui preparamos las variables para recibir un valor
                switch (Advance)
                {
                    case "0": //en el caso 0 va a hacer lo siguiente
                        //IDPM = ConSQL.Insertnewitemlist(mod, Request.Cookies["IDE"].ToString());//en esta variable guardaremos el valor 102
                        mod2 = new Models.ItemListModel.ItemListEventModel();//estamos accesando a las propiedades del modelo itemlistevent
                        mod2.EventoName = Request.Cookies["EVN"].ToString();//En la propiedad eventoname estamos guardando el valor de fin de mes 1
                        mod2.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());//Estamos guardando el valor de ID 0
                        //ConSQL.SaveItemList(newList, IDPM, Request.Cookies["IDE"].ToString());//Estamos guardando en BD las variables en corchetes
                        ViewBag.datasourcedrop = ConSQL.GetListsubCategory("1");//Se esta guardando una cuenta de 11
                        ViewBag.datasourcedrop2 = ConSQL.GetListsubCategory("2");//Se esta guardando una cuenta de 5
                        newList = new List<Models.SyncPSAV.Newitemslist>();//estamos guardando los datos obtenidos  
                        mod2 = new Models.ItemListModel.ItemListEventModel();
                        ViewBag.datasource = newList;
                        mod2 = new Models.ItemListModel.ItemListEventModel();
                        return View(mod2); break;//estamos regresando la vista con los valores dados
                    case "1":
                        newList = new List<Models.SyncPSAV.Newitemslist>();
                        return RedirectToAction("VtaDesc"); break;
                        //case "2":
                        //  IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());

                        //mod = new Models.ItemListModel.ItemListEventModel();
                        //mod.EventoName = Request.Cookies["EVN"].ToString();
                        //mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                        //ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());
                        //ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        //return RedirectToAction("EPT"); break;
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult IL(Models.ItemListModel.ItemListEventModel mod, string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))//Esta parte es cuando guardamos salon
            {
                string IDITL = ""; string IDITLWF = "";//aqui preparamos las variables para recibir un valor
                switch (Advance)
                {
                    case "0": //en el caso 0 va a hacer lo siguiente
                        IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());//en esta variable guardaremos el valor 102
                        IDITLWF = ConSQL.InsertTMItemListWF(mod, Request.Cookies["IDE"].ToString());//en esta variable guardaremos 90
                        mod = new Models.ItemListModel.ItemListEventModel();//estamos accesando a las propiedades del modelo itemlistevent
                        mod.EventoName = Request.Cookies["EVN"].ToString();//En la propiedad eventoname estamos guardando el valor de fin de mes 1
                        mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());//Estamos guardando el valor de ID 0
                        ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());//Estamos guardando en BD las variables en corchetes
                        ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        ViewBag.datasourcedrop = ConSQL.GetListCategory("1");//Se esta guardando una cuenta de 11
                        ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");//Se esta guardando una cuenta de 5
                        ServList = new List<Models.SyncPSAV.ItemListServices>();//estamos guardando los datos obtenidos 
                        WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                        mod = new Models.ItemListModel.ItemListEventModel();
                        ViewBag.datasource = ServList;
                        ViewBag.datasource2 = WFList;
                        mod = new Models.ItemListModel.ItemListEventModel();
                        return View(mod); break;//estamos regresando la vista con los valores dados
                    case "1":
                        ServList = new List<Models.SyncPSAV.ItemListServices>();
                        return RedirectToAction("VtaDesc"); break;
                    case "2":
                        IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());
                        IDITLWF = ConSQL.InsertTMItemListWF(mod, Request.Cookies["IDE"].ToString());
                        mod = new Models.ItemListModel.ItemListEventModel();
                        mod.EventoName = Request.Cookies["EVN"].ToString();
                        mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                        ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());
                        ConSQL.SaveItemListWFDetail(WFList, IDITLWF, Request.Cookies["IDE"].ToString());
                        return RedirectToAction("EPT"); break;
                }
            }
            else
            {
                Models.ItemListModel.ItemListEventModel mode = new Models.ItemListModel.ItemListEventModel();
                mode.EventoName = Request.Cookies["EVN"].ToString();
                mode.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                if (ServList.Count.Equals(0))
                {
                    BindServList(); BindServListWF();
                    ViewBag.datasourcedrop = ConSQL.GetListCategory("1");
                    ViewBag.datasourcedrop2 = ConSQL.GetListCategory("2");
                    ViewBag.datasource = ServList;
                    ViewBag.datasource2 = WFList;
                }
                return View(mod);
            }
            return View();
        }
        #endregion
        [HttpPost]
        public IActionResult ItemList(Models.ItemListModel.ItemListEventModel mod, string Advance)
        {
            if (Advance.Equals("2"))
            {
                string IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());
                return RedirectToAction("EPT");
            }
            if (Advance.Equals("0"))
            {
                string IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());
                return View(mod);
            }
            else
            {
                ServList = new List<Models.SyncPSAV.ItemListServices>();
                return RedirectToAction("ItemListWorkForce");
            }
        }

        //Listas estaticas para guardar 
        //Categoria
        public static List<Models.SyncPSAV.ItemListServices> ServList = new List<Models.SyncPSAV.ItemListServices>();
        public static List<Models.SyncPSAV.ItemListServicesEdit> ServListe = new List<Models.SyncPSAV.ItemListServicesEdit>();
        //Subcategoria
        public static List<Models.SyncPSAV.Newitemslist> newList = new List<Models.SyncPSAV.Newitemslist>();
        public static List<Models.SyncPSAV.NewitemlistEdit> newListeed = new List<Models.SyncPSAV.NewitemlistEdit>();
        //Accesorios
        //public static List<Models.SyncPSAV.Accerosioslist> accesList = new List<Models.SyncPSAV.Accerosioslist>();
        //public static List<Models.SyncPSAV.Accerosioslistedit> accesListeedit = new List<Models.SyncPSAV.Accerosioslistedit>();
        //item
        //public static List<Models.SyncPSAV.itemlist> itemList = new List<Models.SyncPSAV.itemlist>();
        //public static List<Models.SyncPSAV.itemlistedit> ietmListeedit = new List<Models.SyncPSAV.itemlistedit>();
        //notas
        //public static List<Models.SyncPSAV.Notaslist> notasList = new List<Models.SyncPSAV.Notaslist>();
        //public static List<Models.SyncPSAV.Notaslistedit> notasedit = new List<Models.SyncPSAV.Notaslistedit>();

        //categoria
        public void BindServList() { ServList = new List<Models.SyncPSAV.ItemListServices>(); }
        //SubCategoria
        public void BindSubList() { newList = new List<Models.SyncPSAV.Newitemslist>(); }
        //Accesorios
        //public void BindAccesList() { accesList = new List<Models.SyncPSAV.Accerosioslist>(); }
        //items
        //public void BindItemist() { itemList = new List<Models.SyncPSAV.itemlist>(); }
        //notas
        //public void BindNotList() { notasList = new List<Models.SyncPSAV.Notaslist>(); }
        //Botones para agregar, eliminar y editar tabla

        public ActionResult newitemUpdate([FromBody]CRUDModel<Models.SyncPSAV.Newitemslist> myObject2)
        {
            var ord = myObject2.Value;
            Models.SyncPSAV.Newitemslist val2 = newList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val2.ID = ord.ID; val2.Subcategoria = ord.Subcategoria; val2.Accesorios = val2.Accesorios;
            return Json(myObject2.Value);
        }

        public ActionResult newitemInsert([FromBody]CRUDModel<Models.SyncPSAV.Newitemslist> value2)
        {
            Models.SyncPSAV.Newitemslist val2 = value2.Value;
            val2.IDEvento = Convert.ToInt32(Request.Cookies["IDEVNN"].ToString());
            newList.Add(val2);
            return Json(ServList);
        }
        public ActionResult ItemListNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.ItemListServices val = ServList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Cantidad = ord.Cantidad; val.Categoria = ord.Categoria; val.Clave = ord.Clave; val.Descripcion = ord.Descripcion;
            val.Dias = ord.Dias; val.PrecioUnit = ord.PrecioUnit;
            return Json(myObject.Value);
        }
        public ActionResult ItemListNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> value)
        {
            Models.SyncPSAV.ItemListServices val = value.Value;
            val.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            ServList.Add(val);
            return Json(ServList);
        }
        public ActionResult EditItemListNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> value)
        {
            Models.SyncPSAV.ItemListServices val = value.Value;
            val.IDEvento = Convert.ToInt32(Request.Cookies["IDEVNN"].ToString());
            ServList.Add(val);
            return Json(ServList);
        }
        public ActionResult ItemListNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> value)
        {
            ServList.Remove(ServList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region ItemListWorkForce
        #region EditItemListWF
        [HttpGet]
        public IActionResult EditItemListWF()
        {
            Models.SyncPSAV.SalonILWF ILS = ConSQL.GetOneSalonILWF(Request.Cookies["IDIL"].ToString());
            WFList = ConSQL.LILSWF(Request.Cookies["IDIL"].ToString());
            Response.Cookies.Append("IDEVNN", ILS.IDEvt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            ViewBag.datasource = WFList;
            ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
            return View(ILS);
        }
        [HttpPost]
        public IActionResult EditItemListWF2(Models.SyncPSAV.SalonILWF model, string Seccion, string Clave, string Cantidad, string Dias, string Descripcion, string PrecioUnit, string Categoria)
        {
            string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
            model.IDEvt = Request.Cookies["IDEVNN"].ToString();
            WFList.Add(new Models.SyncPSAV.ItemListWorkForce() { Cantidad = Cantidad, Categoria = Categoria, Clave = Clave, Descripcion = Descripcion, Dias = Dias, PrecioUnit = PrecioUnit, Seccion = Seccion, IDITL = Request.Cookies["IDIL"].ToString(), IDEvento = Convert.ToInt32(Request.Cookies["IDEVNN"].ToString()) });
            ConSQL.SaveOneItilWF(model, WFList, WFList[0].IDITL);
            Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
            return RedirectToAction("EditItemListWF");
        }
        [HttpPost]
        public IActionResult EditItemListWF(string IDIL, string Advance, Models.SyncPSAV.SalonILWF model, string EVT)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                if (!string.IsNullOrEmpty(IDIL))
                {
                    Models.SyncPSAV.SalonILWF ILS = ConSQL.GetOneSalonILWF(IDIL);
                    WFList = ConSQL.LILSWF(IDIL);
                    Response.Cookies.Append("IDEVNN", ILS.IDEvt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("IDIL", IDIL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    ViewBag.datasource = WFList;
                    ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
                    return View(ILS);
                }
                else
                {
                    Models.SyncPSAV.SalonILWF ILS = new Models.SyncPSAV.SalonILWF();
                    WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                    Response.Cookies.Append("IDEVNN", ConSQL.GetEPTToEdit(EVT).IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    //Response.Cookies.Append("IDIL", IDIL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    ViewBag.datasource = WFList;
                    ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
                    return View(ILS);
                }
            }
            else
            {
                if (Advance.Equals("2")) { Response.Cookies.Delete("IDIL"); string folio1 = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString()); Response.Cookies.Append("folio", folio1, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true }); return RedirectToAction("ResumeEPT"); }
                string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
                model.IDEvt = Request.Cookies["IDEVNN"].ToString();
                if (WFList.Count.Equals(0)) { ConSQL.UpdateITLWF(model, WFList, ""); } else { ConSQL.UpdateITLWF(model, WFList, WFList[0].IDITL); }
                Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                return RedirectToAction("ResumeEPT");
            }
        }
        #endregion
        [HttpPost]
        public IActionResult ItemListWorkForce(Models.ItemListModel.ItemListEventModel mod, string Advance)
        {
            if (Advance.Equals("2"))
            {
                string IDITL = ConSQL.InsertTMItemListWF(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListWFDetail(WFList, IDITL, Request.Cookies["IDE"].ToString());
                ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
                return RedirectToAction("EPT");
            }
            if (Advance.Equals("0"))
            {
                string IDITL = ConSQL.InsertTMItemListWF(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListWFDetail(WFList, IDITL, Request.Cookies["IDE"].ToString());
                ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
                return View(mod);
            }
            else
            {
                WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
                return RedirectToAction("VtaDesc");
            }
        }
        public IActionResult ItemListWorkForce()
        {
            Models.ItemListModel.ItemListEventModel mod = new Models.ItemListModel.ItemListEventModel();
            mod.EventoName = Request.Cookies["EVN"].ToString();
            mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            if (ServList.Count.Equals(0))
            {
                BindServListWF();
                ViewBag.datasource = WFList;
                ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
            }
            ViewBag.datasourcedrop = ConSQL.GetListCategory("2").ToList();
            return View(mod);
        }
        public static List<Models.SyncPSAV.ItemListWorkForce> WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
        public void BindServListWF() { WFList = new List<Models.SyncPSAV.ItemListWorkForce>(); }
        public ActionResult ItemListWFNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.ItemListWorkForce val = WFList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Cantidad = ord.Cantidad; val.Categoria = ord.Categoria; val.Clave = ord.Clave; val.Descripcion = ord.Descripcion;
            val.Dias = ord.Dias; val.PrecioUnit = ord.PrecioUnit;
            return Json(myObject.Value);
        }
        public ActionResult ItemListWFNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> value)
        {
            Models.SyncPSAV.ItemListWorkForce val = value.Value;
            WFList.Insert(WFList.Count, val);
            return Json(WFList);
        }
        public ActionResult ItemListWFNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> value)
        {
            WFList.Remove(WFList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region VentaDes
        #region edit
        [HttpPost]
        public IActionResult EditVtaDesc(string Advance)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                List<Models.SyncPSAV.VentaDes> list = ConSQL.GetCategoryEvent(Request.Cookies["IDEVT"].ToString());
                VDescList = ConSQL.GetVtaDesc(Request.Cookies["IDEVT"].ToString());
                if (!VDescList.Equals(list.Count))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        bool itemFound = false;
                        string data = "";
                        for (int x = 0; x < VDescList.Count; x++)
                        {
                            itemFound = false;
                            data = list[i].Category;
                            if (VDescList[x].Category.Equals(list[i].Category)) { itemFound = true; break; }
                        }
                        if (!itemFound) { VDescList.Add(new Models.SyncPSAV.VentaDes() { Category = data, ID = "0" }); }
                    }
                }
                ViewBag.datasource = VDescList;
            }
            else
            {
                ConSQL.UpdateVtaDesc(VDescList, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditVtaFee(string Advance)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                List<Models.SyncPSAV.VentaFee> list = ConSQL.GetCategoryEvtFee(Request.Cookies["IDEVT"].ToString());
                VFeeList = ConSQL.GetVFee(Request.Cookies["IDEVT"].ToString());
                if (!VFeeList.Equals(list.Count))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        bool itemFound = false;
                        string data = "";
                        for (int x = 0; x < VFeeList.Count; x++)
                        {
                            itemFound = false;
                            data = list[i].Category;
                            if (VFeeList[x].Category.Equals(list[i].Category)) { itemFound = true; break; }
                        }
                        if (!itemFound) { VFeeList.Add(new Models.SyncPSAV.VentaFee() { Category = data, ID = "0" }); }
                    }
                }
                ViewBag.datasource = VFeeList;
            }
            else
            {
                ConSQL.UpdateVFee(VFeeList, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditSubrenta(string Advance)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                SubRentaList = ConSQL.GetSubRenta(Request.Cookies["IDEVT"].ToString());
                ViewBag.datasource = SubRentaList;
                ViewBag.datasourcedrop = ConSQL.GetListCategory("3");
            }
            else
            {
                ConSQL.UpdateSRenta(SubRentaList, Request.Cookies["IDEVT"].ToString());
                ViewBag.datasourcedrop = ConSQL.GetListCategory("3");
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditOL(string Advance)
        {
            ViewBag.datasourceCom = ConSQL.GetComVtaCat();
            if (string.IsNullOrEmpty(Advance))
            {
                ViewBag.datasource1 = FOList;
                ViewBag.datasource2 = ViaticosL;
                ViewBag.datasource3 = ComVenL;
                ViewBag.datasource4 = GasFinL;
                ViewBag.datasource5 = ConsuL;
                ViewBag.datasource6 = CIntL;
            }
            else
            {
                List<Models.SyncPSAV.VentasFeeTot> tttt = ComVenL;
                for (int i = 0; i < tttt.Count; i++)
                {
                    tttt[i].Comision = ConSQL.GetValueComVtaCat(tttt[i].Puesto);
                }
                ConSQL.UpdateOL(FOList, ViaticosL, tttt, GasFinL, ConsuL, CIntL, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        #endregion
        public static List<Models.SyncPSAV.VentaDes> VDescList = new List<Models.SyncPSAV.VentaDes>();
        public ActionResult VtaDctoNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.VentaDes val = VDescList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.Category = ord.Category; val.VentaEqui = ord.VentaEqui; val.VentaEquEx = ord.VentaEquEx; val.TotalVenta = ord.TotalVenta;
            val.DesPorEq = ord.DesPorEq; val.TotalDescEPS = ord.TotalDescEPS; val.DescExt = ord.DescExt; val.TotalExt = ord.TotalExt;
            val.TotalDesc = ord.TotalDesc; val.PorcTotalDesc = ord.PorcTotalDesc; val.AplicaAut = ord.AplicaAut;
            return Json(myObject.Value);
        }
        public ActionResult VtaDctoNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> value)
        {
            Models.SyncPSAV.VentaDes val = value.Value;
            VDescList.Insert(VDescList.Count, val);
            return Json(VDescList);
        }
        public ActionResult VtaDctoNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> value)
        {
            VDescList.Remove(VDescList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult VtaDesc()
        {
            VDescList = ConSQL.GetCategoryEvent(Request.Cookies["IDE"].ToString());
            ViewBag.datasource = VDescList;
            return View();
        }
        [HttpPost]
        public IActionResult VtaDesc(string Advance)
        {
            ConSQL.SaveVDesc(VDescList, Request.Cookies["IDE"].ToString());
            return RedirectToAction("VentaComision");
        }
        #endregion
        #region VentaFee
        public static List<Models.SyncPSAV.VentaFee> VFeeList = new List<Models.SyncPSAV.VentaFee>();
        public ActionResult VtaFeeNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.VentaFee val = VFeeList.Where(or => or.Category == ord.Category).FirstOrDefault();
            val.BaseFee = ord.BaseFee; val.ImporteFee = ord.ImporteFee; val.PorcFee = ord.PorcFee; val.SubFee = ord.SubFee;
            return Json(myObject.Value);
        }
        public ActionResult VtaFeeNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> value)
        {
            Models.SyncPSAV.VentaFee val = value.Value;
            VFeeList.Insert(VFeeList.Count, val);
            ViewBag.datasource = VFeeList;

            return Json(VFeeList);
        }
        public ActionResult VtaFeeNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> value)
        {
            VFeeList.Remove(VFeeList.Where(or => or.Category == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult VentaComision()
        {
            VFeeList = ConSQL.GetCategoryEvtFee(Request.Cookies["IDE"].ToString());
            ViewBag.datasource = VFeeList;
            return View();
        }
        [HttpPost]
        public IActionResult VentaComision(string Advance)
        {
            ConSQL.SaveVFee(VFeeList, Request.Cookies["IDE"].ToString());
            return RedirectToAction("Subrenta");
        }
        #endregion
        #region Subrenta
        public static List<Models.SyncPSAV.SubRenta> SubRentaList = new List<Models.SyncPSAV.SubRenta>();
        public ActionResult SubRentaNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.SubRenta val = SubRentaList.Where(or => or.Category == ord.Category).FirstOrDefault();
            val.Descripcion = ord.Descripcion; val.Gastosub = ord.Gastosub; val.Invoice = ord.Invoice; val.Supplier = ord.Supplier; val.Ventasub = ord.Ventasub;
            return Json(myObject.Value);
        }
        public ActionResult SubRentaNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> value)
        {
            Models.SyncPSAV.SubRenta val = value.Value;
            SubRentaList.Insert(SubRentaList.Count, val);
            return Json(VFeeList);
        }
        public ActionResult SubRentaNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> value)
        {
            SubRentaList.Remove(SubRentaList.Where(or => or.Category == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult Subrenta()
        {
            SubRentaList = new List<Models.SyncPSAV.SubRenta>();
            ViewBag.datasourcedrop = ConSQL.GetListCategory("3");
            ViewBag.datasource = SubRentaList;
            return View();
        }
        [HttpPost]
        public IActionResult Subrenta(string Advance)
        {
            ConSQL.SaveSubRent(SubRentaList, Request.Cookies["IDE"].ToString());
            ViewBag.datasourcedrop = ConSQL.GetListCategory("3");
            return RedirectToAction("OL");
        }
        #endregion
        #region OL
        public IActionResult OL()
        {
            BindResultsOL();
            ViewBag.datasource1 = FOList;
            ViewBag.datasource2 = ViaticosL;
            ViewBag.datasource3 = ComVenL;
            ViewBag.datasource4 = GasFinL;
            ViewBag.datasource5 = ConsuL;
            ViewBag.datasource6 = CIntL;
            return View();
        }
        [HttpPost]
        public IActionResult OL(string Advance)
        {
            string folio = ConSQL.GetFolioByITL(Request.Cookies["IDE"].ToString());
            ConSQL.SaveOL(FOList, ViaticosL, ComVenL, GasFinL, ConsuL, CIntL, Request.Cookies["IDE"].ToString());
            Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return RedirectToAction("ResumenEPT");
        }
        public static List<Models.SyncPSAV.FreelanceOL> FOList = new List<Models.SyncPSAV.FreelanceOL>();
        public static List<Models.SyncPSAV.Viaticos> ViaticosL = new List<Models.SyncPSAV.Viaticos>();
        public static List<Models.SyncPSAV.VentasFeeTot> ComVenL = new List<Models.SyncPSAV.VentasFeeTot>();
        public static List<Models.SyncPSAV.GastosFinancieros> GasFinL = new List<Models.SyncPSAV.GastosFinancieros>();
        public static List<Models.SyncPSAV.Consumibles> ConsuL = new List<Models.SyncPSAV.Consumibles>();
        public static List<Models.SyncPSAV.CargosInternos> CIntL = new List<Models.SyncPSAV.CargosInternos>();
        public void BindResultsOL()
        {
            ViewBag.datasourceCom = ConSQL.GetComVtaCat();
            FOList = new List<Models.SyncPSAV.FreelanceOL>(); ViaticosL = new List<Models.SyncPSAV.Viaticos>(); ComVenL = new List<Models.SyncPSAV.VentasFeeTot>();
            GasFinL = new List<Models.SyncPSAV.GastosFinancieros>(); ConsuL = new List<Models.SyncPSAV.Consumibles>(); CIntL = new List<Models.SyncPSAV.CargosInternos>();
        }
        #region OLList
        public ActionResult FOLNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.FreelanceOL> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.FreelanceOL val = FOList.Where(or => or.Nombres == ord.Nombres).FirstOrDefault();
            val.Condiciones = ord.Condiciones; val.CostoCarga = ord.CostoCarga; val.CostoTotal = ord.CostoTotal; val.Dias = ord.Dias; val.Nombres = ord.Nombres; val.Puesto = ord.Puesto; val.Sueldo = ord.Sueldo;
            return Json(myObject.Value);
        }
        public ActionResult FOLNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.FreelanceOL> value)
        {
            Models.SyncPSAV.FreelanceOL val = value.Value;
            FOList.Insert(FOList.Count, val);
            return Json(FOList);
        }
        public ActionResult FOLNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.FreelanceOL> value)
        {
            FOList.Remove(FOList.Where(or => or.Nombres == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region viaticosList
        public ActionResult ViaticosNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.Viaticos> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.Viaticos val = ViaticosL.Where(or => or.Nombres == ord.Nombres).FirstOrDefault();
            val.Nombres = ord.Nombres; val.Observaciones = ord.Observaciones; val.Puesto = ord.Puesto; val.TotalSol = ord.TotalSol;
            return Json(myObject.Value);
        }
        public ActionResult ViaticosNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.Viaticos> value)
        {
            Models.SyncPSAV.Viaticos val = value.Value;
            ViaticosL.Insert(ViaticosL.Count, val);
            return Json(ViaticosL);
        }
        public ActionResult ViaticosNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.Viaticos> value)
        {
            ViaticosL.Remove(ViaticosL.Where(or => or.Nombres == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region ComisionVtaL
        public ActionResult ComVtaNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.VentasFeeTot> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.VentasFeeTot val = ComVenL.Where(or => or.Nombres == ord.Nombres).FirstOrDefault();
            val.Comision = ord.Comision; val.Comisiontot = ord.Comisiontot; val.Nombres = ord.Nombres; val.Puesto = ord.Puesto; val.VentaNeta = ord.VentaNeta;
            return Json(myObject.Value);
        }
        public ActionResult ComVtaNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.VentasFeeTot> value)
        {
            Models.SyncPSAV.VentasFeeTot val = value.Value;
            ComVenL.Insert(ComVenL.Count, val);
            return Json(ComVenL);
        }
        public ActionResult ComVtaNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.VentasFeeTot> value)
        {
            ComVenL.Remove(ComVenL.Where(or => or.Nombres == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region GFinanlist
        public ActionResult GFinanNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.GastosFinancieros> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.GastosFinancieros val = GasFinL.Where(or => or.Importe == ord.Importe).FirstOrDefault();
            val.Comision = ord.Comision; val.Importe = ord.Importe; val.ImporteCom = ord.ImporteCom; val.PorcCom = ord.PorcCom;
            return Json(myObject.Value);
        }
        public ActionResult GFinanNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.GastosFinancieros> value)
        {
            Models.SyncPSAV.GastosFinancieros val = value.Value;
            GasFinL.Insert(GasFinL.Count, val);
            return Json(ComVenL);
        }
        public ActionResult GFinanNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.GastosFinancieros> value)
        {
            GasFinL.Remove(GasFinL.Where(or => or.Importe == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region fletesl
        public ActionResult FletesNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.Consumibles> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.Consumibles val = ConsuL.Where(or => or.Cotizacion == ord.Cotizacion).FirstOrDefault();
            val.Costo = ord.Costo; val.Cotizacion = ord.Cotizacion; val.Description = ord.Description; val.Supplier = ord.Supplier;
            return Json(myObject.Value);
        }
        public ActionResult FletesNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.Consumibles> value)
        {
            Models.SyncPSAV.Consumibles val = value.Value;
            ConsuL.Insert(ConsuL.Count, val);
            return Json(ComVenL);
        }
        public ActionResult FletesNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.Consumibles> value)
        {
            ConsuL.Remove(ConsuL.Where(or => or.Cotizacion == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region cargosinternos
        public ActionResult CINormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.CargosInternos> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.CargosInternos val = CIntL.Where(or => or.MontoCargo == ord.MontoCargo).FirstOrDefault();
            val.Categoria = ord.Categoria; val.Equipo = ord.Equipo; val.MontoCargo = ord.MontoCargo; val.PorcCargo = ord.PorcCargo; val.PrecioLista = ord.PrecioLista; val.TipoOper = ord.TipoOper;
            return Json(myObject.Value);
        }
        public ActionResult CINormalInsert([FromBody]CRUDModel<Models.SyncPSAV.CargosInternos> value)
        {
            Models.SyncPSAV.CargosInternos val = value.Value;
            CIntL.Insert(CIntL.Count, val);
            return Json(ComVenL);
        }
        public ActionResult CINormalDelete([FromBody]CRUDModel<Models.SyncPSAV.CargosInternos> value)
        {
            CIntL.Remove(CIntL.Where(or => or.MontoCargo == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #endregion
        #region editEPT
        #region resumeEPT

        public IActionResult ResumenEPT(string folio)
        {
            if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString())) { folio = Request.Cookies["folio"].ToString(); }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return View(eptM);
        }

        public IActionResult ResumeEPT()
        {
            string folio = "";
            if (Request.Cookies["folio"] != null)
            {
                if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString()))
                {
                    if (folio != Request.Cookies["folio"].ToString())
                    {
                        folio = Request.Cookies["folio"].ToString();
                    }
                }
            }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            Response.Cookies.Delete("IDEVNN"); Response.Cookies.Delete("IDIL");
            return View(eptM);

        }
        [HttpPost]
        public IActionResult ResumeEPT(string folio)
        {
            //if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString())){ folio = Request.Cookies["folio"].ToString(); }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            Response.Cookies.Delete("IDEVNN"); Response.Cookies.Delete("IDIL");
            return View(eptM);
        }
        [HttpGet]
        public IActionResult EditEPT(Models.EPTModel eptt, string folio)
        {
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
            ViewBag.CoinsList = CoinsList;
            return View(eptM);
        }
        [HttpPost]
        public IActionResult EditEPT(Models.EPTModel eptM, string NewValue, string folio)
        {
            if (!string.IsNullOrEmpty(NewValue))
            {
                List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClientOne(eptM.IDClient);
                for (int i = 0; i < Clients.Count; i++)
                {
                    eptM.DomicilioComercial = Clients[i].Domicilio;
                    eptM.DomicilioFiscal = Clients[i].Fiscal;
                    eptM.IDClient = Clients[i].IDClient;
                    eptM.RazonSocial = Clients[i].Razon;
                    eptM.Phone = Clients[i].Tel;
                    eptM.Mobile = Clients[i].Cel;
                    eptM.Email = Clients[i].Email;
                    eptM.RFC = Clients[i].RFC;
                    eptM.Ext = Clients[i].Ext;
                    eptM.ContactClientName = Clients[i].NombreContacto;
                    eptM.IDEmpresa = Clients[i].IDEmpresa;
                }
                ConSQL.EditInsertEPT(eptM);
                return RedirectToAction("EPT");
            }
            else
            {
                return View();
            }
        }
        #endregion
        #endregion
        #endregion
        //[HttpPost]
        //public IActionResult ItemList(Models.ItemListModel.ItemListEventModel model,List<Models.ItemListModel.ItemListServices> Servi,string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if(TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";            
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (!string.IsNullOrEmpty(Siguiente))
        //    {
        //        if (Siguiente.Equals("Si"))
        //        {
        //            Response.Cookies.Append("IDE", Request.Cookies["IDE"].ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("EVN", Request.Cookies["EVN"].ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("SAL", model.Salon, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("ASI", model.Asistentes, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("MON", model.Montaje, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("HRO", model.Horario, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            return View("ItemListWorkForce");
        //        }
        //        else
        //        {
        //            return View(Ilem);
        //        }
        //    }
        //    else
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            string IDITL=ConSQL.InsertTMItemList(model, Request.Cookies["IDE"].ToString());
        //            Response.Cookies.Append("IDITL", IDITL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            ConSQL.SaveItemListDetail(model, IDITL);
        //        }
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemList2(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (Siguiente.Equals("Si"))
        //    {
        //        return View("ItemListWorkForce");
        //    }
        //    else
        //    {
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemListWorkForce(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = Request.Cookies["SLN"].ToString();
        //    Ilem.Asistentes = Request.Cookies["ASI"].ToString();
        //    Ilem.Montaje = Request.Cookies["MON"].ToString();
        //    Ilem.Horario = Request.Cookies["HRO"].ToString();
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(2);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (!string.IsNullOrEmpty(Siguiente))
        //    {
        //        if (Siguiente.Equals("Si"))
        //        {
        //            return View("ItemListWorkForce");
        //        }
        //        else
        //        {
        //            return View(Ilem);
        //        }
        //    }
        //    else
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            ConSQL.SaveItemListWFDetail(model, Request.Cookies["IDITL"].ToString());
        //        }
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemListWorkForce2(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(2);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (Siguiente.Equals("Si"))
        //    {
        //        return View("ItemListWorkForce");
        //    }
        //    else
        //    {
        //        return View(Ilem);
        //    }

        //}
        [HttpPost]
        public async Task<IActionResult> ShowEPT(string folio)
        {
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            return View(eptM);
        }
        #region CaptureRatio
        [HttpPost]
        public IActionResult CRatio(string ac1)
        {
            List<Models.CaptureRatio.CRVResumenModel> CRList = ConSQL.GetCRatio(string.Empty);
            ViewBag.CRList = CRList;
            return View();
        }
        public IActionResult CRatio()
        {
            List<Models.CaptureRatio.CRVResumenModel> CRList = ConSQL.GetCRatio(string.Empty);
            ViewBag.CRList = CRList;
            return View();
        }
        public IActionResult NewCratio()
        {
            return View();
        }
        public static List<Models.SyncPSAV.CratioDets> CDets = new List<Models.SyncPSAV.CratioDets>();
        public IActionResult NewCRatio2(Models.CaptureRatio.CRVModel model)
        {

            ViewBag.datasource = CDets;
            return View();
        }
        public ActionResult CratioNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.CratioDets val = CDets.Where(or => or.IDCratioDets == ord.IDCratioDets).FirstOrDefault();

            val.IDCratio = Request.Cookies["IDCR"].ToString();
            //val.ID = ord.ID; val.Name = ord.Name; val.Change = ord.Change; val.Active = ord.Active;
            ConSQL.SaveCRDet(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult CratioNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> value)
        {
            Models.SyncPSAV.CratioDets val = value.Value;
            val.IDCratio = Request.Cookies["IDCR"].ToString();
            val.IDCratioDets = ConSQL.SaveCRDet(value.Value, 0);
            CDets.Insert(CDets.Count, val);
            //ConSQL.SaveCRDet(val, 0);
            return Json(CDets);
        }
        public ActionResult CratioNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> value)
        {
            CDets.Remove(CDets.Where(or => or.IDCratioDets == value.Key.ToString()).FirstOrDefault());
            ConSQL.SaveCRDet(value.Value, 2);
            return Json(value);
        }
        [HttpPost]
        public async Task<IActionResult> NewCratio(Models.CaptureRatio.CRVModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies["IDCR"] != null) { Response.Cookies.Delete("IDCR"); }
                string IDCRatioL = ConSQL.GetCRatio(model);
                model.IDCRAtio = IDCRatioL;
                Response.Cookies.Append("IDCR", IDCRatioL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                return RedirectToAction("NewCRatio2", model);

            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewCR()
        {
            return View("CRatio");
        }
        public static List<Models.SyncPSAV.CratioDets> CRL = new List<Models.SyncPSAV.CratioDets>();
        [HttpPost]
        public async Task<IActionResult> EditCRatio(string IDC)
        {
            Models.CaptureRatio.CRVResumenModel CRVM = new Models.CaptureRatio.CRVResumenModel();
            List<Models.CaptureRatio.CRVResumenModel> CVR = ConSQL.GetCratioConsult(IDC);
            CDets = ConSQL.GetCD(IDC);
            if (Request.Cookies["IDCOne"] != null) { Response.Cookies.Delete("IDCOne"); }
            Response.Cookies.Append("IDCOne", IDC, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            Response.Cookies.Append("IDCR", IDC, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            ViewBag.CRLIt = CDets;
            for (int i = 0; i < CVR.Count; i++)
            {
                CRVM.HotelName = CVR[i].HotelName;
                CRVM.DET = CVR[i].DET;
                CRVM.CityLoc = CVR[i].CityLoc;
                CRVM.Contact = CVR[i].Contact;
                CRVM.FillFormName = CVR[i].FillFormName;
                CRVM.LocationHotel = CVR[i].LocationHotel;
            }
            ViewBag.datasource = CDets;
            return View(CRVM);
        }
        #endregion
        #region EPTExcel

        //Creación de Excel
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;
        public PSAVController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult ExportEPT(string folio)
        {
            try
            {
                var fileDownloadName = "EPTTem.xlsx";
                var reportsFolder = "reports";
                var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
                byte[] ReportArray;
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {

                    //start opening workbook Cotizacion
                    ExcelWorksheet wkCotizacion = package.Workbook.Worksheets[2];
                    //fill cotizacion info from DB
                    Models.EPTModel ModEptFill = ConSQL.GetEPTToEdit(folio);
                    //fill Client Info
                    wkCotizacion.Cells["C10"].Value = ModEptFill.RazonSocial;//Razon
                    wkCotizacion.Cells["C11"].Value = ModEptFill.DomicilioComercial;//Razon
                    wkCotizacion.Cells["C12"].Value = ModEptFill.DomicilioFiscal;//Razon
                    wkCotizacion.Cells["C13"].Value = ModEptFill.RFC;//Razon
                                                                     //fill contact client
                    wkCotizacion.Cells["C15"].Value = ModEptFill.ContactClientName;//Razon
                    wkCotizacion.Cells["C16"].Value = ModEptFill.Job;//Razon
                    wkCotizacion.Cells["C17"].Value = ModEptFill.Phone;//Razon
                    wkCotizacion.Cells["C18"].Value = ModEptFill.Ext;//Razon
                    wkCotizacion.Cells["C19"].Value = ModEptFill.Mobile;//Razon
                    wkCotizacion.Cells["C20"].Value = ModEptFill.Fax;//Razon
                    wkCotizacion.Cells["C21"].Value = ModEptFill.Email;//Razon
                                                                       //fill sales Represent
                    wkCotizacion.Cells["C24"].Value = ModEptFill.SMName;//Razon
                    wkCotizacion.Cells["C25"].Value = ModEptFill.SMJob;//Razon
                    wkCotizacion.Cells["C26"].Value = ModEptFill.SMEmail;//Razon
                    wkCotizacion.Cells["C27"].Value = ModEptFill.SMPhone;//Razon
                                                                         //fill event info
                    wkCotizacion.Cells["G10"].Value = ModEptFill.EventName;//Razon
                    wkCotizacion.Cells["G11"].Value = ModEptFill.MountDate;//Razon
                    wkCotizacion.Cells["G12"].Value = ModEptFill.MountHour;//Razon
                    wkCotizacion.Cells["G13"].Value = ModEptFill.PlaceContact;//Razon
                    wkCotizacion.Cells["G14"].Value = ModEptFill.PlaceMobile;//Razon
                    wkCotizacion.Cells["G15"].Value = ModEptFill.StartDate;//Razon
                    wkCotizacion.Cells["G16"].Value = ModEptFill.StartHour;//Razon
                    wkCotizacion.Cells["G17"].Value = ModEptFill.FinishDate;//Razon
                    wkCotizacion.Cells["G18"].Value = ModEptFill.FinishHour;//Razon
                    wkCotizacion.Cells["G19"].Value = ModEptFill.Place;//Razonr
                    wkCotizacion.Cells["G20"].Value = ModEptFill.Address;//Razon
                    wkCotizacion.Cells["G22"].Value = "MXN Mexican Peso";//Razon
                                                                         //fill psav manager
                    wkCotizacion.Cells["G24"].Value = ModEptFill.PMName;//Razon
                    wkCotizacion.Cells["G25"].Value = ModEptFill.PMMobile;//Razon
                    wkCotizacion.Cells["G27"].Value = ModEptFill.PMEmail;//Razon
                    wkCotizacion.Cells["G28"].Value = ModEptFill.PMLocation;//Razon
                                                                            ///////////Get Item List Sheet
                    ExcelWorksheet wkItemList = package.Workbook.Worksheets[3];
                    ///itemlist sheet
                    int lastPos = 0;
                    //get all areas
                    List<Models.SyncPSAV.SalonIL> SILList = ConSQL.GetSalons(ModEptFill.IDEvent);
                    lastPos = 16;
                    for (int x = 0; x < SILList.Count; x++)
                    {
                        //fill gray blanks
                        if (x > 0)
                        {
                            if (lastPos >= 41) { wkItemList.InsertRow(lastPos, 5); }
                            wkItemList.Cells["B" + (lastPos).ToString()].Value = "EVENTO:";
                            wkItemList.Cells["B" + (lastPos + 1).ToString()].Value = "Salón/Area:";
                            wkItemList.Cells["B" + (lastPos + 2).ToString()].Value = "# de Asistentes:";
                            wkItemList.Cells["B" + (lastPos + 3).ToString()].Value = "Montaje:";
                            wkItemList.Cells["B" + (lastPos + 4).ToString()].Value = "Horario de Evento:";
                            wkItemList.Cells[lastPos, 1, lastPos, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            wkItemList.Cells[lastPos, 1, lastPos, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                            wkItemList.Cells[(lastPos + 1), 1, (lastPos + 1), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            wkItemList.Cells[(lastPos + 1), 1, (lastPos + 1), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                            wkItemList.Cells[(lastPos + 2), 1, (lastPos + 2), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            wkItemList.Cells[(lastPos + 2), 1, (lastPos + 2), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                            wkItemList.Cells[(lastPos + 3), 1, (lastPos + 3), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            wkItemList.Cells[(lastPos + 3), 1, (lastPos + 3), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                            wkItemList.Cells[(lastPos + 4), 1, (lastPos + 4), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            wkItemList.Cells[(lastPos + 4), 1, (lastPos + 4), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        }
                        //fill headers services
                        wkItemList.Cells["E" + (lastPos).ToString()].Value = ModEptFill.EventName;//Event
                        wkItemList.Cells["E" + (lastPos + 1).ToString()].Value = SILList[x].Salon;//Event
                        wkItemList.Cells["E" + (lastPos + 2).ToString()].Value = SILList[x].Asistentes;//Event
                        wkItemList.Cells["E" + (lastPos + 3).ToString()].Value = SILList[x].Montaje;//Event
                        wkItemList.Cells["E" + (lastPos + 4).ToString()].Value = SILList[x].Horario;//Event
                        lastPos = lastPos + 5;
                        //fill itemlist 
                        List<Models.SyncPSAV.ItemListServices> LILS = ConSQL.GetOneILIL(SILList[x].IDEvt, SILList[x].IDITL);
                        for (int a = 0; a < LILS.Count; a++)
                        {
                            wkItemList.Cells["B" + (lastPos).ToString()].Value = LILS[a].Clave;//Event
                            wkItemList.Cells["C" + (lastPos).ToString()].Value = LILS[a].Cantidad;//Event
                            wkItemList.Cells["D" + (lastPos).ToString()].Value = LILS[a].Dias;//Event
                            wkItemList.Cells["E" + (lastPos).ToString()].Value = LILS[a].Descripcion;//Event
                            wkItemList.Cells["F" + (lastPos).ToString()].Value = LILS[a].PrecioUnit;//Event
                            wkItemList.Cells["I" + (lastPos).ToString()].Value = LILS[a].Categoria;//Event
                            if (lastPos >= 41) { wkItemList.InsertRow(lastPos, 1); wkItemList.Cells[lastPos + 1, 1, lastPos + 1, 40].Copy(wkItemList.Cells[lastPos, 1, lastPos, 40]); }
                            lastPos++;
                        }
                        if (lastPos >= 41)
                        {
                            wkItemList.DeleteRow(lastPos);
                        }
                    }
                    int LastPosFormat = lastPos - 2;
                    if (lastPos <= 41) { lastPos = 55; }
                    else
                    {
                        lastPos = lastPos + 12;
                    }
                    int maxFormula = 3; bool sumaFilas = false;

                    List<Models.SyncPSAV.SalonILWF> SILWF = ConSQL.GetSalonsWF(ModEptFill.IDEvent);
                    for (int x = 0; x < SILWF.Count; x++)
                    {
                        //fill gray blanks
                        if (x > 0 && !sumaFilas) { sumaFilas = true; }
                        wkItemList.InsertRow(lastPos, 5);
                        wkItemList.Cells["B" + (lastPos).ToString()].Value = "EVENTO:";
                        wkItemList.Cells["B" + (lastPos + 1).ToString()].Value = "Salón/Area:";
                        wkItemList.Cells["B" + (lastPos + 2).ToString()].Value = "# de Asistentes:";
                        wkItemList.Cells["B" + (lastPos + 3).ToString()].Value = "Montaje:";
                        wkItemList.Cells["B" + (lastPos + 4).ToString()].Value = "Horario de Evento:";
                        wkItemList.Cells[lastPos, 1, lastPos, 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        wkItemList.Cells[lastPos, 1, lastPos, 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        wkItemList.Cells[(lastPos + 1), 1, (lastPos + 1), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        wkItemList.Cells[(lastPos + 1), 1, (lastPos + 1), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        wkItemList.Cells[(lastPos + 2), 1, (lastPos + 2), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        wkItemList.Cells[(lastPos + 2), 1, (lastPos + 2), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        wkItemList.Cells[(lastPos + 3), 1, (lastPos + 3), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        wkItemList.Cells[(lastPos + 3), 1, (lastPos + 3), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                        wkItemList.Cells[(lastPos + 4), 1, (lastPos + 4), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        wkItemList.Cells[(lastPos + 4), 1, (lastPos + 4), 7].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                        //fill headers workforce
                        wkItemList.Cells["E" + (lastPos).ToString()].Value = ModEptFill.EventName;//Event
                        wkItemList.Cells["E" + (lastPos + 1).ToString()].Value = SILWF[x].Salon;//Event
                        wkItemList.Cells["E" + (lastPos + 2).ToString()].Value = SILWF[x].Asistentes;//Event
                        wkItemList.Cells["E" + (lastPos + 3).ToString()].Value = SILWF[x].Montaje;//Event
                        wkItemList.Cells["E" + (lastPos + 4).ToString()].Value = SILWF[x].Horario;//Event
                        lastPos = lastPos + 5;
                        //fill itemlist wf
                        List<Models.SyncPSAV.ItemListWorkForce> LIWF = ConSQL.GetOneILWF(SILWF[x].IDEvt, SILWF[x].IDITL);
                        if ((LIWF.Count >= maxFormula) || sumaFilas == true) { sumaFilas = true; }
                        for (int o = 0; o < LIWF.Count; o++)
                        {
                            wkItemList.Cells["B" + (lastPos).ToString()].Value = LIWF[o].Clave;//Event
                            wkItemList.Cells["C" + (lastPos).ToString()].Value = LIWF[o].Cantidad;//Event
                            wkItemList.Cells["D" + (lastPos).ToString()].Value = LIWF[o].Dias;//Event
                            wkItemList.Cells["E" + (lastPos).ToString()].Value = LIWF[o].Descripcion;//Event
                            wkItemList.Cells["F" + (lastPos).ToString()].Value = LIWF[o].PrecioUnit;//Event
                            wkItemList.Cells["I" + (lastPos).ToString()].Value = LIWF[o].Categoria;//Event
                            if (sumaFilas)
                            {
                                wkItemList.InsertRow(lastPos, 1);
                                wkItemList.Cells[lastPos + 1, 1, lastPos + 1, 40].Copy(wkItemList.Cells[lastPos, 1, lastPos, 40]);
                            }
                            lastPos++;
                        }

                    }
                    wkItemList.DeleteRow(lastPos - 1); wkItemList.DeleteRow(lastPos);
                    //check discount
                    ExcelWorksheet VdItemList = package.Workbook.Worksheets[4];
                    List<Models.SyncPSAV.VentaDes> VDes = ConSQL.VDesCount(ModEptFill.IDEvent);
                    for (int i = 0; i < VDes.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(VDes[i].DesPorEq))
                        {
                            switch (VDes[i].Category)
                            {
                                case "AUDIO": VdItemList.Cells["F19"].Value = VDes[i].DesPorEq + "%"; break;
                                case "VIDEO": VdItemList.Cells["F20"].Value = VDes[i].DesPorEq + "%"; break;
                                case "ACCESORIOS": VdItemList.Cells["F21"].Value = VDes[i].DesPorEq + "%"; break;
                                case "ILUMINACION": VdItemList.Cells["F22"].Value = VDes[i].DesPorEq + "%"; break;
                                case "VIDEO PRODUCCION": VdItemList.Cells["F23"].Value = VDes[i].DesPorEq + "%"; break;
                                case "ESCENOGRAFIA": VdItemList.Cells["F24"].Value = VDes[i].DesPorEq + "%"; break;
                                case "COMPUTO": VdItemList.Cells["F25"].Value = VDes[i].DesPorEq + "%"; break;
                                case "RIGGING EQUIPO": VdItemList.Cells["F26"].Value = VDes[i].DesPorEq + "%"; break;
                                case "GASTOS": VdItemList.Cells["F27"].Value = VDes[i].DesPorEq + "%"; break;
                                case "OTROS": VdItemList.Cells["F28"].Value = VDes[i].DesPorEq + "%"; break;
                            }
                        }
                    }
                    //check fee sale
                    ExcelWorksheet FSItemList = package.Workbook.Worksheets[5];
                    List<Models.SyncPSAV.VentaFee> VFee = ConSQL.GetVFee(ModEptFill.IDEvent);
                    for (int i = 0; i < VFee.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(VFee[i].PorcFee))
                        {
                            switch (VFee[i].Category)
                            {
                                case "AUDIO": FSItemList.Cells["C59"].Value = VFee[i].PorcFee + "%"; break;
                                case "VIDEO": FSItemList.Cells["C60"].Value = VFee[i].PorcFee + "%"; break;
                                case "ACCESORIOS": FSItemList.Cells["C61"].Value = VFee[i].PorcFee + "%"; break;
                                case "ILUMINACION": FSItemList.Cells["C62"].Value = VFee[i].PorcFee + "%"; break;
                                case "VIDEO PRODUCCION": FSItemList.Cells["C63"].Value = VFee[i].PorcFee + "%"; break;
                                case "ESCENOGRAFIA": FSItemList.Cells["C64"].Value = VFee[i].PorcFee + "%"; break;
                                case "COMPUTO": FSItemList.Cells["C65"].Value = VFee[i].PorcFee + "%"; break;
                                case "RIGGING EQUIPO": FSItemList.Cells["C66"].Value = VFee[i].PorcFee + "%"; break;
                                case "GASTOS": FSItemList.Cells["C67"].Value = VFee[i].PorcFee + "%"; break;
                                case "OPERADOR": FSItemList.Cells["C68"].Value = VFee[i].PorcFee + "%"; break;
                                case "MONTAJE-DESMONTAJE": FSItemList.Cells["C68"].Value = VFee[i].PorcFee + "%"; break;
                                case "CARGO POR SERVICIO": FSItemList.Cells["C68"].Value = VFee[i].PorcFee + "%"; break;
                                case "RIGGING": FSItemList.Cells["C68"].Value = VFee[i].PorcFee + "%"; break;
                                case "OUTSIDE LABOR": FSItemList.Cells["C68"].Value = VFee[i].PorcFee + "%"; break;
                            }
                        }
                    }
                    //check for subrent
                    ExcelWorksheet WSSubrenta = package.Workbook.Worksheets[6];
                    List<Models.SyncPSAV.SubRenta> SR = ConSQL.GetSubRenta(ModEptFill.IDEvent);
                    for (int i = 0; i < SR.Count; i++)
                    {
                        WSSubrenta.Cells["A" + (i + 12).ToString()].Value = SR[i].Category;
                        WSSubrenta.Cells["B" + (i + 12).ToString()].Value = SR[i].Invoice;
                        WSSubrenta.Cells["C" + (i + 12).ToString()].Value = SR[i].Supplier;
                        WSSubrenta.Cells["D" + (i + 12).ToString()].Value = SR[i].Descripcion;
                        WSSubrenta.Cells["E" + (i + 12).ToString()].Value = SR[i].Gastosub;
                        WSSubrenta.Cells["F" + (i + 12).ToString()].Value = SR[i].Ventasub;
                        WSSubrenta.Cells["F" + (i + 12).ToString()].Style.Numberformat.Format = "0.00";
                        WSSubrenta.Cells["E" + (i + 12).ToString()].Style.Numberformat.Format = "0.00";
                    }
                    //check for OL
                    ExcelWorksheet WSOL = package.Workbook.Worksheets[7];
                    //check freelance
                    List<Models.SyncPSAV.FreelanceOL> FOL = ConSQL.GetFOLXls(ModEptFill.IDEvent);
                    //Fill Freelance

                    for (int i = 0; i < FOL.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 12).ToString()].Value = FOL[i].Nombres;
                        WSOL.Cells["B" + (i + 12).ToString()].Value = FOL[i].Puesto;
                        WSOL.Cells["C" + (i + 12).ToString()].Value = FOL[i].Dias;
                        WSOL.Cells["D" + (i + 12).ToString()].Value = FOL[i].Sueldo;
                        WSOL.Cells["D" + (i + 12).ToString()].Style.Numberformat.Format = "0.00";
                    }

                    //Check viaticos
                    List<Models.SyncPSAV.Viaticos> VTS = ConSQL.GetListVL(ModEptFill.IDEvent);
                    for (int i = 0; i < VTS.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 44).ToString()].Value = VTS[i].Nombres;
                        WSOL.Cells["B" + (i + 44).ToString()].Value = VTS[i].Puesto;
                        WSOL.Cells["C" + (i + 44).ToString()].Value = VTS[i].Observaciones;
                        WSOL.Cells["D" + (i + 44).ToString()].Value = VTS[i].TotalSol;
                        WSOL.Cells["D" + (i + 44).ToString()].Style.Numberformat.Format = "0.00";
                    }

                    //comision de ventas
                    List<Models.SyncPSAV.VentasFeeTot> VFE = ConSQL.GetListVFT(ModEptFill.IDEvent);
                    for (int i = 0; i < VFE.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 59).ToString()].Value = VFE[i].Nombres;
                        WSOL.Cells["B" + (i + 59).ToString()].Value = VFE[i].Puesto;
                        WSOL.Cells["C" + (i + 59).ToString()].Value = VFE[i].Comision;
                        WSOL.Cells["C" + (i + 59).ToString()].Style.Numberformat.Format = "0.00%";
                        //WSOL.Cells["D" + (i + 59).ToString()].Value = VFE[i].VentaNeta;

                    }
                    //Gastos Financieros
                    List<Models.SyncPSAV.GastosFinancieros> GF = ConSQL.GetListGF(ModEptFill.IDEvent);
                    for (int i = 0; i < GF.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 68).ToString()].Value = GF[i].ImporteCom;
                        WSOL.Cells["B" + (i + 68).ToString()].Value = GF[i].Comision;
                        WSOL.Cells["C" + (i + 68).ToString()].Value = GF[i].Importe;

                    }



                    //Consumibles

                    List<Models.SyncPSAV.Consumibles> CNS = ConSQL.GetListConsumibles(ModEptFill.IDEvent);
                    for (int i = 0; i < CNS.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 76).ToString()].Value = CNS[i].Cotizacion;
                        WSOL.Cells["B" + (i + 76).ToString()].Value = CNS[i].Supplier;
                        WSOL.Cells["C" + (i + 76).ToString()].Value = CNS[i].Description;
                        WSOL.Cells["D" + (i + 76).ToString()].Value = CNS[i].Costo;
                        WSOL.Cells["D" + (i + 76).ToString()].Style.Numberformat.Format = "0.00";
                    }

                    //Cargos internos
                    List<Models.SyncPSAV.CargosInternos> CI = ConSQL.GetListCI(ModEptFill.IDEvent);
                    for (int i = 0; i < CI.Count; i++)
                    {
                        WSOL.Cells["A" + (i + 92).ToString()].Value = CI[i].Equipo;
                        WSOL.Cells["B" + (i + 92).ToString()].Value = CI[i].Categoria;
                        WSOL.Cells["C" + (i + 92).ToString()].Value = CI[i].PrecioLista;
                        WSOL.Cells["D" + (i + 92).ToString()].Value = CI[i].TipoOper;
                    }






                    //finally save the EPT document
                    ////package.Save();
                    ReportArray = package.GetAsByteArray();
                }
                return File(ReportArray, XlsxContentType, "EPT_" + folio + "_" + System.DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
            catch (Exception ex) { ViewBag.datashow = ex.Message; return View(); }
        }
        #endregion
        #region CRATIOOne
        public IActionResult ExportCratioOne()
        {
            string IDC = Request.Cookies["IDCOne"].ToString();
            var fileDownloadName = "CROne.xlsx";
            var reportsFolder = "reports";
            var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
            byte[] ReportArray = null;
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {

                ExcelWorksheet exlWork = package.Workbook.Worksheets[5];
                List<Models.CaptureRatio.CRVResumenModel> CRVRM = ConSQL.GetCRatioExport(IDC);
                //fill header
                exlWork.Cells["B2"].Value = CRVRM[0].HotelName;
                exlWork.Cells["B3"].Value = CRVRM[0].CityLoc;
                exlWork.Cells["B4"].Value = CRVRM[0].LocationHotel;
                exlWork.Cells["B5"].Value = CRVRM[0].DET;
                exlWork.Cells["B6"].Value = CRVRM[0].Contact;
                exlWork.Cells["B7"].Value = CRVRM[0].FillFormName;
                //fill months
                List<Models.CaptureRatio.CRatioList> CRL = ConSQL.GCRLExport(IDC);
                int cellHeader1 = 12;
                int cellHeader2 = 13;
                int cellBody = 14;
                int cellCount = 0;
                for (int a = 1; a <= 12; a++)
                {
                    var varCRLF = CRL.Where(p => p.MesFiltro.Contains(a.ToString("00")));
                    List<Models.CaptureRatio.CRatioList> CRLF = new List<Models.CaptureRatio.CRatioList>();
                    foreach (Models.CaptureRatio.CRatioList inListCR in varCRLF)
                    {
                        CRLF.Add(inListCR);
                    }
                    if (!CRLF.Count.Equals(0))
                    {
                        exlWork.Cells["C" + cellHeader1.ToString()].Value = GetMonth(a.ToString("00"));
                        exlWork.Cells["C" + cellHeader1.ToString()].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        exlWork.Cells["C" + cellHeader1.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["C" + cellHeader1.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //titles
                        exlWork.Cells["A" + cellHeader2.ToString()].Value = "Fecha Inicio";
                        exlWork.Cells["B" + cellHeader2.ToString()].Value = "Fecha Fin";
                        exlWork.Cells["C" + cellHeader2.ToString()].Value = "Nombre del Evento";
                        exlWork.Cells["D" + cellHeader2.ToString()].Value = "Cliente Final";
                        exlWork.Cells["E" + cellHeader2.ToString()].Value = "Nombre del contacto";
                        exlWork.Cells["F" + cellHeader2.ToString()].Value = "Contacto Cliente";
                        exlWork.Cells["G" + cellHeader2.ToString()].Value = "Agencia";
                        exlWork.Cells["H" + cellHeader2.ToString()].Value = "Tipo de evento";
                        exlWork.Cells["I" + cellHeader2.ToString()].Value = "Empresa A/V";
                        exlWork.Cells["J" + cellHeader2.ToString()].Value = "Responsable PSAV";
                        exlWork.Cells["K" + cellHeader2.ToString()].Value = "Monto Ventas PSAV";
                        exlWork.Cells["L" + cellHeader2.ToString()].Value = "Lost Business";
                        exlWork.Cells["M" + cellHeader2.ToString()].Value = "Suma";
                        exlWork.Cells["N" + cellHeader2.ToString()].Value = "Capture Ratio";
                        exlWork.Cells["O" + cellHeader2.ToString()].Value = "Monto de Comisión al Hotel";
                        exlWork.Cells["P" + cellHeader2.ToString()].Value = "Motivo de LB";
                        exlWork.Cells["Q" + cellHeader2.ToString()].Value = "Cuando y donde será el próximo evento";
                        //background
                        exlWork.Cells[cellHeader2, 1, cellHeader2, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        exlWork.Cells["A" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["B" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["C" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["D" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["E" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["F" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["G" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["H" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["I" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["J" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["K" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["L" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["M" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["N" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["O" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["P" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["Q" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        //font color
                        exlWork.Cells["A" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["B" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["C" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["D" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["E" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["F" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["G" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["H" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["I" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["J" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["K" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["L" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["M" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["N" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["O" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["P" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["Q" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    }
                    for (int i = 0; i < CRLF.Count; i++)
                    {
                        exlWork.Cells["A" + cellBody.ToString()].Value = CRL[i].Finicio;
                        exlWork.Cells["B" + cellBody.ToString()].Value = CRL[i].FFin;
                        exlWork.Cells["C" + cellBody.ToString()].Value = CRL[i].EventName;
                        exlWork.Cells["D" + cellBody.ToString()].Value = CRL[i].FinalClient;
                        exlWork.Cells["E" + cellBody.ToString()].Value = CRL[i].ContactName;
                        exlWork.Cells["F" + cellBody.ToString()].Value = CRL[i].ContactContact;
                        exlWork.Cells["G" + cellBody.ToString()].Value = CRL[i].Agency;
                        exlWork.Cells["H" + cellBody.ToString()].Value = CRL[i].EventType;
                        exlWork.Cells["I" + cellBody.ToString()].Value = CRL[i].EmpresaAV;
                        exlWork.Cells["J" + cellBody.ToString()].Value = CRL[i].RsponsablePSAV;
                        exlWork.Cells["K" + cellBody.ToString()].Value = CRL[i].VentasPSAV;
                        exlWork.Cells["L" + cellBody.ToString()].Value = CRL[i].LB;
                        exlWork.Cells["M" + cellBody.ToString()].Value = CRL[i].Suma;
                        exlWork.Cells["N" + cellBody.ToString()].Formula = "(" + "K" + cellBody.ToString() + "*1)/" + ("M" + cellBody.ToString()) + "";
                        exlWork.Cells["N" + cellBody.ToString()].Calculate();
                        exlWork.Cells["O" + cellBody.ToString()].Value = CRL[i].HotelFee;
                        exlWork.Cells["P" + cellBody.ToString()].Value = CRL[i].LBCause;
                        exlWork.Cells["Q" + cellBody.ToString()].Value = CRL[i].NextEventDate + " " + CRL[i].NextEventPlace;
                        cellCount++;
                        cellHeader1 = cellHeader1 + cellCount;
                        cellHeader2 = cellHeader2 + cellCount;
                        cellBody = cellBody + cellCount;
                    }
                }
                ReportArray = package.GetAsByteArray();
            }
            return File(ReportArray, XlsxContentType, "CRatioOne.xlsx");
        }
        public string GetMonth(string MonthsStr)
        {
            string ValReturn = "";
            switch (MonthsStr)
            {
                case "01": ValReturn = "ENERO"; break;
                case "02": ValReturn = "FEBRERO"; break;
                case "03": ValReturn = "MARZO"; break;
                case "04": ValReturn = "ABRIL"; break;
                case "05": ValReturn = "MAYO"; break;
                case "06": ValReturn = "JUNIO"; break;
                case "07": ValReturn = "JULIO"; break;
                case "08": ValReturn = "AGOSTO"; break;
                case "09": ValReturn = "SEPTIEMBRE"; break;
                case "10": ValReturn = "OCTUBRE"; break;
                case "11": ValReturn = "NOVIEMBRE"; break;
                case "12": ValReturn = "DICIEMBRE"; break;
            }
            return ValReturn;
        }
        #endregion
    }
}
