using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    
    public class EPTModel
    {
        [Display(Name ="# EPT")]
        [Required]
        public string EPTNumber { get; set; }
        [Display(Name = "Cliente")]
        public string IDClient { get; set; }
        [Display(Name = "Razón Social")]
        [DataType(DataType.MultilineText)]
        public string RazonSocial { get; set; }
        [Display(Name = "Domicilio")]
        [DataType(DataType.MultilineText)]
        public string DomicilioComercial { get; set; }
        [Display(Name = "Domicilio Fiscal")]
        [DataType(DataType.MultilineText)]
        public string DomicilioFiscal { get; set; }
        [Display(Name = "R.F.C.")]
        public string RFC { get; set; }
        [Display(Name = "Nombre de contacto")]
        public string ContactClientName { get; set; }
        [Display(Name = "Puesto")]
        public string Job { get; set; }
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
        [Display(Name = "Ext.")]
        public string Ext { get; set; }
        [Display(Name = "Móvil")]
        public string Mobile { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Email Alterno")]
        public string AEMail { get; set; }
        [Display(Name = "Fax")]
        public string Fax { get; set; }
        [Display(Name = "Nombre del evento")]        
        public string EventName { get; set; }
        [Display(Name = "Fecha de Montaje")]
        [DataType(DataType.Date)]
        public string MountDate { get; set; }
        [Display(Name = "Hora de Montaje")]
        [DataType(DataType.Time)]
        public string MountHour { get; set; }
        [Display(Name = "Contacto en sitio")]
        public string PlaceContact { get; set; }
        [Display(Name = "Celular")]
        public string PlaceMobile { get; set; }
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public string StartDate { get; set; }
        [Display(Name = "Hora de inicio")]
        [DataType(DataType.Time)]
        public string StartHour { get; set; }
        [Display(Name = "Fecha de termino")]
        [DataType(DataType.Date)]
        public string FinishDate { get; set; }
        [Display(Name = "Hora de termino")]
        [DataType(DataType.Time)]
        public string FinishHour { get; set; }
        [Display(Name = "Lugar")]
        public string Place { get; set; }
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Nombre")]
        public string SMName { get; set; }
        [Display(Name = "Puesto")]
        public string SMJob { get; set; }
        [Display(Name = "Email")]
        public string SMEmail { get; set; }
        [Display(Name = "Telefono")]
        public string SMPhone { get; set; }
        [Display(Name = "Nombre")]
        public string PMName { get; set; }
        [Display(Name = "Celular")]
        public string PMMobile { get; set; }
        [Display(Name = "Email")]
        public string PMEmail { get; set; }
        [Display(Name = "Ubicación")]
        public string PMLocation { get; set; }
        public string IDEmpresa { get; set; }
        public List<Models.PSAVCrud.ClientModel.ClientAutoComplete> Clients { get; set; }
        public string IDEvent { get; set; }
        public EPTModel()
        {
            Data.clsQuery con = new Data.clsQuery();
             Clients = con.GetAutoClients();
        }
        public class EPTConsult
        {
            public string FolioEPT { get; set; }
            public string EventName { get; set; }
            public string Client { get; set; }
            public string StartDate { get; set; }
        }
    }
}
