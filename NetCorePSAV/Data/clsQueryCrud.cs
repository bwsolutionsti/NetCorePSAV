using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GCCorePSAV.Data
{
    //Controla base de datos Sensitive
    public class ClsQueryCrudSensitive
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";
        #region TipoEvento
        public List<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> GetTipoEventos()
        {
            string TabSQLSens = "SELECT * FROM psav_dev.tc_tipoevento";
            List<NetCorePSAV.Models.PSAVCrud.TipoEventoModel> locations = new List<NetCorePSAV.Models.PSAVCrud.TipoEventoModel>();
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQLSens, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read())
            {
                NetCorePSAV.Models.PSAVCrud.TipoEventoModel location = new NetCorePSAV.Models.PSAVCrud.TipoEventoModel();
                location.ID = sdr.GetValue(0).ToString();
                location.Nombre = sdr.GetValue(1).ToString();
                locations.Add(location);
            }
            conn.Close();
            return locations;
        }
        public string UpdateTE(int typeUpdate, NetCorePSAV.Models.PSAVCrud.TipoEventoModel locationModel)
        {
            string queryToInsert = ""; string valor = "";
            switch (typeUpdate)
            {
                case 0:
                    queryToInsert = "insert into psav_dev.tc_tipoevento (tcte_nombe) values('" + locationModel.Nombre + "')";
                    valor = SaveWithIDReturn(queryToInsert);
                    break;
                case 1:
                    queryToInsert = "update psav_dev.tc_tipoevento set tcte_nombe='" + locationModel.Nombre + "' where tcte_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
                case 2:
                    queryToInsert = "delete from psav_dev.tc_tipoevento where tcte_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
            }
            return valor;
        }
        #endregion
        #region MotivosLB
        public List<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> GetMotivos()
        {
            string TabSQLSens = "SELECT * FROM psav_dev.tc_motivolb";
            List<NetCorePSAV.Models.PSAVCrud.MotivosLBModel> locations = new List<NetCorePSAV.Models.PSAVCrud.MotivosLBModel>();
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQLSens, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read())
            {
                NetCorePSAV.Models.PSAVCrud.MotivosLBModel location = new NetCorePSAV.Models.PSAVCrud.MotivosLBModel();
                location.ID = sdr.GetValue(0).ToString();
                location.Nombre = sdr.GetValue(1).ToString();
                locations.Add(location);
            }
            conn.Close();
            return locations;
        }
        public string UpdateMLB(int typeUpdate, NetCorePSAV.Models.PSAVCrud.MotivosLBModel locationModel)
        {
            string queryToInsert = ""; string valor = "";
            switch (typeUpdate)
            {
                case 0:
                    queryToInsert = "insert into psav_dev.tc_motivolb (tcmlb_nombre) values('" + locationModel.Nombre + "')";
                    valor = SaveWithIDReturn(queryToInsert);
                    break;
                case 1:
                    queryToInsert = "update psav_dev.tc_motivolb set tcmlb_nombre='" + locationModel.Nombre + "' where tcmlb_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
                case 2:
                    queryToInsert = "delete from psav_dev.tc_motivolb where tcmlb_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
            }
            return valor;
        }
        #endregion
        #region Etiquetas
        public List<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> GetEtiquetas()
        {
            string TabSQLSens = "SELECT * FROM psav_dev.tc_etiquetas";
            List<NetCorePSAV.Models.PSAVCrud.EtiquetasModel> locations = new List<NetCorePSAV.Models.PSAVCrud.EtiquetasModel>();
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQLSens, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read())
            {
                NetCorePSAV.Models.PSAVCrud.EtiquetasModel location = new NetCorePSAV.Models.PSAVCrud.EtiquetasModel();
                location.ID = sdr.GetValue(0).ToString();
                location.Nombre = sdr.GetValue(1).ToString();
                locations.Add(location);
            }
            conn.Close();
            return locations;
        }
        public string UpdateEtiquetas(int typeUpdate, NetCorePSAV.Models.PSAVCrud.EtiquetasModel locationModel)
        {
            string queryToInsert = ""; string valor = "";
            switch (typeUpdate)
            {
                case 0:
                    queryToInsert = "insert into psav_dev.tc_etiquetas (tce_nombre) values('" + locationModel.Nombre + "')";
                    valor = SaveWithIDReturn(queryToInsert);
                    break;
                case 1:
                    queryToInsert = "update psav_dev.tc_etiquetas set tce_nombre='" + locationModel.Nombre + "' where tce_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
                case 2:
                    queryToInsert = "delete from psav_dev.tc_etiquetas where tce_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
            }
            return valor;
        }
        #endregion
        #region Location
        public List<NetCorePSAV.Models.PSAVCrud.LocationModel> GetLocations()
        {
            string TabSQLSens = "SELECT * FROM psav_dev.tc_location";
            List<NetCorePSAV.Models.PSAVCrud.LocationModel> locations = new List<NetCorePSAV.Models.PSAVCrud.LocationModel>();
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQLSens, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read())
            {
                NetCorePSAV.Models.PSAVCrud.LocationModel location = new NetCorePSAV.Models.PSAVCrud.LocationModel();
                location.ID = sdr.GetValue(0).ToString();
                location.Nombre = sdr.GetValue(1).ToString();
                location.Numero = sdr.GetValue(2).ToString();
                location.Region = sdr.GetValue(3).ToString();
                location.Ciudad = sdr.GetValue(4).ToString();
                locations.Add(location);
            }
            conn.Close();
            return locations;
        }
        public string UpdateLocation(int typeUpdate,NetCorePSAV.Models.PSAVCrud.LocationModel locationModel)
        {
            string queryToInsert = "";string valor = "";
            switch (typeUpdate)
            {
                case 0:
                    queryToInsert = "insert into psav_dev.tc_location (tcl_nombre,tcl_numero,tcl_region,tcl_ciudad) values('"+locationModel.Nombre+"','"+locationModel.Numero+"','"+locationModel.Region+"','"+locationModel.Ciudad+"')";
                    valor=SaveWithIDReturn(queryToInsert);
                    break;
                case 1:
                    queryToInsert = "update psav_dev.tc_location set tcl_nombre='"+locationModel.Nombre+"',tcl_numero='"+locationModel.Numero+"',tcl_region='"+locationModel.Region+"',tcl_ciudad='"+locationModel.Ciudad+"' where tcl_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
                case 2:
                    queryToInsert = "delete from psav_dev.tc_location where tcl_id=" + locationModel.ID;
                    SaveWithoutValidation(queryToInsert);
                    break;
            }
            return valor;
        }
        #endregion
        public List<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> GetcategSens()//estamos referenciando la información del modelo y lo estamos guardando en getcategsens
        {
            string TabSQLSens = "SELECT * FROM psav_dev.tc_sensitive";
            List<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla> Sens1 = new List<Models.PSAVCrud.SensitiveModel.SensitiveModelTabla>();//estamos referenciando Las variabes obtenidas en el modelo
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQLSens, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read()) //Estamos haciendo una iteracion y como condicion estamos diciendo que mientras lea
            {
                Models.PSAVCrud.SensitiveModel.SensitiveModelTabla _ListVM = new Models.PSAVCrud.SensitiveModel.SensitiveModelTabla(); // estamos refernciando al archivo puesto en la primer parte y lo estamos guardando en una nueva parte de la memoria 
                _ListVM.tcs_id = Convert.ToInt32(sdr.GetValue(0).ToString());//Estamos obteniendo los valores puestos en el modelo comvt y lo convertimos a string y las estamos guardando en la variable
                _ListVM.tcs_name = sdr.GetValue(1).ToString();
                _ListVM.tcs_description = sdr.GetValue(2).ToString();
                
                Sens1.Add(_ListVM);
            }
            conn.Close();
            return Sens1;
        }

        public string UpdateSens(Models.PSAVCrud.SensitiveModel.SensitiveModelTabla mod, int oper)
        {

            string Retorno = "";
            string QueryNuevaTabla = "";
            switch (oper)
            {
                 
                case 0:
                    QueryNuevaTabla = "insert into psav_dev.tc_sensitive(tcs_name,tcs_description,tcs_id) values ('" + mod.tcs_name.ToString().ToUpper() + "','" + mod.tcs_description + "','" + mod.tcs_id + "')";
                    Retorno = SaveWithIDReturn(QueryNuevaTabla);
                    break;
                case 1:
                    QueryNuevaTabla = "update psav_dev.tc_sensitive set tcs_name='" + mod.tcs_name.ToString() + "',tcs_description='" + mod.tcs_description.ToString() + "' where tcs_id=" + mod.tcs_id;
                  
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
                case 2:
                    QueryNuevaTabla = "Delete from psav_dev.tc_sensitive where tcs_id=" + mod.tcs_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
            }
            return Retorno;

        }
        public void SaveClient(Models.PSAVCrud.SensitiveModel.SensitiveModelTabla model)
        {

            //inserta TCC_name psav_dev.tc_sensitive
            string QueryToInsert = "insert into psav_dev.tc_sensitive;(tcs_id,tcs_name,tcs_description)" +
                "values('" + "," + model.tcs_id + "','" + model.tcs_name + "','" + model.tcs_description + "',null,'" + ")";
            string IDClient = "";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_ID
            QueryToInsert = "insert into psav_dev.tc_sensitive;(tcct_id,trcp_data,tmp_id) values(1,'" + model.tcs_id + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_sensitive;(tcct_id,trcp_data,tmp_id) values(2,'" + model.tcs_name + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_sensitive;(tcct_id,trcp_data,tmp_id) values(3,'" + model.tcs_description + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_Type
            QueryToInsert = "insert into psav_dev.tc_sensitive;(tcs_id,tcs_name,tcs_description) values(" + IDClient + ",'" + model.tcs_description + "','" + "')";
            SaveWithoutValidation(QueryToInsert);

        }

        public void SaveWithoutValidation(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string SaveWithIDReturn(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return lastID;
        }

    }
    //Controla base de datos LOB
    public class ClsQueryCrudLob
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";

        public List<Models.PSAVCrud.LobModel.LobModelTabla> GetcategLob()
        {
            string TabSQL = "SELECT * FROM psav_dev.tc_lob";//Estamos guardando "SELECT *FROM"
            List<Models.PSAVCrud.LobModel.LobModelTabla> Cients = new List<Models.PSAVCrud.LobModel.LobModelTabla>();//Estamos creando una variable la cual agregaremos los datos obtenidos de la BD y los convertira en una lista
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQL, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read()) //Estamos haciendo una iteracion y como condicion estamos diciendo que mientras lea
            {
                Models.PSAVCrud.LobModel.LobModelTabla _ListVM = new Models.PSAVCrud.LobModel.LobModelTabla(); //Estamos accediendo a los parametros del modelo el cual
                //Le dira a la base de datos que parametro queremos
                _ListVM.tclb_id = Convert.ToInt32(sdr.GetValue(0).ToString());//Estamos obteniendo los valores puestos en el modelo Syncrud y lo convertimos a string
                _ListVM.tclb_name = sdr.GetValue(1).ToString();
                _ListVM.tclb_description = sdr.GetValue(2).ToString();
                _ListVM.tclb_leader = sdr.GetValue(3).ToString();

                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }

        public string UpdateNuevatabla(Models.PSAVCrud.LobModel.LobModelTabla mod, int oper)
        {

            string Retorno = "";
            string QueryNuevaTabla = "";
            switch (oper)
            {

                case 0:
                    QueryNuevaTabla = "insert into psav_dev.tc_lob (tclb_name,tclb_description,tclb_leader,tclb_id) values('" + mod.tclb_name.ToString() + "','" + mod.tclb_description + "','" + mod.tclb_leader + "'," + mod.tclb_id +  ")";
                    Retorno = SaveWithIDReturn(QueryNuevaTabla);
                    break;
                case 1:
                    QueryNuevaTabla = "update psav_dev.tc_lob set tclb_name ='" + mod.tclb_name.ToString() + "', tclb_description='" + mod.tclb_description.ToString() + "', tclb_leader='" + mod.tclb_leader.ToString() + "' where tclb_id=" + mod.tclb_id;
                  //QueryNuevaTabla = "update psav_dev.tc_comvta set tc_cvtext='" + mod.tc_cvtext.ToString() + "', tc_cvfee='" + mod.tc_cvfee.ToString() + "' where tc_cvid=" + mod.tc_cvid;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;

                case 2:
                    QueryNuevaTabla = "Delete from psav_dev.tc_lob where tclb_id=" + mod.tclb_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
            }
            return Retorno;

        }
        public void SaveClient(Models.PSAVCrud.LobModel.LobModelTabla model)
        {

            //inserta TCC_name
            string QueryToInsert = "insert into psav_dev.tc_lob;(tclb_id,tclb_name,tclb_description,tclb_leader)" +
                "values('" + "','" + model.tclb_id + "','" + model.tclb_name + "','" + model.tclb_description + "','" + model.tclb_description + "',null,'" + "')";
            string IDClient = "";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_ID
            QueryToInsert = "insert into psav_dev.tc_lob;(tcct_id,trcp_data,tmp_id) values(1,'" + model.tclb_id + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_lob;(tcct_id,trcp_data,tmp_id) values(2,'" + model.tclb_name + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_lob;(tcct_id,trcp_data,tmp_id) values(3,'" + model.tclb_description + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_lob;(tcct_id,trcp_data,tmp_id) values(4,'" + model.tclb_leader + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_Type
            QueryToInsert = "insert into psav_dev.tc_lob;(tclb_id,tclb_name,tclb_description,tclb_leader) values(" + IDClient + ",'" + model.tclb_leader + "','" + "')";
            SaveWithoutValidation(QueryToInsert);

        }


        public void SaveWithoutValidation(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string SaveWithIDReturn(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return lastID;
        }

    }
    //Controla base de datod de comvta
    public class ClsQueryCrud2
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";

        public List<Models.PSAVCrud.Comvta.Comvtatabla> Getcateg2()
        {
            string TabSQL2 = "SELECT * FROM psav_dev.tc_comvta";
            List<Models.PSAVCrud.Comvta.Comvtatabla> Comv = new List<Models.PSAVCrud.Comvta.Comvtatabla>();//estamos referenciando Las variabes obtenidas en el modelo
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQL2, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read()) //Estamos haciendo una iteracion y como condicion estamos diciendo que mientras lea
            {
                Models.PSAVCrud.Comvta.Comvtatabla _ListVM = new Models.PSAVCrud.Comvta.Comvtatabla(); // estamos refernciando al archivo puesto en la primer parte y lo estamos guardando en una nueva parte de la memoria 
                _ListVM.tc_cvid = Convert.ToInt32(sdr.GetValue(0).ToString());//Estamos obteniendo los valores puestos en el modelo comvt y lo convertimos a string y las estamos guardando en la variable
                _ListVM.tc_cvtext = sdr.GetValue(1).ToString();
                _ListVM.tc_cvfee = sdr.GetValue(2).ToString();
                Comv.Add(_ListVM);
            }
            conn.Close();
            return Comv;
        }

        public string UpdateComvta(Models.PSAVCrud.Comvta.Comvtatabla mod, int oper)
        {

            string Retorno = "";
            string QueryNuevaTabla = "";
            switch (oper)
            {

                case 0:
                    QueryNuevaTabla = "insert into psav_dev.tc_comvta (tc_cvtext,tc_cvfee) values('" + mod.tc_cvtext.ToString() + "'," + mod.tc_cvfee.ToString() + ")";
                    Retorno = SaveWithIDReturn(QueryNuevaTabla);
                    break;
                case 1:
                    QueryNuevaTabla = "update psav_dev.tc_comvta set tc_cvtext='" + mod.tc_cvtext.ToString() + "', tc_cvfee='" + mod.tc_cvfee.ToString() + "' where tc_cvid=" + mod.tc_cvid;
                    //"update psav_dev.tc_category set tcc_name='" + mod.tcc_name.ToString().ToUpper() + "'
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
                case 2:
                    QueryNuevaTabla = "Delete from psav_dev.tc_comvta where tc_cvid=" + mod.tc_cvid;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
            }
            return Retorno;

        }
        public void SaveClient(Models.PSAVCrud.Comvta.Comvtatabla model)
        {

            //inserta TCC_name
            string QueryToInsert = "insert into psav_dev.tc_comvta;(tcc_id,tcc_name,tcc_type)" +
                "values('" + "','" + model.tc_cvid + "','" + model.tc_cvfee + "','" + model.tc_cvtext + "',null,'" + "')";
            string IDClient = "";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_ID
            QueryToInsert = "insert into psav_dev.psav_dev.tc_comvta;(tcct_id,trcp_data,tmp_id) values(1,'" + model.tc_cvid + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.psav_dev.tc_comvta;(tcct_id,trcp_data,tmp_id) values(2,'" + model.tc_cvtext + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.psav_dev.tc_comvta;(tcct_id,trcp_data,tmp_id) values(3,'" + model.tc_cvfee + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_Type
            QueryToInsert = "insert into psav_dev.psav_dev.tc_comvta;(tc_cvid,model.tc_cvfee,tc_cvtext) values(" + IDClient + ",'" + model.tc_cvfee + "','" + "')";
            SaveWithoutValidation(QueryToInsert);

        }

        public void SaveWithoutValidation(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string SaveWithIDReturn(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return lastID;
        }

    }
   
    //controla la BD de tabla nueva
    public class ClsQueryCrud
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";//estamos estableciendo conexión con BD
        
        public List<Models.PSAVCrud.SyncCrud.Tablanueva> Getcateg()
        {
            string TabSQL = "SELECT * FROM psav_dev.tc_category";//Le estamos indicando que tabla queremos de la BD
            List<Models.PSAVCrud.SyncCrud.Tablanueva> Cients = new List<Models.PSAVCrud.SyncCrud.Tablanueva>();//Aquí estamos creando una nueva tabla en la cual estamos insertando la tabla de clientes
            MySqlConnection conn = new MySqlConnection(con);//estamos estableciendo conexión con mySql
            MySqlCommand cmd = new MySqlCommand(TabSQL, conn); //estamos ejecutando el código SELECT FROM
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();//le estamos diciendo que lea los datos guardados en la base de datos
            while (sdr.Read()) //Estamos haciendo una iteracion y como condicion estamos diciendo que mientras lea
            {
                Models.PSAVCrud.SyncCrud.Tablanueva _ListVM = new Models.PSAVCrud.SyncCrud.Tablanueva(); // estamos refernciando al archivo puesto en la primer parte y lo estamos guardando en una nueva parte de la memoria 
                _ListVM.tcc_id = Convert.ToInt32( sdr.GetValue(0).ToString());//Estamos obteniendo los valores puestos en el modelo Syncrud y lo convertimos a string
                _ListVM.tcc_name = sdr.GetValue(1).ToString();
                _ListVM.tcc_type = sdr.GetValue(2).ToString();
               
                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }
     
        public string UpdateNuevatabla(Models.PSAVCrud.SyncCrud.Tablanueva mod, int oper)
        {

            string Retorno = "";
            string QueryNuevaTabla = "";
            switch (oper)
            {
                
                case 0:
                    QueryNuevaTabla = "insert into psav_dev.tc_category (tcc_name,tcc_type,tcc_id) values('" + mod.tcc_name.ToString().ToUpper() + "'," + mod.tcc_type + "," + mod.tcc_id + ")";
                    Retorno = SaveWithIDReturn(QueryNuevaTabla);
                    break;
                case 1:
                    QueryNuevaTabla = "update psav_dev.tc_category set tcc_name='" + mod.tcc_name.ToString().ToUpper() + "', tcc_type='" + mod.tcc_type.ToString() + "' where tcc_id=" + mod.tcc_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
               
                case 2:
                    QueryNuevaTabla = "Delete from psav_dev.tc_category where tcc_id=" + mod.tcc_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
            }
            return Retorno;

        }
        public void SaveClient(Models.PSAVCrud.SyncCrud.Tablanueva model)
        {

            //inserta TCC_name
            string QueryToInsert = "insert into psav_dev.tc_category;(tcc_name,tcc_id,tcc_type)" +
                "values('" + "','" + model.tcc_id + "','" + model.tcc_name + "','" + model.tcc_type + "',null,'" + "')";
            string IDClient = "";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_ID
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(1,'" + model.tcc_id + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(2,'" + model.tcc_name + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(3,'" + model.tcc_type + "'," + IDClient + ")";
            //inserta TCC_Type
            QueryToInsert = "insert into psav_dev.tc_category;(tcc_id,tcc_name,tcc_type) values(" + IDClient + ",'" + model.tcc_type + "','" + "')";
            SaveWithoutValidation(QueryToInsert);
           
        }

        public void SaveWithoutValidation(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string SaveWithIDReturn(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return lastID;
        }

    }

}


