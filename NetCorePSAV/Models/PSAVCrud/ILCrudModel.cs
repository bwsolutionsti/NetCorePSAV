using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePSAV.Models.PSAVCrud
{
    public class ILCrudModel
    {
        public class Categoria
        {
            public string ID { get; set; }
            public string Nombre { get; set; }
        }
        public class SubCategoria
        {
            public string ID { get; set; }
            public string Nombre { get; set; }
        }
    }
}
