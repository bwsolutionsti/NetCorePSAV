using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.JavaScript;
using GCCorePSAV.Models;

namespace GCCorePSAV.Controllers
{
    public class PSAVCrudController : Controller
    {
        Data.clsQuery ConSQL = new Data.clsQuery();
        // GET: /<controller>/
        
        public IActionResult ViewsManager()
        {
            List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> VMList = ConSQL.GetVMList(string.Empty);
            ViewBag.VMList = VMList;
            return View();
        }
        [HttpPost]
        public IActionResult ViewsManager(Models.PSAVCrud.ViewManagerModel.ViewManagerSearch model)
        {
            List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> VMList = ConSQL.GetVMList(model.FullName);
            ViewBag.VMList = VMList;
            return View();
        }
        public IActionResult NewVM()
        {
            return View();
        }
        public IActionResult Clients(Models.PSAVCrud.ClientModel.ClientQuickSearch model)
        {
            List<Models.PSAVCrud.ClientModel.ClientSearch> ClientList = ConSQL.GetClients(model.FullName);
            ViewBag.CoinsList = ClientList;
            return View();
        }
        public IActionResult ContactType(Models.ContactTypeModel model)
        {
            return View();
        }
        //public IActionResult Roles()
        //{
        //    List<Models.PSAVCrud.RolesModel> RM = ConSQL.GetRoles();
        //    ViewData["Theme"] = "light";
        //    return View(RM.ToList());
        //}
        public IActionResult NewClient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewClientPost(Models.PSAVCrud.ClientModel.ClientNewAll model)
        {
            if (ModelState.IsValid)
            {
                ConSQL.SaveClient(model);
                return View("Clients");
            }
            else
            {
                return View();
            }
        }
        #region NewSyncFCrud
        #region Users
        public static List<Models.PSAVCrud.SyncModels.UsersModel> UsersList = new List<Models.PSAVCrud.SyncModels.UsersModel>();
        [HttpPost]
        public ActionResult EditUsuario(string usn)
        {
            Models.PSAVCrud.SyncModels.UsersModel UM = ConSQL.GetUser(usn);
            return View(UM);
        }
        [HttpPost]
        public ActionResult NewUser()
        {
            return View();
        }
        public ActionResult SaveNewUser(Models.PSAVCrud.SyncModels.NewUser model)
        {
            string IDClient = ConSQL.SavePerson(model);
            Models.PSAVCrud.SyncModels.UsersModel UM = new Models.PSAVCrud.SyncModels.UsersModel();
            UM.Active = "1";
            UM.Expira = model.Expira;
            UM.Persona = IDClient;
            UM.Username = model.UserName;
            UM.Password = model.Password;
            string IDU=ConSQL.UpdateUsers(UM, 0);
            ConSQL.SaveRolUser(IDU, "1");
            return RedirectToAction("Usuarios");
        }
        public ActionResult SaveUserEdit(Models.PSAVCrud.SyncModels.UsersModel model)
        {
            ConSQL.UpdateUsers(model, 1);
            return RedirectToAction("Usuarios");
        }
        public ActionResult Usuarios()
        {
            //if (UsersList.Count.Equals(0))
                BindDataSourceUsers();
            ViewBag.datasource = UsersList;
            return View();
        }
        public void BindDataSourceUsers() { UsersList = ConSQL.GetUsers(); }
        public ActionResult UserNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.SyncModels.UsersModel val = UsersList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Active = ord.Active; val.Username = ord.Username; val.Expira = ord.Expira;
            ConSQL.UpdateUsers(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult UserNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> value)
        {
            Models.PSAVCrud.SyncModels.UsersModel val = value.Value;
            val.ID = ConSQL.UpdateUsers(value.Value, 0);
            UsersList.Insert(CoinsList.Count, val);
            return Json(CoinsList);
        }
        public ActionResult UserNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> value)
        {
            UsersList.Remove(UsersList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            ConSQL.UpdateUsers(value.Value, 2);
            return Json(value);
        }
        #endregion
        #region TipoEvento
        public static List<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> TEList = new List<NetCorePSAV.Models.PSAVCrud.TipoEventoModel>();
        public ActionResult TipoEvento()
        {
            if (MLBList.Count.Equals(0))
            {
                BindDataSourceTE();
            }
            ViewBag.datasource = MLBList;
            return View();
        }
        public void BindDataSourceTE() { TEList = QueryCrud.GetTipoEventos(); }
        public ActionResult TENormalUpdate([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> myObject)
        {
            var ord = myObject.Value;
            NetCorePSAV.Models.PSAVCrud.TipoEventoModel val = TEList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Nombre = ord.Nombre;
            QueryCrud.UpdateTE(1, val);
            return Json(myObject.Value);
        }
        public ActionResult TENormalInsert([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.TipoEventoModel val = value.Value;
            val.ID = QueryCrud.UpdateTE(0, value.Value);
            TEList.Insert(TEList.Count, val);
            return Json(TEList);
        }
        public ActionResult TENormalDelete([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.TipoEventoModel val = new NetCorePSAV.Models.PSAVCrud.TipoEventoModel();
            val.ID = value.Key.ToString();
            QueryCrud.UpdateTE(2, val);
            TEList.Remove(TEList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region MotivoLB
        public static List<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> MLBList = new List<NetCorePSAV.Models.PSAVCrud.MotivosLBModel>();
        public ActionResult MotivosLB()
        {
            if (MLBList.Count.Equals(0))
            {
                BindDataSourceMLB();
            }
            ViewBag.datasource = MLBList;
            return View();
        }
        public void BindDataSourceMLB() { MLBList = QueryCrud.GetMotivos(); }
        public ActionResult MLBNormalUpdate([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> myObject)
        {
            var ord = myObject.Value;
            NetCorePSAV.Models.PSAVCrud.MotivosLBModel val = MLBList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Nombre = ord.Nombre;
            QueryCrud.UpdateMLB(1, val);
            return Json(myObject.Value);
        }
        public ActionResult MLBNormalInsert([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.MotivosLBModel val = value.Value;
            val.ID = QueryCrud.UpdateMLB(0, value.Value);
            MLBList.Insert(MLBList.Count, val);
            return Json(MLBList);
        }
        public ActionResult MLBNormalDelete([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.MotivosLBModel val = new NetCorePSAV.Models.PSAVCrud.MotivosLBModel();
            val.ID = value.Key.ToString();
            QueryCrud.UpdateMLB(2, val);
            MLBList.Remove(MLBList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region Etiquetas
        public static List<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> EtiquetasList = new List<NetCorePSAV.Models.PSAVCrud.EtiquetasModel>();
        public ActionResult Etiquetas()
        {
            if (EtiquetasList.Count.Equals(0))
            {
                BindDataSourceTag();
            }
            ViewBag.datasource = EtiquetasList;
            return View();
        }
        public void BindDataSourceTag() { EtiquetasList= QueryCrud.GetEtiquetas(); }
        public ActionResult TagNormalUpdate([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> myObject)
        {
            var ord = myObject.Value;
            NetCorePSAV.Models.PSAVCrud.EtiquetasModel val = EtiquetasList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Nombre = ord.Nombre; 
            QueryCrud.UpdateEtiquetas(1, val);
            return Json(myObject.Value);
        }
        public ActionResult TagNormalInsert([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.EtiquetasModel val = value.Value;
            val.ID = QueryCrud.UpdateEtiquetas(0, value.Value);
            EtiquetasList.Insert(EtiquetasList.Count, val);
            return Json(EtiquetasList);
        }
        public ActionResult TagNormalDelete([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.EtiquetasModel val = new NetCorePSAV.Models.PSAVCrud.EtiquetasModel();
            val.ID = value.Key.ToString();
            QueryCrud.UpdateEtiquetas(2, val);
            EtiquetasList.Remove(EtiquetasList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region Location
        public static List<NetCorePSAV.Models.PSAVCrud.LocationModel> LocationList = new List<NetCorePSAV.Models.PSAVCrud.LocationModel>();
        public ActionResult Location()
        {
            if (LocationList.Count.Equals(0))
            {
                BindDataSourceLocation();
            }
            ViewBag.datasource = LocationList;
            return View();
        }
        public Data.ClsQueryCrudSensitive QueryCrud = new Data.ClsQueryCrudSensitive();
        public void BindDataSourceLocation() { LocationList = QueryCrud.GetLocations(); }
        public ActionResult LocationNormalUpdate([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.LocationModel> myObject)
        {
            var ord = myObject.Value;
            NetCorePSAV.Models.PSAVCrud.LocationModel val = LocationList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Nombre = ord.Nombre; val.Numero= ord.Numero; val.Region= ord.Region;val.Ciudad = ord.Ciudad;
            QueryCrud.UpdateLocation(1,val);
            return Json(myObject.Value);
        }
        public ActionResult LocationNormalInsert([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.LocationModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.LocationModel val = value.Value;
            val.ID = QueryCrud.UpdateLocation( 0,value.Value);
            LocationList.Insert(LocationList.Count, val);
            return Json(LocationList);
        }
        public ActionResult LocationNormalDelete([FromBody]CRUDModel<NetCorePSAV.Models.PSAVCrud.LocationModel> value)
        {
            NetCorePSAV.Models.PSAVCrud.LocationModel val = new NetCorePSAV.Models.PSAVCrud.LocationModel();
            val.ID = value.Key.ToString();
            QueryCrud.UpdateLocation(2,val);
            LocationList.Remove(LocationList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region Coins
        public static List<Models.PSAVCrud.SyncModels.CoinModel> CoinsList = new List<Models.PSAVCrud.SyncModels.CoinModel>();
        public ActionResult Coins()
        {
            if (CoinsList.Count.Equals(0))
                BindDataSourceCoins();
            ViewBag.datasource = CoinsList;
            return View();
        }
        public void BindDataSourceCoins() { CoinsList= ConSQL.GetCoins(); }
        public ActionResult CoinNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.SyncModels.CoinModel val = CoinsList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Name = ord.Name; val.Change = ord.Change; val.Active=ord.Active;
            ConSQL.UpdateCoins(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult CoinNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> value)
        {
            Models.PSAVCrud.SyncModels.CoinModel val = value.Value;
            val.ID = ConSQL.UpdateCoins(value.Value, 0);
            CoinsList.Insert(CoinsList.Count, val);
            return Json(CoinsList);
        }
        public ActionResult CoinNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> value)
        {
            Models.PSAVCrud.SyncModels.CoinModel val = new Models.PSAVCrud.SyncModels.CoinModel();
            val.ID = value.Key.ToString();
            ConSQL.UpdateCoins(val, 2);
            CoinsList.Remove(CoinsList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }

        //Sensitive 
        
        public ActionResult Sensitive()
        {
            GCCorePSAV.Data.ClsQueryCrudSensitive Nueva = new Data.ClsQueryCrudSensitive(); //Estamos creando una nueva variable llamada Nueva y estamos 
            Senslist = Nueva.GetcategSens(); //estamos creando una variable y estamos guardadno la lista de getcateg 
            ViewBag.datasource = Senslist;//esta es nuestra bolsita que guarda la información por el momento 
            return View();//Aquí estamos regresando la vista de "Nuevatabla"
        }
        Data.ClsQueryCrudSensitive TabSQLSens = new Data.ClsQueryCrudSensitive();//agregamos Una varable llamada tabsql y la estamos indexando con los deatos de clsQueryCrud
        public static List<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> Senslist = new List<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla>(); //estamos creando Una lista Vacia llamada Tablanuevalist 
        public void BindDataTablasens() { Senslist = TabSQLSens.GetcategSens(); }

        public ActionResult SensitiveUpdate([FromBody]CRUDModel<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> myObject)// Se va a llenar una nueva tabla con los datos de el html
        {

            var ord = myObject.Value;
            Models.PSAVCrud.SensitiveModel.SensitiveModelTabla valsens = Senslist.Where(or => or.tcs_id == ord.tcs_id).FirstOrDefault();//Aquí estariamos guardando lo obtenido en el modelo
            valsens.tcs_id = ord.tcs_id; valsens.tcs_name = ord.tcs_name; valsens.tcs_description = ord.tcs_description;
            TabSQLSens.UpdateSens(valsens, 1);
            return Json(myObject.Value);

        }
        public ActionResult SensitiveInsert([FromBody]CRUDModel<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.SensitiveModel.SensitiveModelTabla valsens = value.Value;//Estamos creando una Variable llamada Val
            valsens.tcs_id = Convert.ToInt32(TabSQLSens.UpdateSens(value.Value, 0));
            Senslist.Insert(Senslist.Count, valsens);
            return Json(Senslist);
        }
        public ActionResult SensitiveDelete([FromBody]CRUDModel<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.SensitiveModel.SensitiveModelTabla val2 = new Models.PSAVCrud.SensitiveModel.SensitiveModelTabla();//Estamos creando una variable llamada val y estamos Gurdando los datos cambiados
            val2.tcs_id = Convert.ToInt32(value.Key.ToString());
            TabSQLSens.UpdateSens(val2, 2);
            Senslist.Remove(Senslist.Where(or => or.tcs_id == Convert.ToUInt32(value.Key.ToString())).FirstOrDefault());
            return Json(value);

        }
        //Lob Tabla
        public ActionResult Lob()
        {
            GCCorePSAV.Data.ClsQueryCrudLob Nueva = new Data.ClsQueryCrudLob(); //Creamos una variable la cual nos dirigira a la conexion con la BD
            Loblist = Nueva.GetcategLob(); //Aqui Estamos obteniendo los datos obtenidos en el CLSQuery y guardandolos en loblist; cuando ponemos nueva.Getcateglob
            //le estamos indicando cual es la clase a la que nos estamos dirigiendo
            ViewBag.datasource = Loblist;//esta es nuestra bolsita que guarda la información por el momento 
            return View();//Aquí estamos regresando la vista de "Nuevatabla"
        }
        
        public static List<Models.PSAVCrud.LobModel.LobModelTabla> Loblist = new List<Models.PSAVCrud.LobModel.LobModelTabla>(); //estamos creando Una lista Vacia llamada Tablanuevalist 
        public void BindDataLob() { Loblist = TabSQLLob.GetcategLob(); } //sincoroniza los datos de origencon los datos adquiridos

        Data.ClsQueryCrudLob TabSQLLob = new Data.ClsQueryCrudLob();//Utilizamos esta variable para poder sincronizar los datos y poder borrarlos, insertar o editar los datos
        public ActionResult LobUpdate([FromBody]CRUDModel<Models.PSAVCrud.LobModel.LobModelTabla> myObject)// Se va a llenar una nueva tabla con los datos de el html
        {

            var ord = myObject.Value;
            Models.PSAVCrud.LobModel.LobModelTabla val2 = Loblist.Where(or => or.tclb_id == ord.tclb_id).FirstOrDefault();//Aquí estariamos guardando lo obtenido en el modelo
            val2.tclb_id = ord.tclb_id; val2.tclb_name = ord.tclb_name; val2.tclb_description = ord.tclb_description; val2.tclb_leader = ord.tclb_leader ;
            TabSQLLob.UpdateNuevatabla(val2, 1);
            return Json(myObject.Value);

        }
        public ActionResult LobInsert([FromBody]CRUDModel<Models.PSAVCrud.LobModel.LobModelTabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.LobModel.LobModelTabla val2 = value.Value;//Estamos creando una Variable llamada Val2 para poder guardar los obtendio osea lo que inserto el cliente
            val2.tclb_id = Convert.ToInt32(TabSQLLob.UpdateNuevatabla(value.Value, 0));//estamos accediendo a la clase Updatenuevatabla
            Loblist.Insert(Loblist.Count, val2);
            ViewBag.datasource = Loblist;
            return Json(Loblist);
        }
        public ActionResult LobDelete([FromBody]CRUDModel<Models.PSAVCrud.LobModel.LobModelTabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.LobModel.LobModelTabla val2 = new Models.PSAVCrud.LobModel.LobModelTabla();//Estamos creando una variable llamada val y estamos Gurdando los datos cambiados
            val2.tclb_id = Convert.ToInt32(value.Key.ToString());
            TabSQLLob.UpdateNuevatabla(val2, 2);
            Loblist.Remove(Loblist.Where(or => or.tclb_id == Convert.ToUInt32(value.Key.ToString())).FirstOrDefault());
            return Json(value);
        }

        //Comvta 
        public ActionResult Comvta()
        {
            GCCorePSAV.Data.ClsQueryCrud2 Nueva = new Data.ClsQueryCrud2(); //Estamos creando una nueva variable llamada Nueva y estamos 
            comvtalist = Nueva.Getcateg2(); //estamos creando una variable y estamos guardadno la lista de getcateg 
            ViewBag.datasource = comvtalist;//esta es nuestra bolsita que guarda la información por el momento 
            return View();//Aquí estamos regresando la vista de "Nuevatabla"
        }
        Data.ClsQueryCrud2 TabSQL2 = new Data.ClsQueryCrud2();//agregamos Una varable llamada tabsql y la estamos indexando con los deatos de clsQueryCrud
        public static List<Models.PSAVCrud.Comvta.Comvtatabla> comvtalist = new List<Models.PSAVCrud.Comvta.Comvtatabla>(); //estamos creando Una lista Vacia llamada Tablanuevalist 
        public void BindDataTabla2() { comvtalist = TabSQL2.Getcateg2(); }

        public ActionResult ComvtaUpdate([FromBody]CRUDModel<Models.PSAVCrud.Comvta.Comvtatabla> myObject)// Se va a llenar una nueva tabla con los datos de el html
        {

            var ord = myObject.Value;
            Models.PSAVCrud.Comvta.Comvtatabla val = comvtalist.Where(or => or.tc_cvid == ord.tc_cvid).FirstOrDefault();//Aquí estariamos guardando lo obtenido en el modelo
            val.tc_cvid = ord.tc_cvid; val.tc_cvfee = ord.tc_cvfee; val.tc_cvtext = ord.tc_cvtext;
            TabSQL2.UpdateComvta(val, 1);
            return Json(myObject.Value);

        }
        public ActionResult ComvtaInsert([FromBody]CRUDModel<Models.PSAVCrud.Comvta.Comvtatabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.Comvta.Comvtatabla val2 = value.Value;//Estamos creando una Variable llamada Val
            val2.tc_cvid = Convert.ToInt32(TabSQL2.UpdateComvta(value.Value, 0));
            comvtalist.Insert(comvtalist.Count, val2);
            return Json(comvtalist);
        }
        public ActionResult ComvtaDelete([FromBody]CRUDModel<Models.PSAVCrud.Comvta.Comvtatabla> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.Comvta.Comvtatabla val2 = new Models.PSAVCrud.Comvta.Comvtatabla();//Estamos creando una variable llamada val y estamos Gurdando los datos cambiados
            val2.tc_cvid = Convert.ToInt32(value.Key.ToString());
            TabSQL2.UpdateComvta(val2, 2);
            comvtalist.Remove(comvtalist.Where(or => or.tc_cvid == Convert.ToUInt32(value.Key.ToString())).FirstOrDefault());
            return Json(value);

        }

        //Nueva Tabla agregada el 16/02/18
        public ActionResult Nuevatabla()
        {
            GCCorePSAV.Data.ClsQueryCrud Nueva = new Data.ClsQueryCrud(); //Estamos creando una nueva variable llamada Nueva y estamos 
            Tablanuevalist = Nueva.Getcateg(); //estamos creando una variable y estamos guardadno la lista de getcateg 
            ViewBag.datasource = Tablanuevalist;//esta es nuestra bolsita que guarda la información por el momento 
            return View();//Aquí estamos regresando la vista de "Nuevatabla"
        }
        Data.ClsQueryCrud TabSQL = new Data.ClsQueryCrud();//agregamos Una varable llamada tabsql y la estamos indexando con los deatos de clsQueryCrud
        public static  List<Models.PSAVCrud.SyncCrud.Tablanueva> Tablanuevalist=new List<Models.PSAVCrud.SyncCrud.Tablanueva>(); //estamos creando Una lista Vacia llamada Tablanuevalist 
        public void BindDataTabla() { Tablanuevalist = TabSQL.Getcateg(); }

        public ActionResult NuevatablaUpdate([FromBody]CRUDModel<Models.PSAVCrud.SyncCrud.Tablanueva> myObject)// Se va a llenar una nueva tabla con los datos de el html
        {
            
            var ord = myObject.Value;
            Models.PSAVCrud.SyncCrud.Tablanueva val2 = Tablanuevalist.Where(or => or.tcc_id == ord.tcc_id).FirstOrDefault();//Aquí estariamos guardando lo obtenido en el modelo
            val2.tcc_id = ord.tcc_id; val2.tcc_name = ord.tcc_name; val2.tcc_type = ord.tcc_type;
            TabSQL.UpdateNuevatabla(val2, 1);
            return Json(myObject.Value);

        }
        public ActionResult NuevatablaInsert([FromBody]CRUDModel<Models.PSAVCrud.SyncCrud.Tablanueva> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.SyncCrud.Tablanueva val2 = value.Value;//Estamos creando una Variable llamada Val
            val2.tcc_id = Convert.ToInt32(TabSQL.UpdateNuevatabla(value.Value, 0));
            Tablanuevalist.Insert(Tablanuevalist.Count, val2);
            return Json(Tablanuevalist);
        }
        public ActionResult NuevatablaDelete([FromBody]CRUDModel<Models.PSAVCrud.SyncCrud.Tablanueva> value)// Se va a llenar una nueva tabla con los datos de el html
        {
            Models.PSAVCrud.SyncCrud.Tablanueva val2 = new Models.PSAVCrud.SyncCrud.Tablanueva();//Estamos creando una variable llamada val y estamos Gurdando los datos cambiados
            val2.tcc_id = Convert.ToInt32(value.Key.ToString());
            TabSQL.UpdateNuevatabla(val2, 2);
            Tablanuevalist.Remove(Tablanuevalist.Where(or => or.tcc_id == Convert.ToUInt32(value.Key.ToString())).FirstOrDefault());
            return Json(value);
        }




       
        #endregion

        #region Roles
        public static List<Models.PSAVCrud.RolesModel> RolesList = new List<Models.PSAVCrud.RolesModel>();
        public ActionResult Roles()
        {
            if(RolesList.Count.Equals(0))
                BindDataSourceRoles();
            ViewBag.datasource = RolesList;
            return View();
        }
        public ActionResult RoleNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.RolesModel val = RolesList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID;val.Nombre = ord.Nombre;val.Descripcion = ord.Descripcion;
            ConSQL.UpdateRole(val,1);
            return Json(myObject.Value);
        }
        public ActionResult RoleNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> value)
        {
            Models.PSAVCrud.RolesModel val = value.Value;
            val.ID= ConSQL.UpdateRole(value.Value, 0);
            RolesList.Insert(RolesList.Count, val);            
            return Json(RolesList);
        }
        public ActionResult RoleNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> value)
        {
            RolesList.Remove(RolesList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            ConSQL.UpdateRole(value.Value, 2);
            return Json(value);
        }
        public void BindDataSourceRoles()
        {
            RolesList = ConSQL.GetRoles();
        }
#endregion
        #endregion
    }
}