using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePSAV.Models
{
    public class ILNewModel
    {
        public class Category
        {
            public string ParentID { get; set; }
            public string Nombre { get; set; }
            public string Tipo { get; set; }
        }
        public class SubCategory
        {
            public string SCID { get; set; }
            public string Nombre { get; set; }
            public string ParentID { get; set; }
        }
        public class Items
        {
            public string ParentID { get; set; }
            public string SCID { get; set; }
            public string descripcion { get; set; }
            public string comentarios { get; set; }
            public string incluye { get; set; }
            public string precio { get; set; }
            public string ID { get; set; }
        }
        public class ItemDetails
        {
            public string ID { get; set; }
            public string Categoria { get; set; }
            public string SubCategoria { get; set; }
            public string Descripcion { get; set; }
            public string Incluye { get; set; }
            public string PrecioUnit { get; set; }
            public string Cantidad { get; set; }
        }
        public class ILGrid
        {
            public string ID { get; set; }
            public string Categoria { get; set; }
            public string Subcategoria { get; set; }
            public string Descripcion { get; set; }
            public string Incluye { get; set; }
            public string PrecioUnit { get; set; }
            public string Cantidad { get; set; }
        }
        public class ILSalon
        {

        }
    }
}
