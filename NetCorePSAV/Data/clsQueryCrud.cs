﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GCCorePSAV.Data
{
    //controla la BD de tabla nueva
    public class ClsQueryCrud
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";
        
        public List<Models.PSAVCrud.SyncCrud.Tablanueva> Getcateg()
        {
            string TabSQL = "SELECT * FROM psav_dev.tc_category";
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


