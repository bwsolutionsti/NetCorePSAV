using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NetCorePSAV.Models
{
    public class NCRModel
    {
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
            public string LB { get; set; }
            [Display(Name = "Motivo de Lost Business")]
            public string LBMotivo { get; set; }
            [Display(Name = "Servicios Adicionales")]
            public string servadic { get; set; }
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
