﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GCCorePSAV.Data
{
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
                Models.PSAVCrud.SyncCrud.Tablanueva _ListVM = new Models.PSAVCrud.SyncCrud.Tablanueva(); //Creamos una lista llamada _ListVm que empieza a guardar los datos en la parte de abajo
                _ListVM.Tcc_id = Convert.ToInt32( sdr.GetValue(0).ToString());//Estamos obteniendo los valores de la Base de Datos
                _ListVM.Tcc_name = sdr.GetValue(1).ToString();
                _ListVM.Tcc_type = sdr.GetValue(2).ToString();
               
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
                    QueryNuevaTabla = "insert into psav_dev.tc_moneda (tcm_Tcc_id,tcm_Tcc_name,tcm_) values('" + mod.Tcc_id + "'," + mod.Tcc_name.ToString() + "," + mod.Tcc_type + ")";
                    Retorno = SaveWithIDReturn(QueryNuevaTabla);
                    break;
                case 1:
                    QueryNuevaTabla = "update psav_dev.tc_category; set tcm_name='" + mod.Tcc_id + "', tcm_change=" + mod.Tcc_name.ToString() + ", tcm_activo=" + mod.Tcc_type.ToString() + " where tcm_id=" + mod.Tcc_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
                case 2:
                    QueryNuevaTabla = "Delete from psav_dev.tc_category; where tcm_id=" + mod.Tcc_id;
                    SaveWithoutValidation(QueryNuevaTabla);
                    break;
            }
            return Retorno;

        }
        public void SaveClient(Models.PSAVCrud.SyncCrud.Tablanueva model)
        {

            //inserta TCC_name
            string QueryToInsert = "insert into psav_dev.tc_category;(tmp_Tcc_name,tmp_Tcc_ID,tmp_TCC_Type)" +
                "values('" + "','" + model.Tcc_id + "','" + model.Tcc_name + "','" + model.Tcc_type + "',null,'" + "')";
            string IDClient = "";
            SaveWithoutValidation(QueryToInsert);
            //inserta TCC_ID
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(1,'" + model.Tcc_id + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(2,'" + model.Tcc_name + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tc_category;(tcct_id,trcp_data,tmp_id) values(3,'" + model.Tcc_type + "'," + IDClient + ")";
            //inserta TCC_Type
            QueryToInsert = "insert into psav_dev.tc_category;(tmp_id,tmpa_domicilio,tmpa_domiciliofiscal) values(" + IDClient + ",'" + model.Tcc_type + "','" + "')";
            SaveWithoutValidation(QueryToInsert);
            //INSERTA CLIENTE
            QueryToInsert = "insert into psav_dev.tc_category;(tmp_id,tmc_userins,tmc_dateins) values(" + IDClient + ",1,now())";
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


