﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetCorePSAV.Models
{
    public class NCRModel
    {
        #region catalogs
        public class DET
        {
            public string IDDet { get; set; }
            public string Nombre { get; set; }
        }
        public class SMgr
        {
            public string IDSM { get; set; }
            public string Nombre { get; set; }
        }
        public class NLocation
        {
            public string ParentID { get; set; }
            public string Nombre { get; set; }
        }
        public class NuLocation
        {
            public string ParentID { get; set; }
            public string Nombre { get; set; }
        }
        public class Etiqueta
        {
            public string IDEtiqueta { get; set; }
            public string Nombre { get; set; }
        }
        public class TipoEvento
        {
            public string IDTipoEvento { get; set; }
            public string Nombre { get; set; }
        }
        public class LBMotivo
        {
            public string IDMLB { get; set; }
            public string Nombre { get; set; }
        }
        public class sadic
        {
            public string IDSA { get; set; }
            public string Nombre { get; set; }
        }
        #endregion
        public class newCRView
        {
            public string DET { get; set; }
            [Display(Name="Sales Manager")]
            public string SMgr { get; set; }
            [Display(Name = "Nombre Location")]
            public string NLocation { get; set; }
            [Display(Name = "Número Location")]
            public string NuLocation { get; set; }
            [Display(Name = "Prospecto")]            
            public string prospecto { get; set; }
            [Display(Name = "Empresa")]
            public string empresa { get; set; }
            [Display(Name = "Correo Electrónico")]
            [DataType(DataType.EmailAddress,ErrorMessage ="Ingrese una dirección válida")]
            public string correo { get; set; }
            [Display(Name = "Teléfono")]
            public string telefono { get; set; }
            [Display(Name = "Etiqueta")]
            public string etiqueta { get; set; }
            [Display(Name = "Tipo de Evento")]
            public string tipoevento { get; set; }
            [Display(Name = "Nombre de Evento")]
            public string nombreevento { get; set; }
            [Display(Name = "Fecha Inicio")]
            [DataType(DataType.Date)]
            public string fechainicio { get; set; }
            [Display(Name = "Fecha de Término")]
            [DataType(DataType.Date)]
            public string fechafin { get; set; }
            [Display(Name = "Compañía AV")]
            public string AV { get; set; }
            [Display(Name = "Lost Business")]
            [DataType(DataType.Currency)]
            public string LB { get; set; }
            [Display(Name = "Motivo de Lost Business")]
            public string LBMotivo { get; set; }
            [Display(Name = "Servicios Adicionales")]
            public string servadic { get; set; }
            [Display(Name = "Servicios Adicionales")]
            public string sadic { get; set; }//array de serv adicionales
            [Display(Name = "Lugar próximo evento")]
            public string lpe { get; set; }
            [Display(Name = "Fecha próximo evento")]
            [DataType(DataType.Date)]
            public string fpe { get; set; }
            [Display(Name = "Comentarios")]
            public string comments { get; set; }
        }
    }
}
