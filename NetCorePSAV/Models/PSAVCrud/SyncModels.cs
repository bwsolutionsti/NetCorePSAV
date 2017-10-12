using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class SyncModels
    {
        public class CoinModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public decimal Change { get; set; }
            public int Active { get; set; }
        }
        public class UsersModel
        {
            public string ID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Active { get; set; }
            public string Persona { get; set; }
            public string Expira { get; set; }
        }
        public class NewUser
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string UserName { get; set; }
            [Required ]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [DataType(DataType.Date)]
            public string Expira { get; set; }
            public string PrimerNombre { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoNombre { get; set; }
            public string SegundoApellido { get; set; }
        }
    }
}
