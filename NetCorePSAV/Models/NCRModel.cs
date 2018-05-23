using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetCorePSAV.Models
{
    public class NCRModel
    {
        public class NCRReporte
        {
            public string nloc { get; set; }
            public string numloc { get; set; }
            public string region { get; set; }
            public string ciudad { get; set; }
            public string det { get; set; }
            public string smgr { get; set; }
            public string prospecto { get; set; }
            public string empresa { get; set; }
            public string correo { get; set; }
            public string telefono { get; set; }
            public string etiqueta { get; set; }
            public string tipoevento { get; set; }
            public string nomevento { get; set; }
            public string finicio { get; set; }
            public string ffin { get; set; }
            public string ciaav { get; set; }
            public string lb { get; set; }
            public string lbm { get; set; }
            public string servadic { get; set; }
            public string lpe { get; set; }
            public string fpe { get; set; }
            public string comentarios { get; set; }
        }
        public class PreConsultaNCR
        {
            public string IDNCR { get; set; }
            public string Evento { get; set; }
            public string Empresa { get; set; }
            public string Location { get; set; }
            public string LB { get; set; }
            public string LBMotivo { get; set; }
        }
        public class SearchNCR
        {
            public string finicio { get; set; }
            public string ffin { get; set; }
            public string evento { get; set; }
            public string empresa { get; set; }
            public string location { get; set; }
            public string prospecto { get; set; }
            public string lbmotivo { get; set; }
        }
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
            public string ID { get; set; }
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
